using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Configuration;

namespace Data.Storage
{
	public class OnlineBackUp
	{
		private static Nlog.Logger logger = new Nlog.Logger();

		public static void Main(string[] args)
		{
			try
			{
				string brokerList = ConfigurationManager.AppSettings["brokerList"];
				var topics = ConfigurationManager.AppSettings["topics"].Split(',').ToList();
				Run_Poll(brokerList, topics);
			}
			catch (Exception ex)
			{
				logger.Error(ex);
			}
		}

		private static Dictionary<string, object> ConstructConfig(string brokerList, bool enableAutoCommit)
		{
			return new Dictionary<string, object>
            {
                {"group.id", "binlog-consumer"},
                {"enable.auto.commit", enableAutoCommit},
                {"auto.commit.interval.ms", 5000},
                {"statistics.interval.ms", 60000},
                {"bootstrap.servers", brokerList},
                {
                    "default.topic.config", new Dictionary<string, object>()
                    {
                        {"auto.offset.reset", "smallest"}
                    }
                }
            };
		}

		public static void Run_Poll(string brokerList, List<string> topics)
		{
			using (var consumer = new Consumer(ConstructConfig(brokerList, true)))
			{
				var strList = new List<dynamic>();
				consumer.OnMessage += (_, msg) =>
				{
					var value = Encoding.UTF8.GetString(msg.Value);
					if (!string.IsNullOrEmpty(value))
					{
						var binlogMODEL = JsonConvert.DeserializeObject<dynamic>(value);
						strList.Add(binlogMODEL);
						int count = strList.Count;
						if (count.Equals(500))
						{
							EsProvider.BulkPopulateIndexJson(strList);
							strList.Clear();
						}
					}
				};
				consumer.Subscribe(topics);
				var cancelled = false;
				Console.CancelKeyPress += (_, e) =>
				{
					e.Cancel = true;
					cancelled = true;
				};
				logger.Info("Ctrl-C to exit.");
				while (!cancelled)
				{
					consumer.Poll(TimeSpan.FromMilliseconds(100));
				}
			}
		}
	}
}
