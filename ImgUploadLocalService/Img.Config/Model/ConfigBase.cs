using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Img.Config.Model
{
    public class ConfigBase
    {

        public virtual string Name { get; set; }

        public string RootDirectory { get; set; }
        public int ImageCategory { get; set; }
        public string Master { get; set; }
        public string DbConnectString { get; set; }
        public string JobLogDb { get; set; }
    }
}
