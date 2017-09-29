using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Img.UnitTest
{
    [TestClass]
    public class EasyDentTest
    {
        [TestMethod]
        public void IsContains()
        {
            string Name1 = "TestName1_123456";
            string Name = "TestName";
            Assert.IsTrue(Name1.Contains(Name));
        }
    }
}
