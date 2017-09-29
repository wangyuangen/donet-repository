using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Img.ManualUpload;
using Microsoft.Practices.Unity;

namespace Img.UnitTest
{
    [TestClass]
    public class UnityTest
    {
        [TestMethod]
        public void TestUnityConfig()
        {
            //var Unity1 = UnityConfig.Container;
            //var Unity2 = UnityConfig.Container;

            //Assert.ReferenceEquals(Unity1,Unity2);
        }

        [TestMethod]
        public void TestIsExist()
        {

            //var container = UnityConfig.Container;
            //var test = container.Resolve<Test>();
            ////Assert.IsTrue(container != null);
            //Assert.IsNull(test);
        }
    }

    public class Test:ITest
    {

    }
    public interface ITest
    {

    }
}
