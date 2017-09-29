using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Img.JobLogData;
using Img.SaasAPIRequest;
using System.Configuration;
using System.IO;
using ImageMagick;
using Img.Model.Dtos;
using Img.Model.Models;
using Img.Model.Job;

namespace Img.SironaData
{
	public abstract class ImgUploadBase
	{
		protected string domain;		//Saas账号
		protected string officeKeywords; 	//诊所关键字
		protected string directory;		//影像根目录
		protected int category;   		//影像类型
		//protected int total;	 			
		protected int officeId;		
		protected Guid tenantId;
		protected string Token;
		protected string userName;
		protected string userPwd;

		private static Img.Nlog.Imp.Logger logger = new Nlog.Imp.Logger();

		private static AccountRequest AccRequest = new AccountRequest();

		private static PatientRequest PtRequest = new PatientRequest();

		private static AppointmentApiRequest ApptRequest = new AppointmentApiRequest();

		private static ImgUploadRequest ImgRequest = new ImgUploadRequest();

        public ImgUploadBase()
        {

        }
		public ImgUploadBase(string domain,string officeKeywords,string directory,int category,string username,string userpwd)
		{
			this.userName = username;
			this.userPwd = userpwd;
			this.domain = domain;
			this.officeKeywords = officeKeywords;
			this.directory = directory;
			this.category = category;
			//this.total = total;
			this.Token = GetToken();
			var domainDto = GetTenant();
			if(domainDto!=null)
			{
				this.tenantId = domainDto.TenantId;
				this.officeId = domainDto.OfficeId;
			}
		}

		/// <summary>
		/// 校验Token是否已失效
		/// </summary>
		/// <returns></returns>
		protected bool CheckToken()
		{
			var logout = new LogOutDto();
			logout.Domain = domain;
			logout.LoginName = userName;
			logout.OfficeName = officeKeywords;
			return AccRequest.LogOut(logout);
		}

		/// <summary>
		/// 获取Token
		/// </summary>
		/// <returns></returns>
		protected string GetToken()
		{
			LogonDto logon = new LogonDto();
			logon.AccountName = userName;
			logon.Password = userPwd;
			logon.TenantDomain = domain;

			var tup = AccRequest.Logon(logon);
			if(tup.Item1)
			{
				return tup.Item2;
			}
			return "";
		}

		/// <summary>
		/// 校验诊所
		/// </summary>
		/// <returns></returns>
		protected DomainDto GetTenant()
		{
			try
			{
				Tenant tenant = new Tenant();
				tenant.Domain = domain;
				tenant.OfficeName = officeKeywords;
				tenant.Token = Token;
				return AccRequest.CheckDomain(tenant).FirstOrDefault();
			}
			catch (Exception ex)
			{
				logger.Error(string.Format("未找到domain为{0},office关键字为{1}的诊所 - ErrorInfo：{2}",domain,officeKeywords,ex.Message));
				return null;
			}
		}

		/// <summary>
		/// 根据病历号查询患者
		/// </summary>
		/// <param name="blh"></param>
		/// <returns></returns>
		protected Patient GetSaasPatients(string blh)
		{
			if(string.IsNullOrWhiteSpace(blh))
			{
				return null;
			}
			PatientSearch ps = new PatientSearch
			{
				 Blh = blh,
				 OfficeId = this.officeId,
				 TenantId = this.tenantId,
				 Token = Token
			};
			return PtRequest.GetPatientByBlh(ps);
		}

		/// <summary>
		/// 创建预约
		/// </summary>
		/// <param name="appt"></param>
		/// <returns></returns>
		protected int InsertAppointment(Appointment appt)
		{
			return ApptRequest.CreateAppointment(appt);
		}

		/// <summary>
		/// 查询预约
		/// </summary>
		/// <param name="search"></param>
		/// <returns></returns>
		protected AppointmentDto GetAppointment(AppointmentSearch search)
		{
			var apptDtos = ApptRequest.GetAppointment(search);
			if(apptDtos!=null && apptDtos.Count>0)
			{
				return apptDtos.First();
			}
			return null;
		}

		/// <summary>
		/// 影像上传
		/// </summary>
		/// <param name="results"></param>
		protected void Uploading(List<Tuple<Appointment, JobLog, string, string, int>> results)
		{
			var joblogService = new JobLogDataService();
			var successNum = 0;
			var newFile = "";
			foreach (var item in results)
			{
				var flag = false;
				if (!CheckToken())
				{
					logger.Info("Token已失效,影像上传终止!");
					return;
				}
				try
				{
					logger.Info(string.Format("开始上传:{0}", item.Item3));
					item.Item2.UploadTime = DateTime.Now;
					flag = ImgRequest.UploadImage(item.Item1, item.Item3, item.Item4, Token, item.Item5);
					if(flag)
					{
						logger.Info(string.Format("上传成功:{0}", item.Item3));
					}
				}
				catch (Exception)
				{
					try
					{
						logger.Info(string.Format("{0}上传失败,将复制该影像并修改后缀名为.jpg再度上传...",item.Item3));
						newFile = string.Concat(item.Item3.Split('.')[0], ".jpg");
						using(MagickImage image = new MagickImage(item.Item3))
						{
							image.Write(newFile);
						}
						File.SetAttributes(newFile,FileAttributes.Normal);
						if (!File.Exists(newFile))
						{
							logger.Error(string.Format("复制jpg图片失败:{0}", item.Item3));
						}
						flag = ImgRequest.UploadImage(item.Item1, newFile, item.Item4, Token, item.Item5);
						if (flag)
						{
							logger.Info(string.Format("上传成功:{0}", newFile));
						}
					}
					catch (Exception ex)
					{
						item.Item2.ErrorLog = ex.Message;
						logger.Error(string.Format("上传失败:{0}",ex.Message));
					}
					finally
					{
						File.Delete(newFile);
						if(File.Exists(newFile))
						{
							logger.Error(string.Format("删除jpg图片失败:{0}",newFile));
						}
					}
				}
				item.Item2.UploadStatus = flag ? UploadStatus.Successed : UploadStatus.Failed;
				//joblogService.InsertJobLog(item.Item2,);
				if (item.Item2.UploadStatus.Equals(UploadStatus.Successed))
					successNum++;
			}
			logger.Info(string.Format("上传成功的影像记录：{0}", successNum));
		}

	}
}
