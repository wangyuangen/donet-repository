using Img.DataService;
using Img.DataService.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Img.UnitTest
{
    [TestClass]
    public class SironaService
    {
        [TestMethod]
        public void TestInject()
        {
            //Img.DataService.StartUp.Initialization();
            //IDataService dataService = ContainerManager.SidexisDataService;
            //Assert.IsNotNull(dataService);
        }

        [TestMethod]
        public void TestExpression()
        {
            ConstantExpression constantA = Expression.Constant(1);
            ConstantExpression constantB = Expression.Constant(2);

            BinaryExpression binary = Expression.AddAssign(constantA,constantB);
        }
    }
}
