using Img.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Img.SironaData;
using Img.DataService.Infrastructure;
using Img.Nlog;
using Img.Model.Search;
using Img.Model.Sidexis;
using Img.JobLogData;
using Img.Config.config;
using Img.Config.Model;

namespace Img.DataService.Imp
{
    public class SidexisDataService : DataServiceBase<TRawDto, MedicalImage>, IDataService
    {
        private Img.SironaData.SironaImgUpload _SironaImgUpload;
        private ConfigBase SidexisConfig;

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
                return _SironaImgUpload as ILogger;
            }
        }

        public SidexisDataService()
            : base()
        {

        }
        public SidexisDataService(IAccountService accountService,
            IAppointmentService appointmentService,
            IImgUploadService imgUploadService,
            IPatientService patientService,
            ILogger Logger)
            : base(accountService, appointmentService, imgUploadService, patientService, Logger)
        {
            _SironaImgUpload = new SironaImgUpload();
            SidexisConfig = ConfigManager.Instance.GetConfig(Name);
        }

        public Page<MedicalImage> GenerateImageList(ImageSearch search)
        {
            if (string.IsNullOrEmpty(SidexisConfig.DbConnectString))
            {
                return new Page<MedicalImage>();
            }

            var source = _SironaImgUpload.GetTRawDtosByTime_Page(search.PageSize, search.StartTime, search.EndTime, search.PageIndex);
            var DestinationData = ToCollection(source.Item1);
            var Pimglist = new Page<MedicalImage>(search.PageIndex, search.PageSize, source.Item2, DestinationData);
            return Pimglist;
        }

        public IEnumerable<MedicalImage> GenerateImageList()
        {
            if (string.IsNullOrEmpty(SidexisConfig.DbConnectString) || string.IsNullOrEmpty(SidexisConfig.JobLogDb))
            {
                return new List<MedicalImage>();
            }

            var joblogService = new JobLogDataService();
            var uploadSuccessedId = joblogService.GetSuccessedId(SidexisConfig.JobLogDb);
            string startTime = DateTime.Now.Date.AddDays(-1).ToShortDateString();
            var rawDtoList = _SironaImgUpload.GetTRawDtosByTime(uploadSuccessedId, Convert.ToDateTime(startTime));
            var ImageList = ToCollection(rawDtoList);
            return ImageList;
        }
    }
}
