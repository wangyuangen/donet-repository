using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Img.Model.PointNix;
using System.Data.OleDb;
using Img.Config.Model;
using Img.Config.config;
using System.Data;
using Dapper;
using Img.JobLogData;
using Img.Model.Job;
using System.Globalization;
using Img.Nlog.Imp;
using Img.Nlog;

namespace Img.PointNix
{
	public class PointNixData : ILogger
	{
		private ConfigBase config;
		private static Logger Log;

		public PointNixData()
		{
			Log = new Logger();
			config = ConfigManager.Instance.GetConfig("PointNix");
		}
		private OleDbConnection GetConnection(string connstr)
		{
			OleDbConnection conn = new OleDbConnection(connstr);
			if (conn.State == ConnectionState.Closed)
			{
				//OleDbCommand comm = new OleDbCommand("select * from ImageInfo",conn);
				try
				{
					conn.Open();
				}
				catch (Exception ex)
				{
					Log.Error(ex);
				}
			}
			return conn;
		}

		public Tuple<IEnumerable<ImageInfo>, int> GetImageInfoByTime_Page(int PageSize,
			DateTime startTime, DateTime endTime, int PageIndex = 0)
		{
			using (IDbConnection conn = GetConnection(config.DbConnectString))
			{
				JobLogDataService joblog = new JobLogDataService();
				endTime = endTime.AddHours(23).AddMinutes(59).AddSeconds(59);
				var sql = @"select ID as VideoNum,ChartNo_T as PatientId,PatientNm_T as PatientName,PathFix_T as PathFix,PathVar_T as PathVar,
						  FileNm_T as FileNm,ImageID_T as ImageGuid,StudyDate_T as CreatedDate,StudyTime_T as CreatedTime from ImageInfo";
				var dataList = conn.Query<ImageInfo>(sql).ToList();
				var joblogs = joblog.GetAcccessAllJobLogs(config.JobLogDb);
				string rootDir = config.RootDirectory.EndsWith("\\") == true ? config.RootDirectory : config.RootDirectory + "\\";
				foreach (var item in dataList)
				{
					item.Status = "未上传";
					item.BrandType = "PointNix";
					var createdTime = (item.CreatedDate + item.CreatedTime);
					item.CheckTime = DateTime.ParseExact(createdTime.Substring(0, createdTime.Length - 3), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
					item.FilePath = item.PathFix + item.PathVar + item.FileNm;
					//item.FilePath = rootDir + item.FilePath;
					item.Category = (ImageCategory)config.ImageCategory;
					var joblogone = joblogs.OrderByDescending(x => x.Id).FirstOrDefault(x => x.ImgId == item.VideoNum.ToString());
					if (joblogone != null)
					{
						item.Status = joblogone.UploadStatus == (int)UploadStatus.Successed ? "已上传" : "上传失败";
					}
				}
				var count = dataList.Count(x => x.CheckTime<= endTime && x.CheckTime >= startTime);
				dataList = dataList.Where(x => x.CheckTime <= endTime && x.CheckTime >= startTime)
					.OrderByDescending(x=>x.CheckTime)
					.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
				return new Tuple<IEnumerable<ImageInfo>, int>(dataList, count);
			}
		}

		public IEnumerable<ImageInfo> GetImageInfoByTime(IEnumerable<int> idlist, DateTime startTime, DateTime? endTime = null)
		{
			using (IDbConnection conn = GetConnection(config.DbConnectString))
			{
				JobLogDataService joblog = new JobLogDataService();
				if (endTime == null)
				{
					endTime = startTime.AddHours(23).AddMinutes(59).AddSeconds(59);
				}
				string idArray = string.Join(",", idlist);
				var sql = @"select ID as VideoNum,ChartNo_T as PatientId,PatientNm_T as PatientName,PathFix_T as PathFix,PathVar_T as PathVar,
						  FileNm_T as FileNm,ImageID_T as ImageGuid,StudyDate_T as CreatedDate,StudyTime_T as CreatedTime from ImageInfo";

				var failedList = joblog.GetAccessFailedId(config.JobLogDb);
				string rootDir = config.RootDirectory.EndsWith("\\") == true ? config.RootDirectory : config.RootDirectory + "\\";
				var joblogs = joblog.GetAcccessAllJobLogs(config.JobLogDb);

				IEnumerable<ImageInfo> result = null;
				if (!string.IsNullOrWhiteSpace(idArray))
				{
					result = conn.Query<ImageInfo>(string.Concat(sql, " where ID not in (@ID)"), new { ID = idArray }).ToList();
				}
				else
				{
					result = conn.Query<ImageInfo>(sql).ToList();
				}

				foreach (var item in result)
				{
					item.Status = "未上传";
					if (failedList.Count(x => x == item.VideoNum) > 0)
					{
						item.Status = "上传失败";
					}
					item.BrandType = "PointNix";
					var createdTime = (item.CreatedDate + item.CreatedTime);
					item.CheckTime = DateTime.ParseExact(createdTime.Substring(0, createdTime.Length - 3), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
					item.FilePath = item.PathFix + item.PathVar + item.FileNm;
					//item.FilePath = rootDir + item.FilePath;
					item.Category = (ImageCategory)config.ImageCategory;
					var joblogone = joblogs.OrderByDescending(x => x.Id).FirstOrDefault(x => x.ImgId == item.VideoNum.ToString());
					if (joblogone != null)
					{
						item.Status = joblogone.UploadStatus == (int)UploadStatus.Successed ? "已上传" : "上传失败";
					}
				}
				result = result.Where(x => x.CheckTime >= startTime && x.CheckTime <= endTime);
				return result;
			}
		}

		public void Error(object msg, Exception exp = null)
		{

		}

		public void Debug(object msg, Exception exp = null)
		{

		}

		public void Info(object msg, Exception exp = null)
		{
			JobLog joblog = msg as JobLog;
			using (OleDbConnection conn = GetConnection(config.JobLogDb))
			{
				string sql = @"insert into JobLog(ImgId,ImgCreateTime,UploadTime,PatientName,PrivateId,UploadStatus,ErrorLog,BrandType) 
				values(@ImgId,@ImgCreateTime,@UploadTime,@PatientName,@PrivateId,@UploadStatus,@ErrorLog,@BrandType)";
				var cmd = new OleDbCommand(sql,conn);
				cmd.Parameters.Add("@ImgId",OleDbType.Integer).Value = joblog.ImgId;
				cmd.Parameters.Add("@ImgCreateTime",OleDbType.Date).Value = joblog.ImgCreateTime;
				cmd.Parameters.Add("@UploadTime",OleDbType.Date).Value = joblog.UploadTime;
				cmd.Parameters.Add("@PatientName",OleDbType.VarWChar).Value = joblog.PatientName;
				cmd.Parameters.Add("@PrivateId",OleDbType.VarWChar).Value = joblog.PrivateId;
				cmd.Parameters.Add("@UploadStatus",OleDbType.Integer).Value = joblog.UploadStatus;
				cmd.Parameters.Add("@ErrorLog", OleDbType.VarWChar).Value = joblog.ErrorLog == null ? "" : joblog.ErrorLog;
				cmd.Parameters.Add("@BrandType",OleDbType.VarWChar).Value = joblog.BrandType;
				cmd.ExecuteNonQuery();
			}
		}

		public void Warn(object msg, Exception exp = null)
		{

		}
	}
}
