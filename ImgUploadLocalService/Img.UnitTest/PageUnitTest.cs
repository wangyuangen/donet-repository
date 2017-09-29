using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Img.UnitTest
{
    [TestClass]
    public class PageUnitTest
    {
        [TestMethod]
        public void CalculatePageCount()
        {
            int pageSize = 10;
            int totalCount = 98;
            int pageCount = totalCount % pageSize + 1;
            Assert.AreEqual(10, pageCount);
        }
    }
}
