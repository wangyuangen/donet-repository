using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Img.DataService.Infrastructure;
using Img.Model.Models;
using Img.EasyDent;
using Img.Model.EasyDent;
using System.IO;
using Img.Config.config;
using Img.Model.Search;
using Img.Nlog;
using Img.Config.Model;

namespace Img.DataService.Imp
{
    public class EasyDentDataService : DataServiceBase<EDImage, MedicalImage>, IDataService
    {
        private EasyDentData easyDentData;
        private ConfigBase EasyDentConfig;

        public string Name
        {
            get
            {
                return "EasyDent";
            }
        }

        protected override ILogger JobLogger
        {
            get
            {
                return easyDentData as ILogger;
            }
        }

        public EasyDentDataService()
            : base()
        {
        }
        public EasyDentDataService(IAccountService accountService,
            IAppointmentService appointmentService,
            IImgUploadService imgUploadService,
            IPatientService patientService,
            ILogger Logger)
            : base(accountService, appointmentService, imgUploadService, patientService,Logger)
        {
            easyDentData = new EasyDentData();
            EasyDentConfig = ConfigManager.Instance.GetConfig(Name);
        }

        public Page<MedicalImage> GenerateImageList(ImageSearch search)
        {
            if(string.IsNullOrEmpty(EasyDentConfig.DbConnectString))
            {
                return new Page<MedicalImage>();
            }

            var EDImage = easyDentData.GenerateImageList(search);
            var data = ToCollection(EDImage.Items);
            return new Page<MedicalImage>(search.PageIndex, search.PageSize, EDImage.TotalCount, data);
        }


        public IEnumerable<MedicalImage> GenerateImageList()
        {
            if(string.IsNullOrEmpty(EasyDentConfig.DbConnectString))
            {
                return new List<MedicalImage>();
            }
            var data = easyDentData.GenerateImageList();
            var imagelist = ToCollection(data);
            return imagelist;
        }
    }
}