using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Img.Model.Models
{
	public class AppointmentSearch:RequestWithToken
	{
		public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string PrivateId { get; set; }
	}
}
