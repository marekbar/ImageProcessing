using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageProcessing;

namespace ImageProcessingTests
{
    [TestClass]
    public class TestImage
    {
        [TestMethod]
        public void OpenImageFile()
        {                        
            var image = new Image(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\Images\image1.jpg");
            image.ToGrayScale();
            image.Save(@"C:\Users\marek\Desktop\test.bmp");
        }

        [TestMethod]
        public void ToGrayscale()
        {
            var image = new Image(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\Images\image1.jpg");
            image.ToGrayScale();
            image.Save(@"C:\Users\marek\Desktop\test.bmp");
        }

        [TestMethod]
        public void RemoveGreen()
        {
            var image = new Image(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\Images\image1.jpg");
            image.RemoveColor(ColorChoice.Green);
            image.Save(@"C:\Users\marek\Desktop\test.bmp");
        }

        [TestMethod]
        public void RemoveRed()
        {
            var image = new Image(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\Images\image1.jpg");
            image.RemoveColor(ColorChoice.Red);
            image.Save(@"C:\Users\marek\Desktop\test.bmp");
        }

        [TestMethod]
        public void RemoveBlue()
        {
            var image = new Image(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\Images\image1.jpg");
            image.RemoveColor(ColorChoice.Blue);
            image.Save(@"C:\Users\marek\Desktop\test.bmp");
        }

        [TestMethod]
        public void ToBlackAndWhite()
        {
            var image = new Image(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\Images\image1.jpg");
            image.ToBlackOrWhiteScale(128);
            image.Save(@"C:\Users\marek\Desktop\test.bmp");
        }

        [TestMethod]
        public void MinusValue()
        {
            var image = new Image(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\Images\image1.jpg");
            image.MinusValue(ColorChoice.Blue, 28);
            image.MinusValue(ColorChoice.Green, 128);
            image.MinusValue(ColorChoice.Red, 60);
            image.Save(@"C:\Users\marek\Desktop\test.bmp");
        }
    }
}
