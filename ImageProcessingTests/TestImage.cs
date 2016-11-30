using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageProcessing;
using System.Drawing;
using ImageProcessing.ImageOperations;

namespace ImageProcessingTests
{
    [TestClass]
    public class TestImage
    {
        private string file;
        public TestImage()
        {
            file = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\Images\image1.jpg";
        }
        [TestMethod]
        public void OpenImageFile()
        {                        
            var image = new Bitmap(file);
            image.Grayscale();
            image.Save(@"C:\Users\marek\Desktop\test.bmp");
        }

        [TestMethod]
        public void ToGrayscale()
        {
           var image = new Bitmap(file);
            image.Grayscale();
            image.Save(@"C:\Users\marek\Desktop\test.bmp");
        }

        [TestMethod]
        public void RemoveGreen()
        {
           var image = new Bitmap(file);
            image.ColorRemove(ColorChoice.Green);
            image.Save(@"C:\Users\marek\Desktop\test.bmp");
        }

        [TestMethod]
        public void RemoveRed()
        {
           var image = new Bitmap(file);
            image.ColorRemove(ColorChoice.Red);
            image.Save(@"C:\Users\marek\Desktop\test.bmp");
        }

        [TestMethod]
        public void RemoveBlue()
        {
           var image = new Bitmap(file);
            image.ColorRemove(ColorChoice.Blue);
            image.Save(@"C:\Users\marek\Desktop\test.bmp");
        }

        [TestMethod]
        public void ToBlackAndWhite()
        {
           var image = new Bitmap(file);
            image.BlackWhite(128);
            image.Save(@"C:\Users\marek\Desktop\test.bmp");
        }

        [TestMethod]
        public void MinusValue()
        {
           var image = new Bitmap(file);
            image.MinusValue(ColorChoice.Blue, 28);
            image.MinusValue(ColorChoice.Green, 128);
            image.MinusValue(ColorChoice.Red, 60);
            image.Save(@"C:\Users\marek\Desktop\test.bmp");
        }
    }
}
