using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Img.Config;
using Img.Model.Job;
using System.Data.OleDb;
using ADOX;
using Img.Config.Model;
using Img.Config.config;
using System.IO;
using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Data.Isql;

namespace Img.JobLogData
{
    public class JobLogDataService
    {
        private static Img.Nlog.Imp.Logger log = new Img.Nlog.Imp.Logger();

        #region create joblog database
        /// <summary>
        /// 创建日志数据库
        /// </summary>
		public void CreateDatabase(string connectiongStr)
        {
			if(connectiongStr.Contains("Provider"))
			{
				CreateAccessDataBase(connectiongStr);
				return;
			}
			if(connectiongStr.Equals("FireBird"))
			{
				CreateFireBirdDataBse();
				return;
			}
            using (SqlConnection conn = GetSqlConnection(connectiongStr))
            {
                var str = new List<string>();
                str.Add("use master");
                str.Add(@"if not exists(select name from master.dbo.sysdatabases where name ='JobLogDb') 
						  create database JobLogDb");
                str.Add("use JobLogDb");
                str.Add(@"if object_id(N'JobLog',N'U') is null
					      create table JobLog
					      (
					      	 Id int identity primary key not null,
					      	 ImgId nvarchar(250) not null,
					      	 ImgCreateTime datetime not null,
					      	 UploadTime datetime not null,
					      	 PatientName nvarchar(128) not null,
							 PrivateId nvarchar(20) null,
					      	 UploadStatus int not null,
							 ErrorLog nvarchar(200) null
					      )");
				try
				{
					foreach (var item in str)
					{
						SqlCommand comm = new SqlCommand(item, conn);
						var num = comm.ExecuteNonQuery();
						log.Info(string.Format("受影响的行数 :{0}", num));
					}

					//日志表sql升级-新增影像品牌字段
					string strUpdate = @"use JobLogDb
                                   select COUNT(*) from syscolumns where id=object_id('joblog') and name='brandtype'";

					int count = conn.QuerySingle<int>(strUpdate);
					if (count == 0)
					{
						string sql = "alter table joblog add brandtype nvarchar(50) null";
						conn.Execute(sql);

						string sqlSidexis = "update joblog set brandtype='sidexis'";
						conn.Execute(sqlSidexis);
					}
				}
				catch (Exception ex)
				{
					log.Info("sql执行失败:" + ex);
				}
            }
        }

		/// <summary>
		/// 创建FireBird数据库
		/// </summary>
		private static void CreateFireBirdDataBse()
		{
			ConfigBase config = ConfigManager.Instance.GetConfig("DBSwin");

			var str = @"CREATE TABLE JobLog(
					   	 ImgId VARCHAR(250) not null,
					   	 ImgCreateTime TIMESTAMP not null,
					   	 UploadTime TIMESTAMP not null,
					   	 PatientName VARCHAR(128) not null,
					   	 PrivateId VARCHAR(20),
					   	 UploadStatus INTEGER not null,
						 BrandType VARCHAR(50),
					   	 ErrorLog VARCHAR(200)
					   );";
			
			//创建JoblogDb数据库
			try
			{
				FbConnection.CreateDatabase(config.JobLogDb);
				FbScript script = new FbScript(str);
				script.Parse();

				using (FbConnection conn = new FbConnection(config.JobLogDb))
				{
					FbBatchExecution fbe = new FbBatchExecution(conn);
					foreach (var item in script.Results)
					{
						fbe.Statements.Add(item);
					}
					//fbe.AppendSqlStatements(script);
					fbe.Execute();
				}
			}
			catch (Exception ex)
			{
				log.Info("FireBird数据库创建失败:" + ex);
			}
		}

		/// <summary>
		/// create access database
		/// </summary>
		/// <param name="connectiongStr"></param>
		private static void CreateAccessDataBase(string connectiongStr)
		{
			ConfigBase config = ConfigManager.Instance.GetConfig("PointNix");
			if(File.Exists(Path.Combine(config.RootDirectory,"JobLogDb.mdb")))
			{
				return;
			}
			ADOX.Catalog catelog = new Catalog();
			catelog.Create(connectiongStr);
			ADODB.Connection cn = new ADODB.Connection();
			cn.Open(connectiongStr, null, null);
			catelog.ActiveConnection = cn;
			ADOX.Table table = new ADOX.Table();
			table.Name = "JobLog";
			ADOX.Column column = new Column();
			column.ParentCatalog = catelog;
			column.Name = "Id";
			column.Type = DataTypeEnum.adInteger;
			column.DefinedSize = 9;
			column.Properties["AutoIncrement"].Value = true;
			table.Columns.Append(column, DataTypeEnum.adInteger, 9);
			table.Keys.Append("FirstTablePrimaryKey", KeyTypeEnum.adKeyPrimary, column, null, null);
			table.Columns.Append("ImgId", DataTypeEnum.adVarWChar, 200);
			table.Columns.Append("ImgCreateTime", DataTypeEnum.adDate, 0);
			table.Columns.Append("UploadTime", DataTypeEnum.adDate, 0);
			table.Columns.Append("PatientName", DataTypeEnum.adVarWChar, 50);
			table.Columns.Append("PrivateId", DataTypeEnum.adVarWChar, 50);
			table.Columns.Append("UploadStatus", DataTypeEnum.adInteger, 5);
			table.Columns.Append("ErrorLog", DataTypeEnum.adVarWChar, 200);
			table.Columns.Append("BrandType",DataTypeEnum.adVarWChar,50);
			catelog.Tables.Append(table);
			cn.Close();
		}
        #endregion

        #region 操作日志表
        /// <summary>
        /// 取所有上传失败的记录
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetFailedId(string connectiongStr)
        {
            using (IDbConnection conn = GetSqlConnection(connectiongStr))
            {
                var sqlStr = "select ImgId from JobLog where brandtype=@brandtype and UploadStatus = 1";
                var idList = conn.Query<int>(sqlStr, new { brandtype = "Sidexis"});
                return idList;
            }
        }

		public IEnumerable<int> GetAccessFailedId(string connectionStr)
		{
			using(IDbConnection conn = GetAccessConnection(connectionStr))
			{
				var sql = "select ImgId from JobLog where brandtype=@brandtype and UploadStatus = 1";
				var idList = conn.Query<int>(sql, new { brandtype = "PointNix"});
                return idList;
			}
		}

		public bool GetImgById(string id)
		{
			ConfigBase config = ConfigManager.Instance.GetConfig("EasyDent");
			using(IDbConnection conn = GetSqlConnection(config.JobLogDb))
			{
				var sql = "select * from JobLog where ImgId=@ImgId and UploadStatus = 0 ";
				var list = conn.Query<JobLog>(sql, new { ImgId = id });
				return list.Count() > 0;
			}
		}

        /// <summary>
        /// 得到上传过的最大影像id
        /// </summary>
        /// <returns></returns>
        public int GetMaxId(string connectiongStr)
        {
            using (IDbConnection conn = GetSqlConnection(connectiongStr))
            {
                var sqlStr = "select max(ImgId) from JobLog";
                var maxId = conn.QueryFirst<int>(sqlStr);
                return maxId;
            }
        }

        public IEnumerable<JobLog> GetAllJobLogs(string connectiongStr)
        {
            using (IDbConnection conn = GetSqlConnection(connectiongStr))
            {
                var sqlStr = "select * from JobLog where brandtype=@brandtype";
                var dataLsit = conn.Query<JobLog>(sqlStr, new { brandtype = "sidexis" });
                return dataLsit;
            }
        }

		public IEnumerable<JobLog> GetAcccessAllJobLogs(string connectionStr)
		{
			using(IDbConnection conn = GetAccessConnection(connectionStr))
			{
				var sql = "select * from JobLog where brandtype=@brandtype";
				var dataList = conn.Query<JobLog>(sql, new { brandtype = "PointNix" });
				return dataList;
			}
		}

        /// <summary>
        /// 获取所有上传成功的记录
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetSuccessedId(string connectiongStr)
        {
            using (IDbConnection conn = GetSqlConnection(connectiongStr))
            {
                var sqlStr = string.Format(@"select ImgId from JobLog where brandtype=@brandtype and UploadStatus = 0");
                var idList = conn.Query<int>(sqlStr, new { brandtype ="Sidexis" });
                return idList;
            }
        }

		public IEnumerable<int> GetAccessSuccessedId(string connectionStr)
		{
			using (IDbConnection conn = GetAccessConnection(connectionStr))
			{
				var sqlStr = string.Format(@"select ImgId from JobLog where brandtype=@brandtype and UploadStatus = 0");
				var idList = conn.Query<int>(sqlStr, new { brandtype = "PointNix" });
				return idList;
			}
		}

        /// <summary>
        /// 插数据
        /// </summary>
        /// <param name="joblog"></param>
        /// <returns></returns>
        public bool InsertJobLog(JobLog joblog, string connectiongStr = "Data Source=.;Initial CataLog=JobLogDb;User ID=sa;password=123456;")
        {
            using (IDbConnection conn = GetSqlConnection(connectiongStr))
            {
                var sqlStr = @"insert into JobLog(ImgId,ImgCreateTime,UploadTime,PatientName,PrivateId,UploadStatus,ErrorLog,brandtype) 
							values(@ImgId,@ImgCreateTime,@UploadTime,@PatientName,@PrivateId,@UploadStatus,@ErrorLog,@brandtype)";
                return conn.Execute(sqlStr, joblog) > 0;
            }
        }

		private static FbConnection GetFireBirdConnection(string connectionStr)
		{
			FbConnection conn = new FbConnection();
			conn.ConnectionString = connectionStr;
			try
			{
				conn.Open();
			}
			catch (Exception ex)
			{
				log.Error("FireBird数据库连接失败:" + ex);
			}
			return conn;
		}

		private static OleDbConnection GetAccessConnection(string connectiongStr)
		{
			OleDbConnection conn = new OleDbConnection(connectiongStr);
			if (conn.State == ConnectionState.Closed)
            {
				//OleDbCommand comm = new OleDbCommand("select * from ImageInfo",conn);
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
					log.Info("连接数据库失败：" + ex);
                }
            }
            return conn;
		}

        private static SqlConnection GetSqlConnection(string connectiongStr)
        {
            SqlConnection conn = new SqlConnection(connectiongStr);

            if (conn.State == ConnectionState.Closed)
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
					log.Info("连接数据库失败：" + ex);
                    log.Error(ex);
                }
            }
            return conn;
        }
        #endregion
    }
}
