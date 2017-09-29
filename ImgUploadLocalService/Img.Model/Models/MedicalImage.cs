using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Img.Model.Models
{
    public class MedicalImage
    {
        public DateTime CheckTime { get; set; }
        public string PatientId { get; set; }
        public string VideoNum { get; set; }
        public string FilePath { get; set; }
        public string PatientName { get; set; }
        public string Status { get; set; }
        public bool IsUpload { get; set; }
        public ImageCategory Category { get; set; }
        public string BrandType { get; set; }
    }

    public enum ImageCategory
    {
        PANORAMA = 1,
        SENSOR = 2,
        CBCT = 3,
        CAMERA = 4,
        OTHER = 5
    }

    public enum Status
    {
        已上传,
        未上传,
        上传失败
    }
}
