using Img.DataService.Infrastructure;
using Img.Model.Dtos;
using Img.Model.Models;
using Img.Nlog;
using Img.SaasAPIRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Img.Config;
using System.IO;
using Img.Model.Job;
using ImageMagick;
using Img.Config.config;
using Img.Nlog.Imp;
using Img.DataService.Imp;
using BitMiracle.LibTiff.Classic;

namespace Img.DataService.Infrastructure
{
	public abstract class DataServiceBase<TSource, TDestination> : DtoService<TSource, TDestination>
	{
		#region Property
		protected Tenant tenant;
		protected JobLog joblog;

		protected IAccountService accountService;
		protected IAppointmentService appointmentService;
		protected IImgUploadService imgUploadService;
		protected IPatientService patientService;
		protected ILogger logger;
		protected JobLogData.JobLogDataService joblogService = new JobLogData.JobLogDataService();

		protected virtual ILogger JobLogger
		{
			get
			{
				return null;
			}
		}

		#endregion

		#region Ctor
		public DataServiceBase()
		{

		}
		public DataServiceBase(IAccountService accountService,
			IAppointmentService appointmentService,
			IImgUploadService imgUploadService,
			IPatientService patientService,
			ILogger Logger)
		{
			this.accountService = accountService;
			this.appointmentService = appointmentService;
			this.imgUploadService = imgUploadService;
			this.patientService = patientService;
			this.logger = Logger;

			this.tenant = GetTenantInfo();

			Login();
			CheckDomain();
		}
		#endregion

		#region Method
		public virtual void Login()
		{
			if (tenant == null)
			{
				logger.Info("Tenant信息为空");
				return;
			}

			LogonDto logon = new LogonDto();
			logon.AccountName = tenant.UserName;
			logon.Password = tenant.UserPwd;
			logon.TenantDomain = tenant.Domain;

			var tup = accountService.Logon(logon);
			if (tup.Item1)
			{
				tenant.Token = tup.Item2;
				logger.Info(string.Format("Token：{0}", tenant.Token));
			}
		}

		public virtual void CheckDomain()
		{
			try
			{
				if (tenant == null)
				{
					logger.Info("Tenant信息为空");
					return;
				}

				var domian = accountService.CheckDomain(tenant).FirstOrDefault();
				if (domian != null)
				{
					tenant.TenantId = domian.TenantId;
					tenant.OfficeId = domian.OfficeId;

					logger.Info(string.Format("TenantId：{0}，OfficeId：{1}", domian.TenantId, domian.OfficeId));
				}
			}
			catch (Exception ex)
			{
				logger.Error(string.Format("未找到domain为{0},office关键字为{1}的诊所 - ErrorInfo：{2}",
					tenant.Domain, tenant.OfficeName, ex.Message));
			}
		}

		public virtual bool ValidateToken()
		{
			var logout = new LogOutDto();
			logout.Domain = tenant.Domain;
			logout.LoginName = tenant.UserName;
			logout.OfficeName = tenant.OfficeName;
			return accountService.LogOut(logout);
		}

		public virtual void Upload(IEnumerable<MedicalImage> imageList)
		{
			var result = new List<Tuple<Appointment, JobLog, string, string, int>>();

			AppointmentSearch apptSearch;
			Appointment appt;
			AppointmentDto apptDto;
			
			foreach (var item in imageList)
			{
				try
				{
					if(item.BrandType.Equals("EasyDent") &&
						joblogService.GetImgById(item.VideoNum))
					{
						continue;
					}
					if (!File.Exists(item.FilePath))
					{
						WriteLog(joblog, item, "没有找到影像文件");
						continue;
					}

					var patient = patientService.GetPatientByBlh(new PatientSearch()
					{
						Blh = item.PatientId,
						OfficeId = tenant.OfficeId,
						TenantId = tenant.TenantId,
						Token = tenant.Token
					});

					if (patient == null)
					{
						WriteLog(joblog, item, "未在Saas中匹配到该患者");
						continue;
					}
					//如果有预约取第一个,没有则创建
					apptSearch = new AppointmentSearch
					{
						TenantId = tenant.TenantId,
						OfficeId = tenant.OfficeId,
						StartTime = item.CheckTime,
						EndTime = item.CheckTime.AddHours(23).AddMinutes(59).AddSeconds(59),
						PrivateId = item.PatientId,
						Token = tenant.Token
					};

					apptDto = appointmentService.GetAppointment(apptSearch).FirstOrDefault();
					appt = new Appointment();
					appt.PatientId = patient.Id;
					appt.Token = tenant.Token;
					appt.TenantId = tenant.TenantId;
					appt.OfficeId = tenant.OfficeId;
					if (apptDto == null)
					{
						appt.StartTime = item.CheckTime;
						appt.EndTime = item.CheckTime.AddMinutes(30);
						appt.Notes = "由当天影像记录创建的预约";
						appt.SourceType = "普通";
						appt.RecordCreatedTime = appt.StartTime;
						appt.Id = appointmentService.CreateAppointment(appt);
					}
					else
					{
						appt.PatientId = apptDto.PatientId;
						appt.Id = apptDto.Id;
					}

					joblog = new JobLog();
					joblog.ImgId = item.VideoNum;
					joblog.ImgCreateTime = item.CheckTime;
					joblog.PatientName = patient.Name;
					joblog.PrivateId = patient.PrivateId;
					joblog.BrandType = item.BrandType;
					logger.Info(string.Format("预约Id：{0};影像Id：{1};文件名：{2}", appt.Id, joblog.ImgId, item.FilePath));

					string uploadFileName = item.FilePath;
					//如果希诺德上传影像为tiff,则Vertical Flip
					if (item.BrandType.Equals("Sidexis"))
					{
						if (IsTiff(item.FilePath))
						{
							uploadFileName = ReplaceTiffName(item.FilePath);
							if (uploadFileName != string.Empty)
							{
								TiffVerticalFlip(item.FilePath, uploadFileName);
							}
						}
					}
					result.Add(new Tuple<Appointment, JobLog, string, string, int>(appt, joblog, uploadFileName, "", (int)item.Category));
				}
				catch (Exception errorlog)
				{
					WriteLog(joblog, item, errorlog.Message);
				}
			}
			logger.Info(string.Format("成功匹配到的影像记录：{0}", result.Count));

			var successNum = 0;
			var newFile = "";
			foreach (var item in result)
			{
				var flag = false;
				if (!ValidateToken())
				{
					logger.Info("Token已失效,影像上传终止!");
					return;
				}
				try
				{
					logger.Info(string.Format("开始上传:{0}", item.Item3));
					item.Item2.UploadTime = DateTime.Now;
					flag = imgUploadService.UploadImage(item.Item1, item.Item3, item.Item4, tenant.Token, item.Item5);
					if (flag)
					{
						logger.Info(string.Format("上传成功:{0}", item.Item3));
					}
				}
				catch (Exception)
				{
					try
					{
						logger.Info(string.Format("{0}上传失败,将复制该影像并修改后缀名为.jpg再度上传...", item.Item3));
						newFile = string.Concat(item.Item3.Split('.')[0], ".jpg");
						using (MagickImage image = new MagickImage(item.Item3))
						{
							image.Write(newFile);
						}
						File.SetAttributes(newFile, FileAttributes.Normal);
						if (!File.Exists(newFile))
						{
							logger.Error(string.Format("复制jpg图片失败:{0}", item.Item3));
						}
						flag = imgUploadService.UploadImage(item.Item1, newFile, item.Item4, tenant.Token, item.Item5);
						if (flag)
						{
							logger.Info(string.Format("上传成功:{0}", newFile));
						}
					}
					catch (Exception ex)
					{
						item.Item2.ErrorLog = ex.Message;
						logger.Error(string.Format("上传失败:{0}", ex.Message));
					}
					finally
					{
						File.Delete(newFile);
						if (File.Exists(newFile))
						{
							logger.Error(string.Format("删除jpg图片失败:{0}", newFile));
						}
					}
				}
				item.Item2.UploadStatus = flag ? UploadStatus.Successed : UploadStatus.Failed;
				JobLogger.Info(item.Item2);
				if (item.Item2.UploadStatus.Equals(UploadStatus.Successed))
					successNum++;
			}
			logger.Info(string.Format("上传成功的影像记录：{0}", successNum));
		}

		private Tenant GetTenantInfo()
		{
			if (string.IsNullOrEmpty(ConfigManager.Instance.Global.Domain))
			{
				return null;
			}

			var tenant = new Tenant()
			{
				Domain = ConfigManager.Instance.Global.Domain,
				OfficeName = ConfigManager.Instance.Global.OfficeName,
				UserName = ConfigManager.Instance.Global.UserName,
				UserPwd = ConfigManager.Instance.Global.UserPwd
			};

			return tenant;
		}

		protected void WriteLog(JobLog joblog, MedicalImage item, string errorlog)
		{
			joblog = new JobLog();
			joblog.ImgId = item.VideoNum;
			joblog.ImgCreateTime = item.CheckTime;
			joblog.PatientName = item.PatientName;
			joblog.PrivateId = item.PatientId;
			joblog.UploadStatus = UploadStatus.Failed;
			joblog.UploadTime = DateTime.Now;
			joblog.ErrorLog = errorlog;
			joblog.BrandType = item.BrandType;
			JobLogger.Info(joblog);
		}
		#endregion

		private bool IsTiff(string uploadFileName)
		{
			string postFix = uploadFileName.Substring(uploadFileName.Length - 4, 4);
			if (postFix.ToLower() == ".tif")
			{
				return true;
			}

			return false;

		}


		private string ReplaceTiffName(string inputFileName)
		{
			try
			{
				int index = inputFileName.ToLower().IndexOf(".tif");
				if (index != -1)
				{
					string s = inputFileName.Substring(index, 4);
					inputFileName = inputFileName.Replace(s, "_lc.tif");
					return inputFileName;
				}

				return string.Empty;
			}
			catch (Exception errorlog)
			{
				return string.Empty;
			}

		}


		private void TiffVerticalFlip(string inputFileName, string outputFileName)
		{
			using (Tiff input = Tiff.Open(inputFileName, "r"))
			{
				using (Tiff output = Tiff.Open(outputFileName, "w"))
				{
					for (short page = 0; page < input.NumberOfDirectories(); page++)
					{
						input.SetDirectory(page);
						output.SetDirectory(page);

						int width = input.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
						int height = input.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
						int samplesPerPixel = input.GetField(TiffTag.SAMPLESPERPIXEL)[0].ToInt();
						int bitsPerSample = input.GetField(TiffTag.BITSPERSAMPLE)[0].ToInt();
						int photo = input.GetField(TiffTag.PHOTOMETRIC)[0].ToInt();


						int[] raster = new int[width * height];
						input.ReadRGBAImageOriented(width, height, raster, Orientation.TOPLEFT);


						output.SetField(TiffTag.IMAGEWIDTH, width);
						output.SetField(TiffTag.IMAGELENGTH, height);
						output.SetField(TiffTag.SAMPLESPERPIXEL, 3);
						output.SetField(TiffTag.BITSPERSAMPLE, 8);
						output.SetField(TiffTag.ROWSPERSTRIP, height);
						output.SetField(TiffTag.PHOTOMETRIC, photo);
						output.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG);
						output.SetField(TiffTag.COMPRESSION, Compression.DEFLATE);
						output.SetField(TiffTag.ORIENTATION, Orientation.BOTLEFT);

						byte[] strip = rasterToRgbBuffer(raster);
						output.WriteEncodedStrip(0, strip, strip.Length);

						output.WriteDirectory();
					}
				}
			}
		}

		private static byte[] rasterToRgbBuffer(int[] raster)
		{
			byte[] buffer = new byte[raster.Length * 3];
			for (int i = 0; i < raster.Length; i++)
				Buffer.BlockCopy(raster, i * 4, buffer, i * 3, 3);

			return buffer;
		}

		private static int[] rotate(int[] buffer, int angle, ref int width, ref int height)
		{
			int rotatedWidth = width;
			int rotatedHeight = height;
			int numberOf90s = angle / 90;
			if (numberOf90s % 2 != 0)
			{
				int tmp = rotatedWidth;
				rotatedWidth = rotatedHeight;
				rotatedHeight = tmp;
			}

			int[] rotated = new int[rotatedWidth * rotatedHeight];

			for (int h = 0; h < height; ++h)
			{
				for (int w = 0; w < width; ++w)
				{
					int item = buffer[h * width + w];
					int x = 0;
					int y = 0;
					switch (numberOf90s % 4)
					{
						case 0:
							x = w;
							y = h;
							break;

						case 1:
							x = (height - h - 1);
							y = (rotatedHeight - 1) - (width - w - 1);
							break;

						case 2:
							x = (width - w - 1);
							y = (height - h - 1);

							break;

						case 3:
							x = (rotatedWidth - 1) - (height - h - 1);
							y = (width - w - 1);
							break;
					}

					rotated[y * rotatedWidth + x] = item;
				}
			}

			width = rotatedWidth;
			height = rotatedHeight;
			return rotated;
		}

	}
}