using Img.DataService.Infrastructure;
using Img.Model.Models;
using Img.Model.PointNix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Img.PointNix;
using Img.Config.Model;
using Img.Nlog;
using Img.Config.config;
using Img.JobLogData;

namespace Img.DataService.Imp
{
	public class PointNixDataService:DataServiceBase<ImageInfo,MedicalImage>,IDataService
	{
		private PointNixData pointNixData;
		private ConfigBase PointNixConfig;

		public string Name
		{
			get
			{
				return "PointNix";
			}
		}

		protected override ILogger JobLogger
        {
            get
            {
                return pointNixData as ILogger;
            }
        }

		public PointNixDataService() : base() { }

		public PointNixDataService(IAccountService accountService,
			IAppointmentService appointmentService,
			 IImgUploadService imgUploadService,
			IPatientService patientService,
			ILogger Logger)
			: base(accountService, appointmentService, imgUploadService, patientService, Logger)
		{
			pointNixData = new PointNixData();
			PointNixConfig = ConfigManager.Instance.GetConfig(Name);
		}



		public Page<MedicalImage> GenerateImageList(Model.Search.ImageSearch search)
		{
			if (string.IsNullOrEmpty(PointNixConfig.DbConnectString))
			{
				return new Page<MedicalImage>();
			}

			var source = pointNixData.GetImageInfoByTime_Page(search.PageSize, search.StartTime, search.EndTime, search.PageIndex);
			var DestinationData = ToCollection(source.Item1);
			var Pimglist = new Page<MedicalImage>(search.PageIndex, search.PageSize, source.Item2, DestinationData);
			return Pimglist;
		}

		public IEnumerable<MedicalImage> GenerateImageList()
		{
			if (string.IsNullOrEmpty(PointNixConfig.DbConnectString) || string.IsNullOrEmpty(PointNixConfig.JobLogDb))
			{
				return new List<MedicalImage>();
			}
			var joblogService = new JobLogDataService();
            var uploadSuccessedId = joblogService.GetAccessSuccessedId(PointNixConfig.JobLogDb);
            string startTime = DateTime.Now.Date.AddDays(-1).ToShortDateString();
            var imageInfoList = pointNixData.GetImageInfoByTime(uploadSuccessedId, Convert.ToDateTime(startTime));
            var ImageList = ToCollection(imageInfoList);
            return ImageList;
		}
	}
}
