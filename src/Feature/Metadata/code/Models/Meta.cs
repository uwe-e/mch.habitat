using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Metadata.Models
{
    public class Meta
    {
        public string Name
        {
            get;
        }
        public string Content
        {
            get;
        }
        public string AddOnceToken
        {
            get; set;
        }
        public Meta(string name, string content)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            Name = name;
            Content = content;
            this.AddOnceToken = Name.GetHashCode().ToString();
        }
    }
}