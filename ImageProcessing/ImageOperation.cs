using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace ImageProcessing
{
    public class ImageOperation
    {
        private List<KeyValuePair<string, Bitmap>> Operations = new List<KeyValuePair<string, Bitmap>>();
        private Dictionary<string,int> History
        {
            get
            {
                Dictionary<string, int> h = new Dictionary<string, int>();
                for(int i = 0; i < Operations.Count; i++)
                {
                    var name = (typeof(ImageOperation).GetMethod(Operations[i].Key).GetCustomAttributes(typeof(ForMenuAttribute)).ToArray()[0] as ForMenuAttribute)
                        .NameText;
                    h[name] = i;
                }
                return h;
            }
        }

        public delegate void ImageLoaded(Bitmap image, List<Action> availableActions);
        public event ImageLoaded OnImageLoaded;

        public delegate void ImageChanged(Bitmap image);
        public event ImageChanged OnImageChanged;

        public delegate void Error(string message);
        public event Error OnError;

        public delegate void Executed(string message, Dictionary<string, int> history);
        public event Executed OnExecuted;

        private Image img;
        public void SetImage(string filename)
        {
            img = new Image(filename);
            OnImageLoaded?.Invoke(img.GetCopy(), ListOperations());
        }

        public ImageOperation()
        {
            
        }

        private List<Action> ListOperations()
        {
            var list = new List<Action>();
            var methods = this.GetType().GetMethods()
                .Where(q =>
                q.DeclaringType == typeof(ImageOperation) &&
                q.IsPublic &&
                !q.IsStatic &&
                !q.IsVirtual)
                .ToList();
            foreach (var method in methods)
            {
                var attr = method.GetCustomAttributes(typeof(ForMenuAttribute)).ToArray();
                if (attr != null && attr.Length > 0)
                {
                    ForMenuAttribute action = (ForMenuAttribute)attr[0];

                    list.Add(new Action
                    {
                        Category = action.CategoryText,
                        ActionName = action.NameText,
                        ActionMethod = method.Name
                    });
                }
            }
            return list;
        }

        [ForMenu(Category = "BasicEdition", Name = "Grayscale", ResourceType = typeof(Resources.Resource))]
        public void Grayscale()
        {
            try
            {
                Operations.Add(new KeyValuePair<string, Bitmap>(nameof(Grayscale), img.GetCopy()));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                img.ToGrayScale();
                sw.Stop();
                OnExecuted?.Invoke(sw.ElapsedMilliseconds.ToString() + "ms", History);
                OnImageChanged?.Invoke(img.GetCopy());
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
        }

        [ForMenu(Category = "ContrastEdition", Name = "Contrast1", ResourceType = typeof(Resources.Resource))]
        public void AdjustContrast1()
        {
            try
            {
                Operations.Add(new KeyValuePair<string, Bitmap>(nameof(AdjustContrast1), img.GetCopy()));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                img.AdjustContrast(1);
                sw.Stop();
                OnExecuted?.Invoke(sw.ElapsedMilliseconds.ToString() + "ms", History);
                OnImageChanged?.Invoke(img.GetCopy());
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
        }

        [ForMenu(Category = "ContrastEdition", Name = "Contrast5", ResourceType = typeof(Resources.Resource))]
        public void AdjustContrast5()
        {
            try
            {
                Operations.Add(new KeyValuePair<string, Bitmap>(nameof(AdjustContrast5), img.GetCopy()));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                img.AdjustContrast(5);
                sw.Stop();
                OnExecuted?.Invoke(sw.ElapsedMilliseconds.ToString() + "ms", History);
                OnImageChanged?.Invoke(img.GetCopy());
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
        }

        [ForMenu(Category = "ContrastEdition", Name = "Contrast10", ResourceType = typeof(Resources.Resource))]
        public void AdjustContrast10()
        {
            try
            {
                Operations.Add(new KeyValuePair<string, Bitmap>(nameof(AdjustContrast10), img.GetCopy()));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                img.AdjustContrast(10);
                sw.Stop();
                OnExecuted?.Invoke(sw.ElapsedMilliseconds.ToString() + "ms", History);
                OnImageChanged?.Invoke(img.GetCopy());
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
        }

        [ForMenu(Category = "BrightnessEdition", Name = "BrightnessPlus10", ResourceType = typeof(Resources.Resource))]
        public void AdjustBrightnessPlus10()
        {
            try
            {
                Operations.Add(new KeyValuePair<string, Bitmap>(nameof(AdjustBrightnessPlus10), img.GetCopy()));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                img.AdjustBrightness(10);
                sw.Stop();
                OnExecuted?.Invoke(sw.ElapsedMilliseconds.ToString() + "ms", History);
                OnImageChanged?.Invoke(img.GetCopy());
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
        }

        [ForMenu(Category = "BrightnessEdition", Name = "BrightnessMinus10", ResourceType = typeof(Resources.Resource))]
        public void AdjustBrightnessMinus10()
        {
            try
            {
                Operations.Add(new KeyValuePair<string, Bitmap>(nameof(AdjustBrightnessMinus10), img.GetCopy()));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                img.AdjustBrightness(-10);
                sw.Stop();
                OnExecuted?.Invoke(sw.ElapsedMilliseconds.ToString() + "ms", History);
                OnImageChanged?.Invoke(img.GetCopy());
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
        }

        [ForMenu(Category = "ColorEdition", Name = "RemoveGreen", ResourceType = typeof(Resources.Resource))]
        public void RemoveGreen()
        {
            try
            {
                Operations.Add(new KeyValuePair<string, Bitmap>(nameof(RemoveGreen), img.GetCopy()));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                img.RemoveColor(ColorChoice.Green);
                sw.Stop();
                OnExecuted?.Invoke(sw.ElapsedMilliseconds.ToString() + "ms", History);
                OnImageChanged?.Invoke(img.GetCopy());
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
        }

        [ForMenu(Category = "ColorEdition", Name = "RemoveRed", ResourceType = typeof(Resources.Resource))]
        public void RemoveRed()
        {
            try
            {
                Operations.Add(new KeyValuePair<string, Bitmap>(nameof(RemoveRed), img.GetCopy()));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                img.RemoveColor(ColorChoice.Red);
                sw.Stop();
                OnExecuted?.Invoke(sw.ElapsedMilliseconds.ToString() + "ms", History);
                OnImageChanged?.Invoke(img.GetCopy());
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
        }

        [ForMenu(Category = "ColorEdition", Name = "RemoveBlue", ResourceType = typeof(Resources.Resource))]
        public void RemoveBlue()
        {
            try
            {
                Operations.Add(new KeyValuePair<string, Bitmap>(nameof(RemoveBlue), img.GetCopy()));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                img.RemoveColor(ColorChoice.Blue);
                sw.Stop();
                OnExecuted?.Invoke(sw.ElapsedMilliseconds.ToString() + "ms", History);
                OnImageChanged?.Invoke(img.GetCopy());
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
        }

        [ForMenu(Category = "BasicEdition", Name = "BlackAndWhite", ResourceType = typeof(Resources.Resource))]
        public void BlackAndWhite()
        {
            try
            {
                Operations.Add(new KeyValuePair<string, Bitmap>(nameof(BlackAndWhite), img.GetCopy()));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                img.ToBlackOrWhiteScale(128);
                sw.Stop();
                OnExecuted?.Invoke(sw.ElapsedMilliseconds.ToString() + "ms", History);
                OnImageChanged?.Invoke(img.GetCopy());
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
        }

    }
}
