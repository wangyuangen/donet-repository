using Img.Config.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Img.Config.config
{
    public class Common
    {
        /// <summary>
        /// 转换数据库连接字符串
        /// </summary>
        /// <param name="ConnectStr"></param>
        /// <returns></returns>
        public static Tuple<string, string, string, string> ParseDbConnectString(string ConnectStr)
        {
            if(string.IsNullOrEmpty(ConnectStr))
            {
                return new Tuple<string, string, string, string>("", "", "","");
            }
			//if (!ConnectStr.Contains("Data Source") || !ConnectStr.Contains("Initial Catalog")
			//	|| !ConnectStr.Contains("User ID") || !ConnectStr.Contains("password"))
			//{
			//	throw new Exception("数据库连接字符串不符合规则");
			//}

            string[] DbStrArray = ConnectStr.Split(';');
            string DataSource = DbStrArray[0].Replace("Data Source=", "");
			string DBName = DbStrArray[1].Contains("Initial Catalog=") ? DbStrArray[1].Replace("Initial Catalog=", "") : "";
			var UserName = "";
			var UserPwd = "";
			if(DbStrArray.Length>2)
				UserName = DbStrArray[2].Contains("User ID=") ? DbStrArray[2].Replace("User ID=", "") : "";
			if(DbStrArray.Length>3)
				UserPwd = DbStrArray[3].Contains("password=") ? DbStrArray[3].Replace("password=", "") : "";
            return new Tuple<string, string, string, string>(DataSource, DBName, UserName, UserPwd);
        }

		public static string ConvertDbConnectString(string DataSource, string DbName, string UserName,
			string UserPwd,ConfigBase config, string ImgType = "")
		{
			string ConnectStr = "";
			if (ImgType == "Sidexis")
			{
				DbName = "PDATA_SQLEXPRESS";
				ConnectStr = string.Format("Data Source={0};Initial Catalog={1};User ID={2};password={3};",
				DataSource, DbName, UserName, UserPwd);
			}
			else if (ImgType == "EasyDent")
			{
				DbName = "TSFD";
				ConnectStr = string.Format("Data Source={0};Initial Catalog={1};User ID={2};password={3};",
				DataSource, DbName, UserName, UserPwd);
			}
			else if (ImgType == "PointNix")
			{
				DbName = "ImageInfo.mdb";
				ConnectStr = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};",
				config.RootDirectory + "\\" + DbName);
			}
			else if (ImgType.Equals("DBSwin"))
			{
				return config.DbConnectString;
			}
			if (string.IsNullOrWhiteSpace(ImgType))
			{
				if(config.Name.Equals("DBSwin") && DbName.Equals("master"))
				{
					return config.Master;
				}
				if(config.Name.Equals("DBSwin") && DbName.Equals("JobLogDb"))
				{
					return config.JobLogDb;
				}
				if (config.Master.Contains("Initial Catalog"))
				{
					ConnectStr = string.Format("Data Source={0};Initial Catalog={1};User ID={2};password={3};",
					DataSource, DbName, UserName, UserPwd);
				}
				else if (config.Master.Contains("Provider"))
				{
					DbName = "JobLogDb.mdb";
					ConnectStr = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};",
					config.RootDirectory + "\\" + DbName);
				}
			}
			return ConnectStr;
		}
    }
}
