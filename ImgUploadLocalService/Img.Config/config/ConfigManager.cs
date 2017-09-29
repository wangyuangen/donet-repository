using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Img.Config.Model;

namespace Img.Config.config
{
    public class ConfigManager
    {
        private static object obj_Lock = new object();

        private static ConfigManager _instance;
        public static ConfigManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj_Lock)
                    {
                        if (_instance == null)
                        {
                            //var config = ConfigAppSettings.ConfigAppSettingsto(ConfigAppSettings.configfile);
                            //_instance = config.GetSection("ImageConfiguration") as ImageConfiguration;
                            _instance = new ConfigManager();
                        }
                    }
                }
                return _instance;
            }
        }

        public ConfigBase GetConfig(string ConfigName)
        {
            var configbase = XmlHelper.ReadXmlNode(ConfigName);
            configbase.Name=ConfigName;
            return configbase;
        }
        //[ConfigurationProperty("Glogal")]
        public Global Global
        {
            get
            {
                return XmlHelper.ReadXmlNode<Global>("Global");
            }
        }

        //[ConfigurationProperty("Sidexis")]
        //public Sidexis Sidexis
        //{
        //    get
        //    {
        //        return XmlHelper.ReadXmlNode<Sidexis>("Sidexis");
        //    }
        //}

        //[ConfigurationProperty("EasyDent")]
        //public EasyDent EasyDent
        //{
        //    get
        //    {
        //        return XmlHelper.ReadXmlNode<EasyDent>("EasyDent");
        //    }
        //}
    }
}
