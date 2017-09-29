using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Img.Model.Models
{
	public class RequestWithToken
	{
		public Guid TenantId { get; set; }
        public int OfficeId { get; set; }
		public string Token { get; set; }
	}
}
