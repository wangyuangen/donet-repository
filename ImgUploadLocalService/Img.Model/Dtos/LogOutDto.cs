using Img.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Img.Model.Dtos
{
	public class LogOutDto:RequestWithToken
	{
		public string Domain { get; set; }
		public string OfficeName { get; set; }
		public string LoginName { get; set; }
	}
}
