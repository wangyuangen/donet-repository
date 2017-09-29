using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
using Img.Config;
using Img.Model.Models;
using Img.Config.config;

namespace Img.SaasAPIRequest.HttpHelper
{
	public class httpHelper
	{
		private static Img.Nlog.Imp.Logger logger = new Nlog.Imp.Logger();

		public static string domainUrl = ConfigManager.Instance.Global.ImgWepApiHostUrl.EndsWith("/")==true
            ? ConfigManager.Instance.Global.ImgWepApiHostUrl : ConfigManager.Instance.Global.ImgWepApiHostUrl+"/";

		public static Res PostRequest<Req, Res>(string address, Req req)
		   where Req : RequestWithToken
		{
			string srcString = "";
			Res model = (Res)Activator.CreateInstance(typeof(Res));

			try
			{
				CookieContainer cookieContainer = new CookieContainer();
				// 将提交的字符串数据转换成字节数组 
				string json = JsonConvert.SerializeObject(req);
				byte[] postData = Encoding.UTF8.GetBytes(json);
				// 设置提交的相关参数 
				HttpWebRequest request = WebRequest.Create(domainUrl + address) as HttpWebRequest;
				Encoding myEncoding = Encoding.GetEncoding("gb2312");
				request.Method = "POST";
				request.KeepAlive = false;
				request.AllowAutoRedirect = true;
				request.ContentType = "application/json";
				request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR  2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR  3.0.4506.2152; .NET CLR 3.5.30729)";
				request.CookieContainer = cookieContainer;
				request.ContentLength = postData.Length;
				request.Headers.Add("Authorization", req.Token);

				// 提交请求数据 
				System.IO.Stream outputStream = request.GetRequestStream();
				outputStream.Write(postData, 0, postData.Length);
				outputStream.Close();
				HttpWebResponse response;
				Stream responseStream;
				StreamReader reader;

				response = request.GetResponse() as HttpWebResponse;

				responseStream = response.GetResponseStream();
				reader = new System.IO.StreamReader(responseStream, Encoding.UTF8);
				srcString = reader.ReadToEnd();
				reader.Close();

				model = JsonConvert.DeserializeObject<Res>(srcString);
			}
			catch (Exception ex)
			{
				logger.Error(ex);
				model = default(Res);
			}

			return model;
		}

		public static bool boolRequest(string address,string json)
		{
			bool flag = false;

			try
			{
				CookieContainer cookieContainer = new CookieContainer();
				byte[] postData = Encoding.UTF8.GetBytes(json);
				// 设置提交的相关参数 
				HttpWebRequest request = WebRequest.Create(domainUrl + address) as HttpWebRequest;
				Encoding myEncoding = Encoding.GetEncoding("gb2312");
				request.Method = "POST";
				request.KeepAlive = false;
				request.AllowAutoRedirect = true;
				request.ContentType = "application/json";
				request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR  2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR  3.0.4506.2152; .NET CLR 3.5.30729)";
				request.CookieContainer = cookieContainer;
				request.ContentLength = postData.Length;

				// 提交请求数据 
				System.IO.Stream outputStream = request.GetRequestStream();
				outputStream.Write(postData, 0, postData.Length);
				outputStream.Close();
				HttpWebResponse response;
				Stream responseStream;
				StreamReader reader;
				response = request.GetResponse() as HttpWebResponse;

				responseStream = response.GetResponseStream();
				reader = new System.IO.StreamReader(responseStream, Encoding.UTF8);
				var srcString = reader.ReadToEnd();
				reader.Close();
				flag = Convert.ToBoolean(srcString);
			}
			catch (Exception ex)
			{
				logger.Error(ex);
			}
			return flag;
		}

		public static string PostRequest(string address, string json)
		{
			string token = "";

			try
			{
				CookieContainer cookieContainer = new CookieContainer();
				// 将提交的字符串数据转换成字节数组 
				//byte[] postData = Encoding.UTF8.GetBytes("[\"str\":\"1234\"]");
				byte[] postData = Encoding.UTF8.GetBytes(json);
				// 设置提交的相关参数 
				HttpWebRequest request = WebRequest.Create(domainUrl + address) as HttpWebRequest;
				Encoding myEncoding = Encoding.GetEncoding("gb2312");
				request.Method = "POST";
				request.KeepAlive = false;
				request.AllowAutoRedirect = true;
				request.ContentType = "application/json";
				request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR  2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR  3.0.4506.2152; .NET CLR 3.5.30729)";
				request.CookieContainer = cookieContainer;
				request.ContentLength = postData.Length;

				// 提交请求数据 
				System.IO.Stream outputStream = request.GetRequestStream();
				outputStream.Write(postData, 0, postData.Length);
				outputStream.Close();
				HttpWebResponse response;
				Stream responseStream;
				StreamReader reader;
				response = request.GetResponse() as HttpWebResponse;

				responseStream = response.GetResponseStream();
				reader = new System.IO.StreamReader(responseStream, Encoding.UTF8);
				string srcString = reader.ReadToEnd();
				reader.Close();
				token = srcString;
			}
			catch (Exception ex)
			{
				logger.Error(ex);
			}

			return token;
		}

		public static Tuple<bool,string> tokenRequest(string address, string json)
        {
			var flag = false;
			var token = "";
			var tuple = new Tuple<bool, string>(flag,token);
            try
            {
                CookieContainer cookieContainer = new CookieContainer();
                // 将提交的字符串数据转换成字节数组 
                //byte[] postData = Encoding.UTF8.GetBytes("[\"str\":\"1234\"]");
                byte[] postData = Encoding.UTF8.GetBytes(json);
                // 设置提交的相关参数 
                HttpWebRequest request = WebRequest.Create(domainUrl + address) as HttpWebRequest;
                Encoding myEncoding = Encoding.GetEncoding("gb2312");
                request.Method = "POST";
                request.KeepAlive = false;
                request.AllowAutoRedirect = true;
                request.ContentType = "application/json";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR  2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR  3.0.4506.2152; .NET CLR 3.5.30729)";
                request.CookieContainer = cookieContainer;
                request.ContentLength = postData.Length;

                // 提交请求数据 
                System.IO.Stream outputStream = request.GetRequestStream();
                outputStream.Write(postData, 0, postData.Length);
                outputStream.Close();
                HttpWebResponse response;
                Stream responseStream;
                StreamReader reader;

                response = request.GetResponse() as HttpWebResponse;

				responseStream = response.GetResponseStream();
				reader = new System.IO.StreamReader(responseStream, Encoding.UTF8);
				string srcString = reader.ReadToEnd();
				srcString = srcString.Replace("\"", "");
				var strArray = srcString.Split(',');
				flag = Convert.ToBoolean(strArray[0].Split(':')[1]);
				token = strArray[1].Split(':')[1].TrimEnd('}');
				tuple = new Tuple<bool, string>(flag,token);
				reader.Close();
				
            }
            catch (Exception ex)
            {
				logger.Error(ex);
            }

            return tuple;
        }
	}
}
