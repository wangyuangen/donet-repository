using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Quartz;
using Img.Config;
using Img.DataService.Imp;
using Img.Config.config;
using Img.Config.Model;
using Img.DataService;
using Img.DataService.Infrastructure;
using Img.Model.Search;
using Img.Model.Models;
using System.Threading;

namespace ImgAutoUploadService
{
    public class JobRun : IJob
    {
        private static Img.Nlog.Imp.Logger logger = new Img.Nlog.Imp.Logger();
        private Dictionary<string, IDataService> dicServices;
        private Dictionary<string, ConfigBase> dicConfig;
        private DateTime? StopTime;

        //影像软件
        private static string BrandType = ConfigManager.Instance.Global.AutoUploadBrandType;
        private IDataService dataService;
        private ConfigBase config;

        public void Execute(IJobExecutionContext context)
        {
            if (BrandType == "")
            {
                logger.Info("BrandType is not configured...");
                return;
            }
            logger.Info("Reisgter System Componet...");
            StartUp.Initialization();

            logger.Info("Img Auto Upload Service Start...");
            int FixedPeriod = ConfigManager.Instance.Global.FixedPeriod;

            dicConfig = StartUp.LoadConfig();
            dicServices = StartUp.LoadServices();

            foreach (var item in dicConfig)
            {
                if (item.Key.Contains(BrandType))
                {
                    config = item.Value;
                    break;
                }
            }

            foreach (var item in dicServices)
            {
                if (item.Key.Contains(BrandType))
                {
                    dataService = item.Value;
                    break;
                }
            }

			//首次运行
			if (!StopTime.HasValue)
			{
				StopTime = DateTime.Now.AddHours(5);
				logger.Info(string.Format("服务停止时间：{0}", StopTime));
				StartUp.CreateLogDataBase(config.Master);
			}

            if (FixedPeriod == 1)
            {
                logger.Info("Image Upload By Condition Start...");
                UploadByCondition();
            }
            else
            {
                logger.Info("Image Default Upload Start...");
                DefaultUpload();
            }

            logger.Info("Img Auto Upload Service End");
        }

        /// <summary>
        /// 按约定时间段上传
        /// 默认上传当天的影像
        /// </summary>
        private void DefaultUpload()
        {
            var TodayImageList = dataService.GenerateImageList();
            List<MedicalImage> list = TodayImageList.ToList();
            int count = list.Count;
            int index = 0;

            while (index < count)
            {
                try
                {
                    KillThreadUntilStopTime();
                    List<MedicalImage> templist = new List<MedicalImage>();
                    templist.Add(list[index]);
                    dataService.Upload(templist);
                    index++;
                }
                catch (ThreadAbortException ex)
                {
                    logger.Info(string.Format("{0} DefaultUpload is already stop...", DateTime.Now));
                }
            }
        }

        /// <summary>
        /// 按自定义时间段上传
        /// </summary>
        private void UploadByCondition()
        {
            int CurrentPageIndex = 1;
            var search = new ImageSearch()
            {
                StartTime = ConfigManager.Instance.Global.StartTime,
                EndTime = ConfigManager.Instance.Global.EndTime,
                PageIndex = CurrentPageIndex,
                PageSize = 20,
                BrandType = BrandType
            };

            while (true)
            {
                try
                {
                    KillThreadUntilStopTime();
                    Page<MedicalImage> ImageList = dataService.GenerateImageList(search);
                    int PageCount = ImageList.PageCount;
                    logger.Info(string.Format("类型：自定义时间段上传，当前页码：{0}，总页数：{1}", CurrentPageIndex, PageCount));

                    if (CurrentPageIndex > PageCount)
                    {
                        break;
                    }

                    List<MedicalImage> templist = new List<MedicalImage>();
                    foreach (var item in ImageList.Items)
                    {
                        if (item.Status == Status.已上传.ToString() || string.IsNullOrEmpty(item.PatientId))
                        {
                            continue;
                        }
                        templist.Add(item);
                    }

                    if (!templist.Any())
                    {
                        logger.Info(string.Format("当前页码：{0}，总页数：{1}。未匹配到任何上传失败或未上传影像，跳转至下一页码", CurrentPageIndex, PageCount));
                        continue;
                    }

                    logger.Info(string.Format("当次匹配到的影像记录条数：{0}", templist.Count));
                    dataService.Upload(templist);
                }
                catch (ThreadAbortException ex)
                {
                    logger.Info(string.Format("{0} Upload By Condition is already stop...", DateTime.Now));
                }
                catch(Exception ex)
                {
                    logger.Info(ex.Message);
                }
                finally
                {
                    search.PageIndex = ++CurrentPageIndex;
                }
            }
        }

        private void KillThreadUntilStopTime()
        {
            if (DateTime.Compare(DateTime.Now, (DateTime)StopTime) >= 0)
            {
                Thread.CurrentThread.Abort();
            }
        }
    }
}