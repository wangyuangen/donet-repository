using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Img.Model.Dtos
{
	public class LogonDto
	{  
		public string AccountName { get; set; }
        public string Password { get; set; }
        public string TenantDomain { get; set; }
        public int OfficeId { get; set; }
	}
}
