using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Img.Model.PointNix
{
	public class ImageInfo
	{
		public int VideoNum { get; set; }				//ID
		public string PatientId { get; set; }	//ChartNo_T
		public string PatientName { get; set; }	//PatientNm_T
		public string PathFix { get; set; }	//PathFix_T
		public string PathVar { get; set; }	//PathVar_T
		public string FileNm { get; set; }	//FileNm_T
		public string ImageGuid { get; set; }	//ImageID_T

		public string CreatedDate { get; set; }
		public string CreatedTime { get; set; }

		public string FilePath { get; set; }

		public DateTime CheckTime { get; set; }	//StudyDateTime

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
