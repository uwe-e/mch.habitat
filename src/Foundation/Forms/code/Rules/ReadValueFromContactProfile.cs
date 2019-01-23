using Sitecore.Forms.Core.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Analytics;
using Sitecore.Analytics.Tracking;
using Sitecore.WFFM.Abstractions.Analytics;
using Sitecore.Forms.Core.Data.Helpers;
using Sitecore.Analytics.Model.Framework;
using System.Reflection;
using Sitecore.Diagnostics;

namespace Sitecore.Foundation.Forms.Rules
{
    public class ReadValueFromContactProfile<T> : ReadValue<T> where T : ConditionalRuleContext
    {
        private IContactFacetFactory m_contactFacetFactory;

        protected IContactFacetFactory ContactFacetFactory => m_contactFacetFactory ?? (m_contactFacetFactory = ContactFacetsHelper.FacetFactory);

        protected override object GetValue()
        {
            var contact = Tracker.Current?.Contact;
            if (contact != null)
            {
                return GetFacetValue(Name, contact);
            }
            return null;
        }

        protected string GetFacetValue(string facetPath, Contact contact)
        {
            string value = null;
            string[] xPath = facetPath.Split('/');
            string index = xPath[0];
            if (xPath.Length > 1)
            {
                string memberName = xPath[1];
                IFacet facet = contact.Facets[index];
                IEnumerable<IModelMember> members = ContactFacetFactory.GetFacetMembers((IElement)facet);
                IModelMember modelMember = Enumerable.FirstOrDefault<IModelMember>(members, (Func<IModelMember, bool>)(x => x.Name == memberName));

                if (string.Equals(memberName, "Entries", StringComparison.OrdinalIgnoreCase))
                {
                    IElementDictionary<IElement> elementDictionary = facet.GetType().GetProperty("Entries", BindingFlags.Instance | BindingFlags.Public).GetValue((object)facet) as IElementDictionary<IElement>;
                    Assert.IsNotNull((object)elementDictionary, "Can't get facet entries.");
                    IElement element;

                    PropertyInfo property = facet.GetType().GetProperty("Preferred", BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                    var preferredKey = (string)property.GetValue((object)facet);

                    if (!string.IsNullOrWhiteSpace(preferredKey) &&
                        Enumerable.FirstOrDefault<string>((IEnumerable<string>)elementDictionary.Keys, (Func<string, bool>)(x => string.Equals(x, preferredKey, StringComparison.InvariantCultureIgnoreCase))) != null)
                    {
                        element = elementDictionary[preferredKey];
                        memberName = xPath[2];
                        PropertyInfo pi = element.GetType().GetProperty(memberName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                        value = (string)pi.GetValue((object)element);
                    }
                }
                else if (modelMember is IModelAttributeMember && ((IModelAttributeMember)modelMember).Value != null)
                {
                    PropertyInfo pi = facet.GetType().GetProperty(memberName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                    value = System.Convert.ChangeType((object)((IModelAttributeMember)modelMember).Value, pi.PropertyType).ToString();
                }
                else if (facetPath.Remove(0, index.Length + 1).Length > 0)
                {
                    value = this.GetFacetValue(facetPath.Remove(0, index.Length + 1), contact);
                }
            }
            return value;
        }
    }
}