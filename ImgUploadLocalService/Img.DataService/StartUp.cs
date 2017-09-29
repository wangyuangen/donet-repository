using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using AutoMapper;
using Img.DataService.Infrastructure;
using Img.DataService.Imp;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Img.Model.Models;
using Img.Nlog;
using Img.Nlog.Imp;
using Img.Config.Model;
using Img.Config.config;

namespace Img.DataService
{
    public class StartUp
    {
        public static void Initialization()
        {
            ProfileRegister();
            RegisterTypes(ContainerManager.Container);
        }

        private static void ProfileRegister()
        {
            var profiles = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(Profile).IsAssignableFrom(t) && t.GetConstructor(Type.EmptyTypes) != null)
                .Select(t => Activator.CreateInstance(t)).Cast<Profile>().ToList();

            Mapper.Initialize(m => profiles.ForEach(m.AddProfile));
        }

        private static void RegisterTypes(IUnityContainer Container)
        {
            Container.RegisterType<ILogger, Logger>();
            //Container.RegisterType<ILogger, JobLogger>("JobLog");

            Container.RegisterType<IAccountService, AccountService>();
            Container.RegisterType<IAppointmentService, AppointmentService>();
            Container.RegisterType<IImgUploadService, ImgUploadService>();
            Container.RegisterType<IPatientService, PatientService>();
            Container.RegisterType<IDataService, SidexisDataService>("Sidexis", new ContainerControlledLifetimeManager());
            Container.RegisterType<IDataService, EasyDentDataService>("EasyDent", new ContainerControlledLifetimeManager());
        }


        public static Dictionary<string, ConfigBase> LoadConfig()
        {
            Dictionary<string, ConfigBase> dicConfig = new Dictionary<string, ConfigBase>();

            var ConfigLayer = AppDomain.CurrentDomain.GetAssemblies()
                               .Where(a => a.FullName.Contains("Img.Config")).FirstOrDefault();
            if (ConfigLayer == null)
            {
                throw new Exception("未初始化Config组件");
            }

            var configs = ConfigLayer.GetTypes()
                   .Where(m => typeof(ConfigBase).IsAssignableFrom(m)
                       && m.Name != "ConfigBase").ToList();

            foreach (var item in configs)
            {
                Type type = item.GetType();
                var instance = Activator.CreateInstance(item);
                var properties = type.GetProperties();
                foreach (var p in properties)
                {
                    if (p.Name == "Name")
                    {
                        string value = p.GetValue(item).ToString();
                        if (value != "")
                        {
                            dicConfig.Add(value, ConfigManager.Instance.GetConfig(value));
                        }
                        break;
                    }
                }
            }

            return dicConfig;
        }

        public static Dictionary<string, IDataService> LoadServices()
        {
            Dictionary<string, IDataService> dicServices = new Dictionary<string, IDataService>();

            var ServiceLayer = AppDomain.CurrentDomain.GetAssemblies()
                               .Where(a => a.FullName.Contains("Img.DataService")).FirstOrDefault();
            if (ServiceLayer == null)
            {
                throw new Exception("未初始化Service组件");
            }

            var dataServices = ServiceLayer.GetTypes()
                               .Where(m => typeof(IDataService).IsAssignableFrom(m)
                                   && m.Name != "IDataService").ToList();

            foreach (var item in dataServices)
            {
                Type type = item.GetType();
                var instance = Activator.CreateInstance(item);
                var properties = type.GetProperties();
                foreach (var p in properties)
                {
                    if (p.Name == "Name")
                    {
                        string value = p.GetValue(item).ToString();
                        if (value != "")
                        {
                            dicServices.Add(value, ContainerManager.GetDataService(instance.GetType(), value) as IDataService);
                        }
                        break;
                    }
                }
            }

            return dicServices;
        }

        public static void CreateLogDataBase(string connectiongStr)
        {
            var JobLogDataService = new JobLogData.JobLogDataService();
            JobLogDataService.CreateDatabase(connectiongStr);
        }
    }
}
