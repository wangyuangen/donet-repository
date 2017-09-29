using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Img.DataService.Imp;
using Img.Nlog;
using Img.Nlog.Imp;

namespace Img.DataService.Infrastructure
{
    public class ContainerManager
    {
        private static object objLock = new object();

        private ContainerManager()
        {

        }
        private static IUnityContainer _Container;
        public static IUnityContainer Container
        {
            get
            {
                if (_Container == null)
                {
                    lock (objLock)
                    {
                        if (_Container == null)
                        {
                            _Container = new UnityContainer();
                        }
                    }
                }

                return _Container;
            }
        }

        public static object GetDataService(Type t, string Name)
        {
            return _Container.Resolve(t, Name);
        }

        public static T GetDataService<T>(string Name)
        {
            return _Container.Resolve<T>(Name);
        }

        //public static IDataService SidexisDataService
        //{
        //    get
        //    {
        //        return Container.Resolve<SidexisDataService>("Sidexis");
        //    }
        //}

        //public static IDataService EasyDentDataService
        //{
        //    get
        //    {
        //        return Container.Resolve<EasyDentDataService>("EasyDent");
        //    }
        //}

        //public static ILogger NLogger
        //{
        //    get
        //    {
        //        return Container.Resolve<Logger>("NLog");
        //    }
        //}

        //public static ILogger JobLogger
        //{
        //    get
        //    {
        //        return Container.Resolve<JobLogger>("JobLog");
        //    }
        //}
    }
}
