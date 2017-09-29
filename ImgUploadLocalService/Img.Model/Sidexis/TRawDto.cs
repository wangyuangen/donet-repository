using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Img.Model.Sidexis
{
	public class TRawDto
	{
		public int ImageId { get; set; }

		public DateTime ImgCreateTime { get; set; }

		public string PrivateId { get; set; }
		
		public string PatientName { get; set; }

		public string FilePath { get; set; }

		public string UploadStatus { get; set; }

        public string BrandType { get; set; }

        public int Category { get; set; }
	}
}
