using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageMagick;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Img.UnitTest
{
    [TestClass]
    public class ImageUnitTest
    {
        [TestMethod]
        public void ConvertImage()
        {
            string filepath = @"C:\PDATA\006540.TIF";
            string newfilepath = @"C:\PDATA\006540_01.jpg";
            using (MagickImage image = new MagickImage(filepath))
            {
                image.Write(newfilepath);
            }

        }
    }
}
