using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Img.Model.Search
{
    public class ImageSearch
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string BrandType { get; set; }
    }
}
