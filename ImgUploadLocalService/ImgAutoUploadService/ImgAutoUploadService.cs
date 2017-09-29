using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Quartz;
using Quartz.Impl;
using System.Configuration;
using Img.Config;
using Img.Config.config;
using Img.Config.Model;

namespace ImgAutoUploadService
{
	public partial class ImgAutoUploadService : ServiceBase
	{
		public ImgAutoUploadService()
		{
			InitializeComponent();
		}

		private static IScheduler scheduler;
		//上传时间	
		//private static string runTime = ConfigManager.Instance.Global.RunTime;
		private Global GlobalConfig = ConfigManager.Instance.Global;

		private static Img.Nlog.Imp.Logger logger = new Img.Nlog.Imp.Logger();

		protected override void OnStart(string[] args)
		{
			//logger.Info(string.Format("影像上传将于凌晨1点-6点开始执行..."));

			//从工厂获取调度器实例
			ISchedulerFactory factory = new StdSchedulerFactory();	
			scheduler = factory.GetScheduler();
			scheduler.Start();
			var runtime = GlobalConfig.RunTime;
			var strArray = runtime.Split(':');
			//创建任务imgautoupload.job
			IJobDetail job = JobBuilder.Create<JobRun>().WithIdentity("job","imgautoupload").Build();

			//创建每天指定时间执行的触发器imgautoupload.trigger
			ITrigger trigger = TriggerBuilder.Create()
				.WithIdentity("trigger","imgautoupload")
				.WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(int.Parse(strArray[0]), int.Parse(strArray[1])))
				.Build();

			//将任务与触发器绑定到调度器
			scheduler.ScheduleJob(job, trigger);
		}

		protected override void OnStop()
		{
			//停止服务时关闭定时任务
			scheduler.Shutdown();
		}
	}
}
