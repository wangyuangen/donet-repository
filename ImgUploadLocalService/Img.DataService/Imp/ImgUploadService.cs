using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Img.Model.Models;
using Img.SaasAPIRequest;

namespace Img.DataService.Imp
{
    public class ImgUploadService : IImgUploadService
    {
        private ImgUploadRequest ImgUploadRequest;
        public ImgUploadService()
        {
            ImgUploadRequest = new ImgUploadRequest();
        }

        public bool UploadImage(Appointment appt, string filePath, string note, string _apiToken, int DefaultCategory = 4)
        {
            return ImgUploadRequest.UploadImage(appt, filePath, note, _apiToken, DefaultCategory);
        }
    }
}
