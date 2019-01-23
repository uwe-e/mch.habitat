using Sitecore.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Sitecore.Foundation.Forms.Resources
{
    public class ResourceManager
    {
        private static System.Resources.ResourceManager Instance
        {
            get;
        } = new System.Resources.ResourceManager("Resources.Resources", Assembly.Load("App_GlobalResources"));

        public static string GetString(string name)
        {
            return Translate.Text(Instance.GetString(name));
        }
    }
}