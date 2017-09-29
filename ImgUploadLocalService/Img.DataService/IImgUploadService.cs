using Img.Model.Job;
using Img.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Img.DataService
{
    public interface IImgUploadService
    {
        bool UploadImage(Appointment appt, string filePath, string note, string _apiToken, int DefaultCategory = 4);
    }
}
