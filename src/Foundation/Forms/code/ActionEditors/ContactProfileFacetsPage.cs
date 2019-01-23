using Sitecore.Analytics.Model.Framework;
using Sitecore.Controls;
using Sitecore.Diagnostics;
using Sitecore.Forms.Core.Data.Helpers;
using Sitecore.Forms.Shell.UI.Dialogs;
using Sitecore.Foundation.Accounts.Providers;
using Sitecore.Foundation.Forms.Resources;
using Sitecore.Web;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.XamlSharp.Ajax;
using Sitecore.Web.UI.XamlSharp.Xaml;
using Sitecore.WFFM.Abstractions.Analytics;
using Sitecore.WFFM.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Sitecore.Foundation.Forms.ActionEditors
{
    public class ContactProfileFacetsPage : SmartDialogPage
    {
        private IContactFacetFactory m_contactFacetFactory;
        protected TreeView myTreeView;
        protected HtmlInputHidden MappedFields;

        protected IContactFacetFactory ContactFacetFactory => m_contactFacetFactory ?? (m_contactFacetFactory = ContactFacetsHelper.FacetFactory);

        protected override void OnInit(EventArgs e)
        {
            MappedFields.Value = StringUtil.GetString(
                UrlHandle.Get()["mapping"], string.Empty);
            base.OnInit(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                myTreeView.Nodes.Clear();

                IReadOnlyDictionary<string, IFacet> contactFacets = ContactFacetFactory.ContactFacets;
                if (contactFacets != null)
                {
                    foreach (KeyValuePair<string, IFacet> contactFacet in contactFacets)
                    {
                        myTreeView.Nodes.Add(GetTreeNode(contactFacet.Key, contactFacet.Value, contactFacet.Key));
                    }
                }
                ExpandParent(myTreeView.SelectedNode);
                myTreeView.DataBind();

            }
        }
        protected override void ExecuteAjaxCommand(AjaxCommandEventArgs args)
        {
            Assert.ArgumentNotNull(args, "args");

            switch (args.Name)
            {
                case "contactprofilefacetspage:facetselected":
                    XamlControl.AjaxScriptManager.SetDialogValue(args.Parameters["path"]);
                    SheerResponse.CloseWindow();
                    break;
            }
        }
        protected override void Localize()
        {
            base.Localize();
            Header = ResourceManager.GetString("test");
        }
        /// <summary>
        /// Retrieves the differnt facet members to be placed into TreeNode objects for disaply
        /// </summary>
        /// <param name="name"></param>
        /// <param name="facet"></param>
        /// <returns></returns>
        private TreeNode GetTreeNode(string name, IElement facet, string path)
        {
            TreeNode treeNode = new TreeNode
            {
                Text = name,
                Value = path
            };

            IEnumerable<IModelMember> facetMembers = ContactFacetFactory.GetFacetMembers(facet);
            if (facetMembers != null && facetMembers.Any())
            {
                foreach (IModelMember modelMember in facetMembers)
                {
                    if (modelMember is IModelAttributeMember && !string.Equals(modelMember.Name, "Preferred", StringComparison.OrdinalIgnoreCase))
                    {
                        var itemPath = string.Format("{0}/{1}", path, modelMember.Name);
                        var node = new TreeNode(modelMember.Name, itemPath);
                        node.NavigateUrl = "javascript: scForm.invoke(\"contactprofilefacetspage:facetselected(path=" + itemPath + ")\");";
                        node.Selected = itemPath.ToLowerInvariant() == this.MappedFields.Value.ToLowerInvariant();
                        treeNode.ChildNodes.Add(node);
                    }
                    else if (modelMember is IModelElementMember)
                    {
                        IModelElementMember modelElementMember = modelMember as IModelElementMember;
                        treeNode.ChildNodes.Add(GetTreeNode(modelElementMember.Name, modelElementMember.Element, string.Format("{0}/{1}", path, modelElementMember.Name)));
                    }
                    else if (modelMember is IModelDictionaryMember || modelMember is IModelCollectionMember)
                    {
                        IModelDictionaryMember dictionaryMember = modelMember as IModelDictionaryMember;
                        Type type = dictionaryMember == null ? Enumerable.Single<Type>(
                            (IEnumerable<Type>)(modelMember as IModelCollectionMember).Elements.GetType().GetGenericArguments()) : Enumerable.Single<Type>((IEnumerable<Type>)dictionaryMember.Elements.GetType().GetGenericArguments());
                        if (!(type == (Type)null))
                        {
                            treeNode.ChildNodes.Add(
                                GetTreeNode(
                                    "Entries",
                                    ContactFacetFactory.CreateElement(type), string.Format("{0}/{1}", path, "Entries")));
                        }
                    }
                }
            }
            return treeNode;
        }

        private void ExpandParent(TreeNode treeNode)
        {
            treeNode?.Expand();
            if (treeNode?.Parent != null)
            {
                ExpandParent(treeNode.Parent);
            }
        }
    }
}