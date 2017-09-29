using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Img.Model.DBSwin
{
	public class DBSwinInfo
	{
		public string Vorgnr { get; set; }
		public string PrivateId { get; set; }
		public string PVName { get; set; }
		public string PNName { get; set; }
		public string PatientName { get; set; }
		public string ImgFile { get; set; }

		public DateTime CreatedDate { get; set; }
		public TimeSpan CreatedTime { get; set; }
		public DateTime CheckTime { get; set; }

		public string Status { get; set; }
		public string BrandType { get; set; }
		public ImageCategory Category { get; set; }
	}

	public enum ImageCategory
	{
		PANORAMA = 1,
		SENSOR = 2,
		CBCT = 3,
		CAMERA = 4,
		OTHER = 5
	}
}
