using Img.Config.config;
using Img.Config.Model;
using Img.DataService.Infrastructure;
using Img.Model.DBSwin;
using Img.Model.Models;
using Img.Model.Search;
using Img.Nlog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Img.DataService.Imp
{
	public class DBSwinDataService : DataServiceBase<DBSwinInfo, MedicalImage>, IDataService
	{
		private DBSwin.DBSwinData dbswinData;
		private ConfigBase dbSwinDataConfig;

		public string Name
		{
			get
			{
				return "Sidexis";
			}
		}
		protected override ILogger JobLogger
		{
			get
			{
				return dbswinData as ILogger;
			}
		}

		public DBSwinDataService() : base() { }

		public DBSwinDataService(IAccountService accountService,
			IAppointmentService appointmentService,
			 IImgUploadService imgUploadService,
			IPatientService patientService,
			ILogger Logger)
			: base(accountService, appointmentService, imgUploadService, patientService, Logger)
		{
			dbswinData = new DBSwin.DBSwinData();
			dbSwinDataConfig = ConfigManager.Instance.GetConfig(Name);
		}

		public Page<MedicalImage> GenerateImageList(ImageSearch search)
		{
			if (string.IsNullOrEmpty(dbSwinDataConfig.DbConnectString))
			{
				return new Page<MedicalImage>();
			}
			var ImgList = dbswinData.GenerateImageList(search);
			var data = ToCollection(ImgList.Items);
			return new Page<MedicalImage>(search.PageIndex, search.PageSize, ImgList.TotalCount, data);
		}

		public IEnumerable<MedicalImage> GenerateImageList()
		{
			if (string.IsNullOrEmpty(dbSwinDataConfig.DbConnectString))
			{
				return new List<MedicalImage>();
			}
			var ImgList = dbswinData.GenerateImageList();
			var data = ToCollection(ImgList);
			return data;
		}
	}
}
