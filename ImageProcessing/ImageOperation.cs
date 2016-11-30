using ImageProcessing.Filters;
using ImageProcessing.ImageOperations;
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

        private Bitmap img;
        public void SetImage(string filename)
        {
            img = new Bitmap(filename);
            OnImageLoaded?.Invoke(img.Copy(), ListOperations());
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
                Operations.Add(new KeyValuePair<string, Bitmap>(nameof(Grayscale), img.Copy()));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                img.Grayscale();
                sw.Stop();
                OnExecuted?.Invoke(sw.ElapsedMilliseconds.ToString() + "ms", History);
                OnImageChanged?.Invoke(img.Copy());
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
                Operations.Add(new KeyValuePair<string, Bitmap>(nameof(AdjustContrast1), img.Copy()));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                img.Contrast(1);
                sw.Stop();
                OnExecuted?.Invoke(sw.ElapsedMilliseconds.ToString() + "ms", History);
                OnImageChanged?.Invoke(img.Copy());
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
                Operations.Add(new KeyValuePair<string, Bitmap>(nameof(AdjustContrast5), img.Copy()));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                img.Contrast(5);
                sw.Stop();
                OnExecuted?.Invoke(sw.ElapsedMilliseconds.ToString() + "ms", History);
                OnImageChanged?.Invoke(img.Copy());
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
        }

        [ForMenu(Category = "FiltersMenu", Name = "FilterSharpen", ResourceType = typeof(Resources.Resource))]
        public void Sharpen()
        {
            try
            {
                Operations.Add(new KeyValuePair<string, Bitmap>(nameof(Sharpen), img.Copy()));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                img.Filter(FilterFactory.Sharpen);
                sw.Stop();
                OnExecuted?.Invoke(sw.ElapsedMilliseconds.ToString() + "ms", History);
                OnImageChanged?.Invoke(img.Copy());
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
        }

        [ForMenu(Category = "FiltersMenu", Name = "FilterAverage", ResourceType = typeof(Resources.Resource))]
        public void Average()
        {
            try
            {
                Operations.Add(new KeyValuePair<string, Bitmap>(nameof(Average), img.Copy()));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                img.Filter(FilterFactory.AverageFilter);
                sw.Stop();
                OnExecuted?.Invoke(sw.ElapsedMilliseconds.ToString() + "ms", History);
                OnImageChanged?.Invoke(img.Copy());
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
        }

        [ForMenu(Category = "FiltersMenu", Name = "FilterSobleHorizontal", ResourceType = typeof(Resources.Resource))]
        public void SobelHorizontal()
        {
            try
            {
                Operations.Add(new KeyValuePair<string, Bitmap>(nameof(SobelHorizontal), img.Copy()));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                img.Filter(FilterFactory.EdgesSobelHorizontal);
                sw.Stop();
                OnExecuted?.Invoke(sw.ElapsedMilliseconds.ToString() + "ms", History);
                OnImageChanged?.Invoke(img.Copy());
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
        }

        [ForMenu(Category ="FiltersMenu", Name = "EmbossEast", ResourceType = typeof(Resources.Resource))]
        public void EmbossEast()
        {
            try
            {
                Operations.Add(new KeyValuePair<string, Bitmap>(nameof(EmbossEast), img.Copy()));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                img.Filter(FilterFactory.EmbossEast);
                sw.Stop();
                OnExecuted?.Invoke(sw.ElapsedMilliseconds.ToString() + "ms", History);
                OnImageChanged?.Invoke(img.Copy());
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
                Operations.Add(new KeyValuePair<string, Bitmap>(nameof(AdjustContrast10), img.Copy()));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                img.Contrast(10);
                sw.Stop();
                OnExecuted?.Invoke(sw.ElapsedMilliseconds.ToString() + "ms", History);
                OnImageChanged?.Invoke(img.Copy());
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
                Operations.Add(new KeyValuePair<string, Bitmap>(nameof(AdjustBrightnessPlus10), img.Copy()));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                img.Brightness(10);
                sw.Stop();
                OnExecuted?.Invoke(sw.ElapsedMilliseconds.ToString() + "ms", History);
                OnImageChanged?.Invoke(img.Copy());
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
                Operations.Add(new KeyValuePair<string, Bitmap>(nameof(AdjustBrightnessMinus10), img.Copy()));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                img.Brightness(-10);
                sw.Stop();
                OnExecuted?.Invoke(sw.ElapsedMilliseconds.ToString() + "ms", History);
                OnImageChanged?.Invoke(img.Copy());
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
                Operations.Add(new KeyValuePair<string, Bitmap>(nameof(RemoveGreen), img.Copy()));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                img.ColorRemove(ColorChoice.Green);
                sw.Stop();
                OnExecuted?.Invoke(sw.ElapsedMilliseconds.ToString() + "ms", History);
                OnImageChanged?.Invoke(img.Copy());
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
                Operations.Add(new KeyValuePair<string, Bitmap>(nameof(RemoveRed), img.Copy()));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                img.ColorRemove(ColorChoice.Red);
                sw.Stop();
                OnExecuted?.Invoke(sw.ElapsedMilliseconds.ToString() + "ms", History);
                OnImageChanged?.Invoke(img.Copy());
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
                Operations.Add(new KeyValuePair<string, Bitmap>(nameof(RemoveBlue), img.Copy()));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                img.ColorRemove(ColorChoice.Blue);
                sw.Stop();
                OnExecuted?.Invoke(sw.ElapsedMilliseconds.ToString() + "ms", History);
                OnImageChanged?.Invoke(img.Copy());
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
                Operations.Add(new KeyValuePair<string, Bitmap>(nameof(BlackAndWhite), img.Copy()));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                img.BlackWhite(128);
                sw.Stop();
                OnExecuted?.Invoke(sw.ElapsedMilliseconds.ToString() + "ms", History);
                OnImageChanged?.Invoke(img.Copy());
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
        }

    }
}
