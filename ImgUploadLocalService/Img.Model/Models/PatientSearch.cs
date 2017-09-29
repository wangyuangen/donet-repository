using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Img.Model.Models
{
	public class PatientSearch:RequestWithToken
	{
		public string Blh { get; set; }
	}
}
