using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing
{
    public class ForMenuAttribute : Attribute
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public Type ResourceType { get; set; }

        public string CategoryText
        {
            get
            {
                return GetText(Category);
            }
        }

        public string NameText
        {
            get
            {
                return GetText(Name);
            }
        }

        private string GetText(string name)
        {                      
            if (string.IsNullOrEmpty(name)) return string.Empty;
            return ResourceHelper.GetResourceLookup(ResourceType, name);
         
        }
    }
}
