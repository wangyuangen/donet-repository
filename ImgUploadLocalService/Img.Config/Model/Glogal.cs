using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Img.Config.Model
{
    public class Global
    {
        public string Name
        {
            get
            {
                return "Global";
            }
        }

        public string Domain { get; set; }
        public string OfficeName { get; set; }
        public string UserName { get; set; }
        public string UserPwd { get; set; }
        public string RunTime { get; set; }
        //固定时段
        public int FixedPeriod { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ImgWepApiHostUrl { get; set; }
        public string ImgUploadHostUri { get; set; }
        public string ManualUploadBrandType { get; set; }
        public string AutoUploadBrandType { get; set; }
        public string UploadType { get; set; }
    }
}
