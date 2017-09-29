using FirebirdSql.Data.FirebirdClient;
using Img.Config.config;
using Img.Config.Model;
using Img.Model.DBSwin;
using Img.Model.Models;
using Img.Model.Search;
using Img.Nlog;
using Img.Nlog.Imp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.IO;
using System.Collections;

namespace Img.DBSwin
{
	public class DBSwinData:ILogger
	{
		private ConfigBase config;
		private static Logger Log;

		public DBSwinData()
		{
			Log = new Logger();
			config = ConfigManager.Instance.GetConfig("DBSwin");
		}

		public FbConnection GetConnection(string connection)
		{
			FbConnection conn = new FbConnection();
			conn.ConnectionString = connection;
			try
			{
				conn.Open();
			}
			catch (Exception ex)
			{
				Log.Error("FireBird数据库连接失败:" + ex);
			}
			return conn;
		}

		/// <summary>
		/// 自动上传
		/// </summary>
		/// <returns></returns>
		public IEnumerable<DBSwinInfo> GenerateImageList()
		{
			using(FbConnection conn = GetConnection(config.DbConnectString))
			{
				string sTimeStr = DateTime.Now.AddDays(-1).ToShortDateString();
				DateTime StartTime = Convert.ToDateTime(sTimeStr);
				DateTime EndTime = StartTime.AddHours(23).AddMinutes(59).AddSeconds(59);

				var sql = @"select x.vorgnr,p.pnr,p.knr as PrivateId,p.pvname as PVName,p.pnname as PNName,x.orgfile as ImgFile ,x.AUFNDATUM as CreatedDate,x.aufnzeit as CreatedTime 
						   from PATIENT p inner join xrayvideo x on p.pnr = x.pnr;";
				try
				{
					var data = conn.Query<DBSwinInfo>(sql).ToList();
					var dir = new DirectoryInfo(config.RootDirectory);
					var arrList = new List<string>();
					GetDir(dir, arrList);

					foreach (var item in data)
					{
						item.CheckTime = item.CreatedDate.Add(item.CreatedTime);
						item.Category = (Model.DBSwin.ImageCategory)config.ImageCategory;
						item.BrandType = item.ImgFile.Contains(".VTF") ? "DBSwinVTF" : item.ImgFile.Contains(".XTF") ? "DBSwinXTF" : "DBSwin";
						item.Status = GetStatus(item.Vorgnr, item.BrandType) == "" ? "未上传" : GetStatus(item.Vorgnr, item.BrandType);
						var fullname = arrList.FirstOrDefault(x=>x.Contains(item.ImgFile));
						item.ImgFile = fullname == null ? "" : fullname;
						item.PatientName = item.PNName;
						if(!item.PVName.Equals("No ID Number"))
						{
							item.PatientName = string.Concat(item.PNName,item.PVName);
						}
					}

					data = data.Where(x=>x.CheckTime>=StartTime && x.CheckTime <= EndTime).ToList();

					return data;
				}
				catch (Exception ex)
				{
					Log.Error(ex);
				}
			}
			return null;
		}

		/// <summary>
		/// 手动上传
		/// </summary>
		/// <param name="search"></param>
		/// <returns></returns>
		public Page<DBSwinInfo> GenerateImageList(ImageSearch search)
		{
			using(FbConnection conn = GetConnection(config.DbConnectString))
			{
				var sql = @"select x.vorgnr,p.pnr,p.knr as PrivateId,p.pvname as PVName,p.pnname as PNName,x.orgfile as ImgFile ,x.AUFNDATUM as CreatedDate,x.aufnzeit as CreatedTime 
						   from PATIENT p inner join xrayvideo x on p.pnr = x.pnr;";
				
				try
				{
					var data = conn.Query<DBSwinInfo>(sql).ToList();
					var dir = new DirectoryInfo(config.RootDirectory);
					var arrList = new List<string>();
					GetDir(dir, arrList);

					foreach (var item in data)
					{
						item.CheckTime = item.CreatedDate.Add(item.CreatedTime);
						item.Category = (Model.DBSwin.ImageCategory)config.ImageCategory;
						item.BrandType = item.ImgFile.Contains(".VTF") ? "DBSwinVTF" : item.ImgFile.Contains(".XTF") ? "DBSwinXTF" : "DBSwin";
						item.Status = GetStatus(item.Vorgnr, item.BrandType) == "" ? "未上传" : GetStatus(item.Vorgnr, item.BrandType);
						var fullName = arrList.FirstOrDefault(x=>x.Contains(item.ImgFile));
						item.ImgFile = fullName == null ? "" : fullName;
						item.PatientName = item.PNName;
						if(!item.PVName.Equals("No ID Number"))
						{
							item.PatientName = string.Concat(item.PNName,item.PVName);
						}
					}

					var TotalCount = data.Count(x => x.CheckTime <= search.EndTime && x.CheckTime >= search.StartTime);

					data = data.Where(x => x.CheckTime <= search.EndTime && x.CheckTime >= search.StartTime)
						.OrderByDescending(x => x.CheckTime)
						.Skip((search.PageIndex - 1) * search.PageSize).Take(search.PageSize).ToList();

					return new Page<DBSwinInfo>(search.PageIndex, search.PageSize, TotalCount, data);
				}
				catch (Exception ex)
				{
					Log.Error(ex);
				}
				return null;
			}
		}

		/// <summary>
		/// 获取目录下所有文件
		/// </summary>
		/// <param name="directory"></param>
		/// <param name="arrlist"></param>
		private void GetDir(DirectoryInfo directory, List<string> arrlist)
		{
			FileInfo[] files = directory.GetFiles();

			foreach (FileInfo file in files)
			{
				arrlist.Add(file.FullName);
			}

			DirectoryInfo[] directs = directory.GetDirectories();

			foreach (DirectoryInfo direct in directs)
			{
				GetDir(direct, arrlist);
			}
		}


		public string GetStatus(string Vorgnr,string brandType)
		{
			string status = "";
			string strsql = @"select uploadstatus from JobLog
                                where ImgId=@ImgId and BrandType=@BrandType
                                order by UploadTime desc";

			using (FbConnection conn = GetConnection(config.JobLogDb))
			{
				var data = conn.Query<string>(strsql, new { ImgId = Vorgnr, BrandType = brandType }).FirstOrDefault();
				if (data != null)
				{
					status = data.ToString() == "0" ? "已上传" : "上传失败";
				}
			}

			return status;
		}

		public void Error(object msg, Exception exp = null)
		{
			throw new NotImplementedException();
		}

		public void Debug(object msg, Exception exp = null)
		{
			throw new NotImplementedException();
		}

		public void Info(object msg, Exception exp = null)
		{
			using(FbConnection conn = GetConnection(config.JobLogDb))
			{
				var sqlStr = @"insert into JobLog(ImgId,ImgCreateTime,UploadTime,PatientName,PrivateId,UploadStatus,ErrorLog,brandtype) 
							values(@ImgId,@ImgCreateTime,@UploadTime,@PatientName,@PrivateId,@UploadStatus,@ErrorLog,@brandtype)";
                conn.Execute(sqlStr, msg);
			}
		}

		public void Warn(object msg, Exception exp = null)
		{
			throw new NotImplementedException();
		}
	}
}
