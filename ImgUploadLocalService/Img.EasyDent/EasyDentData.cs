using Img.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Img.Config.config;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Img.Nlog;
using Img.Nlog.Imp;
using Img.Config;
using Img.Model.EasyDent;
using Img.Model.Search;
using Img.Config.Model;
using Img.Model.Job;

namespace Img.EasyDent
{
    public class EasyDentData:ILogger
    {
        private ConfigBase config;
        private static Img.Nlog.Imp.Logger Log = new Img.Nlog.Imp.Logger();

        public EasyDentData()
        {
            config = ConfigManager.Instance.GetConfig("EasyDent");
        }

        public Page<EDImage> GenerateImageList(ImageSearch search)
        {
            using (IDbConnection connection = GetSqlConnection(config.DbConnectString))
            {
                string TotalSql = @"select count(0) 
                                    from ImgInfo img
                                    left join 
                                    (select chart,name_f,name from injek
                                    group by chart,name_f,name) i on i.chart=img.chart where 1=1 ";

                string sql = @"select * 
                               from (
                               select img.seq,img.C_Date,img.C_Type,
                               '未上传' status,i.chart,i.name_f+i.name PName,rowindex =(select count(1) from ImgInfo where seq>=img.seq)
                               from ImgInfo img
                               left join 
                               (select chart,name_f,name from injek
                               group by chart,name_f,name) i on i.chart=img.chart where 1=1 ";

				try
				{
					if (search.StartTime.ToString() != "")
					{
						TotalSql += string.Format(" and img.C_Date>=@StartTime");
						sql += string.Format(" and img.C_Date>=@StartTime");
					}

					if (search.EndTime.ToString() != "")
					{
						TotalSql += string.Format(" and img.C_Date<=@EndTime");
						sql += string.Format(" and img.C_Date<=@EndTime");
					}

					sql += string.Format(" )temp where rowindex between {0} and {1}", (search.PageIndex - 1) * search.PageSize + 1, search.PageIndex * search.PageSize);

					var TotalCount = connection.QuerySingle<int>(TotalSql, new { StartTime = search.StartTime, EndTime = search.EndTime });
					var data = connection.Query<EDImage, Injek, EDImage>(sql, (e, i) => { e.Injek = i; return e; },
						new { StartTime = search.StartTime, EndTime = search.EndTime }, splitOn: "chart");

					data.ToList().ForEach(d =>
					{
						d.BrandType = search.BrandType;
						d.FilePath = GetFilePath(d);
						d.Status = GetImageStatus(d.Seq, search) == "" ? d.Status : GetImageStatus(d.Seq, search);
					});
					return new Page<EDImage>(search.PageIndex, search.PageSize, TotalCount, data);
				}
				catch (Exception ex)
				{
					Log.Info("手动上传sql执行失败：" + ex);
				}
				return null;
            }
        }

        /// <summary>
        /// 查询影像最新的上传状态
        /// </summary>
        /// <param name="ImgId"></param>
        /// <returns></returns>
        private string GetImageStatus(int ImgId, ImageSearch search)
        {
            string status = "";
            string strsql = @"select uploadstatus from JobLog
                                where ImgId=@ImgId and BrandType=@BrandType
                                order by UploadTime desc";

            using (IDbConnection conn = GetSqlConnection(config.JobLogDb))
            {
                var data = conn.Query<string>(strsql, new { ImgId = ImgId, BrandType = search.BrandType }).FirstOrDefault();
                if (data != null)
                {
                    status = data.ToString() == "0" ? "已上传" : "上传失败";
                }
            }

            return status;
        }

        /// <summary>
        /// 查询当天影像记录
        /// </summary>
        /// <returns></returns>
		public IEnumerable<EDImage> GenerateImageList()
		{
			using (IDbConnection conn = GetSqlConnection(config.DbConnectString))
			{
				string sTimeStr = DateTime.Now.AddDays(-1).ToShortDateString();
				DateTime StartTime = Convert.ToDateTime(sTimeStr);
				DateTime EndTime = StartTime.AddHours(23).AddMinutes(59).AddSeconds(59);
				
				string sql = @"select img.seq,img.C_Date,img.C_Type,
                               case jlog.UploadStatus when 0 then '未上传'
							   when 1 then '上传失败' end as status
							   ,i.chart,i.name_f+i.name PName
                               from ImgInfo img
                               left join 
							   (select chart,name_f,name from injek
                                group by chart,name_f,name) i on i.chart=img.chart
								inner join (
							        select ImgId,UploadStatus from JobLogDb.dbo.JobLog
									where BrandType='easydent' and UploadStatus=0
									group by ImgId,UploadStatus
							   ) jlog on jlog.ImgId<>img.seq
                               where img.C_Date>=@StartTime and img.C_Date<=@EndTime";

				try
				{
					var data = conn.Query<EDImage, Injek, EDImage>(sql, (e, i) =>
						  {
							  e.Injek = i;
							  return e;
						  }, new { StartTime = StartTime, EndTime = EndTime },
						  splitOn: "chart");
					var search = new ImageSearch()
					{
						BrandType = "EasyDent"
					};
					Log.Info(string.Format("执行sql语句成功:{0}",data.Count()));
					data.ToList().ForEach(d =>
					{
						d.BrandType = search.BrandType;
						d.FilePath = GetFilePath(d);
						d.Status = GetImageStatus(d.Seq, search) == "" ? d.Status : GetImageStatus(d.Seq, search);
					});
					return data;
				}
				catch (Exception ex)
				{
					Log.Info("执行失败:" + ex);
				}
				return null;
			}
		}
        /// <summary>
        /// 数据库连接
        /// </summary>
        /// <returns></returns>
        public SqlConnection GetSqlConnection(string connectiongString)
        {
            SqlConnection conn = new SqlConnection(connectiongString);

            if (conn.State == ConnectionState.Closed)
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
					Log.Info("连接数据库失败：" + ex);
                    Log.Error(ex);
                }
            }
            return conn;
        }

        private string GetFilePath(EDImage image)
        {
            string subFolder = string.Format("sub{0}{1}{2}", image.C_Date.ToString("yyyy").Substring(1, 3),
                image.C_Date.ToString("MM"), CalculateDay(image.C_Date));

            string filepath = string.Format("{0}{1}{2}_{3}{4}{5}_0000.bmp", image.C_Date.ToString("yyyy"), image.C_Date.ToString("MM"), image.C_Date.ToString("dd"),
                image.C_Date.ToString("HH"), image.C_Date.ToString("mm"), image.C_Date.ToString("ss"));
            switch (image.C_Type)
            {
                case 1:
                    filepath = "p" + filepath;
                    break;
                case 2:
                    filepath = "s" + filepath;
                    break;
                default:
                    filepath = "c" + filepath;
                    break;
            }

            string rootDir = config.RootDirectory.EndsWith("\\") == true ? config.RootDirectory : config.RootDirectory + "\\";
            string fullPath = rootDir + subFolder + "\\" + filepath;
            return fullPath;
        }

        private int CalculateDay(DateTime dt)
        {
            int Day = Convert.ToInt32(dt.ToString("dd"));
            int result = Day / 10;
            return result;
        }

        public void Error(object msg, Exception exp = null)
        {
            
        }

        public void Debug(object msg, Exception exp = null)
        {
            
        }

        public void Info(object msg, Exception exp = null)
        {
            using (IDbConnection conn = GetSqlConnection(config.JobLogDb))
            {
                var sqlStr = @"insert into JobLog(ImgId,ImgCreateTime,UploadTime,PatientName,PrivateId,UploadStatus,ErrorLog,brandtype) 
							values(@ImgId,@ImgCreateTime,@UploadTime,@PatientName,@PrivateId,@UploadStatus,@ErrorLog,@brandtype)";
                conn.Execute(sqlStr, msg);
            }
        }

        public void Warn(object msg, Exception exp = null)
        {
            
        }
    }
}
