using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Img.Config
{
    public class ConfigAppSettings
    {
        //配置文件路径
        public static string configfile = System.AppDomain.CurrentDomain.BaseDirectory.EndsWith("\\") == true ?
            string.Concat(System.AppDomain.CurrentDomain.BaseDirectory, "ImgUpload.config") :
            string.Format("{0}{1}", System.AppDomain.CurrentDomain.BaseDirectory, "\\ImgUpload.config");

        private static Configuration config;

        public static Configuration ConfigAppSettingsto(string filename)
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = filename;
            config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            return config;
        }
        /// <summary>  
        /// 写入值到appSettings
        /// </summary>  
        /// <param name="key"></param>  
        /// <param name="value"></param>  
        public static void SetValue(string key, string value)
        {
            config = ConfigAppSettingsto(ConfigAppSettings.configfile);
            if (config.AppSettings.Settings[key] == null)
            {
                config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                config.AppSettings.Settings[key].Value = value;
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
        /// <summary>  
        /// 读取appSettings指定key的值  
        /// </summary>  
        /// <param name="key"></param>  
        /// <returns></returns>  
        public static string GetValue(string key)
        {
            try
            {
                config = ConfigAppSettingsto(ConfigAppSettings.configfile);
                if (config.AppSettings.Settings[key] == null)
                {
                    return null;
                }
                else
                {
                    return config.AppSettings.Settings[key].Value;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 写入值到connectionStrings
        /// </summary>
        /// <param name="name"></param>
        /// <param name="connectionstring"></param>
        public static void SetConnectionString(string name, string connectionstring, string providerName = "System.Data.SqlClient")
        {
            config = ConfigAppSettingsto(ConfigAppSettings.configfile);
            if (config.ConnectionStrings.ConnectionStrings[name] == null)
            {
                config.ConnectionStrings.ConnectionStrings.Add
                (
                    new ConnectionStringSettings(name, connectionstring, providerName)
                );
            }
            else
            {
                config.ConnectionStrings.ConnectionStrings[name].ConnectionString = connectionstring;
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("connectionStrings");
        }

        /// <summary>
        /// 读取connectionStrings下指定name的connectionString
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public static string GetConnectionString(string dbName)
        {
            try
            {
                config = ConfigAppSettingsto(ConfigAppSettings.configfile);
                if (config.ConnectionStrings.ConnectionStrings[dbName] == null)
                {
                    return null;
                }
                else
                {
                    return config.ConnectionStrings.ConnectionStrings[dbName].ConnectionString;
                }
            }
            catch
            {
                return null;
            }
            }
    }
}
