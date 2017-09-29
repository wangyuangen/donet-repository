using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Img.Model.Dtos
{
	public class AppointmentDto
	{
		public int Id { get; set; }
        public DateTime RecordCreatedTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int PatientId { get; set; }
        public bool IsFirstVisit { get; set; }
        public string SourceType { get; set; }
        public string Notes { get; set; }
	}
}
