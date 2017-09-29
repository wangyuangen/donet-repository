using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Img.Config.config;
using Img.Model.Models;

namespace Img.SaasAPIRequest
{
	public class ImgUploadRequest
	{
		private static Img.Nlog.Imp.Logger log = new Nlog.Imp.Logger();
        private static string ImgUploadHostUri = ConfigManager.Instance.Global.ImgUploadHostUri;

		public bool UploadImage(Appointment appt, string filePath, string note, string _apiToken, int DefaultCategory = 4)
		{
            string urlStr = ImgUploadHostUri;
			var patientId = appt.PatientId;
			var appointId = appt.Id;
			var imagePath = filePath;
			// We assume all images are jpg
			var categoryId = DefaultCategory;

			//Upload image
			var wr = WebRequest.CreateHttp(urlStr + "/api/v1/image/upload?appointmentId=" + appointId + "&categoryId=" + categoryId.ToString()) as HttpWebRequest;
			wr.Method = "POST";
			wr.KeepAlive = false;
			wr.Timeout = 360000;
			wr.Headers.Add("Authorization", _apiToken);
			wr.Headers.Add("Content-Disposition", string.Format("attachment; filename=\"{0}\"", Path.GetFileName(filePath)));
			wr.ContentType = "image/jpeg";
			wr.ProtocolVersion = HttpVersion.Version10;

			JObject jObject = null;
			var isDicom = false;

			using (Stream rs = wr.GetRequestStream())
			{
				if (!File.Exists(imagePath))
				{
					var temp = string.Format("pratentId:{0},无文件影像路径：{1}", appt.PatientId, imagePath);
					return false;
				}

				byte[] fileBuffer = new byte[4096];
				int bytesRead = 0;

				FileStream imgfs = File.Open(imagePath, FileMode.Open);
				if (imgfs.Length == 0)
				{
					var temp = string.Format("pratentId:{0},影像文件大小为0,文件路径：{1}", appt.PatientId, imagePath);
					return false;
				}
				while ((bytesRead = imgfs.Read(fileBuffer, 0, fileBuffer.Length)) != 0)
				{
					rs.Write(fileBuffer, 0, bytesRead);
				}

				try
				{
					using (var result = wr.GetResponse() as HttpWebResponse)
					{
						using (StreamReader reader = new StreamReader(result.GetResponseStream(), Encoding.UTF8))
						{
							isDicom = IsDicom(imagePath);
							if (!isDicom)
							{
								jObject = JsonConvert.DeserializeObject(reader.ReadToEnd()) as JObject;
								jObject["comment"] = note; //use to update comments
							}
						}
						result.Close();
					}
				}
				catch (Exception ex)
				{
					log.Error(ex.Message);
				}
				if (imgfs != null)
				{
					imgfs.Close();
				}
				rs.Close();
			}

			//不是影像的图片要更新SaaS的image表记录
			if (!isDicom)
			{
				//Update comments
				var wr2 = WebRequest.CreateHttp(urlStr + "/api/v1/imaging/update") as HttpWebRequest;
				wr2.Method = "POST";
				wr2.KeepAlive = false;
				wr2.Timeout = 360000;
				wr2.Headers.Add("Authorization", _apiToken);
				wr2.ContentType = "application/json; charset=UTF-8";
				using (Stream rs2 = wr2.GetRequestStream())
				{
					var bytes = Encoding.UTF8.GetBytes(jObject.ToString());
					rs2.Write(bytes, 0, bytes.Length);

					using (var result2 = wr2.GetResponse() as HttpWebResponse)
					{
						result2.Close();
					}
				}
			}

			Thread.Sleep(100);
			return true;
		}

		//判断上传的是否为影像文件
		private bool IsDicom(string fileName)
		{
			fileName = fileName.ToLower();
			return (fileName.EndsWith(".dcm") || fileName.EndsWith(".dic") || fileName.EndsWith(".dicom") || (!fileName.Contains(".")));
		}
	}
}
