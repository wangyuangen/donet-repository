using Data.Storage.Model;
using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Storage
{
	public class EsProvider
	{
		private static Nlog.Logger logger = new Nlog.Logger();
		public static ElasticClient Client = new ElasticClient(Setting.ConnectionSettings);
		public static string StrIndexName = @"data_storage";
		public static string StrDocType = "binlogType";
		public static bool Index(PersonDetail person)
		{
			var client = new ElasticClient(Setting.ConnectionSettings);
			try
			{
				//添加数据 

				//在调用下面的index方法的时候，如果没有指定使用哪个index，ElasticSearch会直接使用我们在setting中的defaultIndex，如果没有，则会自动创建  
				var index = client.Index(person);
				return index.Created;
			}
			catch (Exception ex)
			{
				Console.WriteLine(" Excepton Message : " + ex.Message);
			}
			return false;
		}

		/// <summary>
		/// 批量写入
		/// </summary>
		/// <param name="posts"></param>
		/// <returns></returns>
		public static bool BulkPopulateIndex(List<LogModel> posts)
		{
			try
			{
				if (posts.Count <= 0) return false;
				var bulkRequest = new BulkRequest { Operations = new List<IBulkOperation>() };
				var idxops = posts.Select(o => new BulkIndexOperation<LogModel>(o) { Id = o.Id }).Cast<IBulkOperation>().ToList();
				bulkRequest.Operations = idxops;
				var response = Client.Bulk(bulkRequest);
				return response.IsValid;
			}
			catch (Exception ex)
			{
				logger.Error(ex);
				return false;
			}
		}

		public static bool BulkPopulateIndexJson(List<dynamic> posts)
		{
			try
			{
				if (posts.Count <= 0) return false;
				var index = "binlog-" + DateTime.Now.ToString("yyyy.MM.dd",DateTimeFormatInfo.InvariantInfo);
				if (!index.Equals(Client.ConnectionSettings.DefaultIndex))
				{
					Client = new ElasticClient(Setting.ConnectionSettings.DefaultIndex(index));
					logger.Info("index:" + index);
				}
				var bulkRequest = new BulkRequest { Operations = new List<IBulkOperation>() };
				var idxops = posts.Select(o => new BulkIndexOperation<dynamic>(o)).Cast<IBulkOperation>().ToList();
				bulkRequest.Operations = idxops;
				var response = Client.Bulk(bulkRequest);
				return response.IsValid;
			}
			catch (Exception ex)
			{
				logger.Error(ex);
				return false;
			}
		}
		public static long GetCount()
		{
			var response = Client.Count<LogModel>();
			return response.Count;
		}
	}

	public static class Setting
	{
		public static string StrConnectionString = ConfigurationManager.AppSettings["connection"];
		public static Uri Node
		{
			get
			{
				return new Uri(StrConnectionString);
			}
		}
		public static ConnectionSettings ConnectionSettings
		{
			get
			{
				return new ConnectionSettings(Node).DefaultIndex("binlog");
			}
		}
	}
}
