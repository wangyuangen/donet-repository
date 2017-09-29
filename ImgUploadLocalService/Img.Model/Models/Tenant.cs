using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Img.Model.Models
{
    public class Tenant : RequestWithToken
    {
        public string Domain { get; set; }
        public string OfficeName { get; set; }
        public string Directory { get; set; }
        public int Category { get; set; }
        public string UserName { get; set; }
        public string UserPwd { get; set; }
    }
}
