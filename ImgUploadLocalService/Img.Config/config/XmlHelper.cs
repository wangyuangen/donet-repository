using Img.Config.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Img.Config.config
{
    public class XmlHelper
    {
        //配置文件路径
        private static string configfile = System.AppDomain.CurrentDomain.BaseDirectory.EndsWith("\\") == true ?
            string.Concat(System.AppDomain.CurrentDomain.BaseDirectory, "ImgUpload.config") :
            string.Format("{0}{1}", System.AppDomain.CurrentDomain.BaseDirectory, "\\ImgUpload.config");

        public static void WriteXmlNode<T>(string node, T config)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(configfile);

            XmlNodeList nodeList = xmlDoc.SelectNodes(string.Format("/configuration/ImageConfiguration/{0}", node));
            Type configType = config.GetType();

            foreach (XmlNode xn in nodeList)
            {
                XmlElement xe = (XmlElement)xn;
                foreach (var item in configType.GetProperties())
                {
                    if (xe.GetElementsByTagName(item.Name).Item(0) == null)
                    {
                        continue;
                    }

                    string value = item.GetValue(config) == null ? "" : item.GetValue(config).ToString();
                    xe.GetElementsByTagName(item.Name).Item(0).InnerText = value;
                }
            }
            xmlDoc.Save(configfile);
        }

        public static T ReadXmlNode<T>(string node)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(configfile);

            XmlNodeList nodeList = xmlDoc.SelectNodes(string.Format("/configuration/ImageConfiguration/{0}", node));
            T config = Activator.CreateInstance<T>();
            Type configType = typeof(T);

            foreach (XmlNode xn in nodeList)
            {
                XmlElement xe = (XmlElement)xn;
                foreach (var item in configType.GetProperties())
                {
                    if (xe.GetElementsByTagName(item.Name).Item(0) == null)
                    {
                        continue;
                    }

                    string content = xe.GetElementsByTagName(item.Name).Item(0).InnerText;
                    if (content == "")
                    {
                        continue;
                    }
                    item.SetValue(config, Convert.ChangeType(content, item.PropertyType));
                }
            }

            return config;
        }

        public static ConfigBase ReadXmlNode(string node)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(configfile);

            XmlNodeList nodeList = xmlDoc.SelectNodes(string.Format("/configuration/ImageConfiguration/{0}", node));
            ConfigBase config = new ConfigBase();
            Type configType = typeof(ConfigBase);

            foreach (XmlNode xn in nodeList)
            {
                XmlElement xe = (XmlElement)xn;
                foreach (var item in configType.GetProperties())
                {
                    if (xe.GetElementsByTagName(item.Name).Item(0) == null)
                    {
                        continue;
                    }

                    string content = xe.GetElementsByTagName(item.Name).Item(0).InnerText;
                    if (content == "")
                    {
                        continue;
                    }

                    item.SetValue(config, Convert.ChangeType(content, item.PropertyType));
                }
            }

            return config;
        }
    }
}
