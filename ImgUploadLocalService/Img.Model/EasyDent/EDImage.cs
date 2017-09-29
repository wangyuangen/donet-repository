using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Img.Model.EasyDent
{
    public class EDImage
    {
        public int Seq { get; set; }
        public Injek Injek { get; set; }
        public DateTime C_Date { get; set; }
        public int C_Type { get; set; }
        public string Status { get; set; }
        public string BrandType { get; set; }
        public string FilePath { get; set; }
    }

    public enum EasyDentImageCategory
    {
        PANORAMA = 1,
        SENSOR = 2,
        CAMERA = 3,
        CEPHALO = 4
    }
}
