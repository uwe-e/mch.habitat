using Sitecore.Diagnostics;
using Sitecore.Rules.RuleMacros;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.Web.UI.Sheer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Sitecore.Foundation.Forms.Rules.RuleMacros
{
    public class ContactFacetMarco : IRuleMacro
    {
        /// <summary>
		/// Executes the specified macro.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="name">The name of the field.</param>
		/// <param name="parameters">The parameters.</param>
		/// <param name="value">The value.</param>
		public void Execute(XElement element, string name, UrlString parameters, string value)
        {
            Assert.ArgumentNotNull(element, "element");
            Assert.ArgumentNotNull(name, "name");
            Assert.ArgumentNotNull(parameters, "parameters");
            Assert.ArgumentNotNull(value, "value");

            UrlString urlString = new UrlString("/sitecore/shell/~/xaml/MCH.Shell.Applications.Dialogs.Forms.ActionEditors.ContactProfileFacets.aspx");
            UrlHandle urlHandle = new UrlHandle();
            urlHandle["mapping"] = value;
            urlHandle.Add(urlString);
            SheerResponse.ShowModalDialog(urlString.ToString(), true);
        }
    }
}