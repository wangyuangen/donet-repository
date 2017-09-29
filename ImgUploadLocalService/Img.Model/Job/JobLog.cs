using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Img.Model.Job
{
	public class JobLog
	{
		public int Id { get; set; }
		public string ImgId { get; set; }
		public DateTime ImgCreateTime { get; set; }
		public DateTime UploadTime { get; set; }
		public string PatientName { get; set; }
		public string PrivateId { get; set; }
		public UploadStatus UploadStatus { get; set; }
		public string ErrorLog { get; set; }
        public string BrandType { get; set; }
	}
	
	
	public enum UploadStatus
	{
		Successed=0,
		Failed,
	}
}
