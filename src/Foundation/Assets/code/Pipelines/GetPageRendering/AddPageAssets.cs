namespace Sitecore.Foundation.Assets.Pipelines.GetPageRendering
{
    using System.Linq;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Foundation.Assets.Models;
    using Sitecore.Foundation.Assets.Repositories;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.Mvc.Pipelines.Response.GetPageRendering;
    using Sitecore.Mvc.Presentation;

    public class AddPageAssets : GetPageRenderingProcessor
    {
        public override void Process(GetPageRenderingArgs args)
        {
            this.AddAssets(PageContext.Current.Item);
        }

        protected void AddAssets(Item item)
        {
            var styling = this.GetPageAssetValue(item, Templates.PageAssets.Fields.CssCode);
            if (!string.IsNullOrWhiteSpace(styling))
            {
                AssetRepository.Current.AddInlineStyling(styling, true);
            }
            var scriptBottom = this.GetPageAssetValue(item, Templates.PageAssets.Fields.JavascriptCodeBottom);
            if (!string.IsNullOrWhiteSpace(scriptBottom))
            {
                AssetRepository.Current.AddInlineScript(scriptBottom, ScriptLocation.Body, true);
            }
            var scriptHead = this.GetPageAssetValue(item, Templates.PageAssets.Fields.JavascriptCodeTop);
            if (!string.IsNullOrWhiteSpace(scriptHead))
            {
                AssetRepository.Current.AddInlineScript(scriptHead, ScriptLocation.Head, true);
            }
            //each page can have an asset folder. This folder contains one ore more scripts or stylesheets.
            //These assets are added into the page content
            var assetFolder = item.Children.Where(itm => itm.IsDerived(Templates.AssetFolder.ID)).FirstOrDefault();
            if (assetFolder != null)
            {
                var scripts = assetFolder.Children.Where(itm => itm.IsDerived(Templates.ScriptAsset.ID)).ToList();
                if (scripts != null)
                {
                    scripts.ForEach(itm =>
                    {
                        if (itm != null)
                        {
                            string inlineScript = itm.Fields[Templates.ScriptAsset.Fields.Code].Value;
                            if (!string.IsNullOrEmpty(inlineScript))
                            {
                                AssetRepository.Current.AddInlineScript(inlineScript, GetScriptLocation(itm), true);
                            }
                            string file = itm.Fields[Templates.ScriptAsset.Fields.Src].Value;
                            if (!string.IsNullOrEmpty(file))
                            {
                                AssetRepository.Current.AddScriptFile(file, GetScriptLocation(itm), true);
                            }
                        }
                    });
                }
            }
        }

        private string GetPageAssetValue(Item item, ID assetField)
        {
            if (item.IsDerived(Templates.PageAssets.ID))
            {
                var assetValue = item[assetField];
                if (!string.IsNullOrWhiteSpace(assetValue))
                {
                    return assetValue;
                }
            }

            return GetInheritedPageAssetValue(item, assetField);
        }

        private static string GetInheritedPageAssetValue(Item item, ID assetField)
        {
            var inheritedAssetItem = item.Axes.GetAncestors().FirstOrDefault(i => i.IsDerived(Templates.PageAssets.ID) && MainUtil.GetBool(item[Templates.PageAssets.Fields.InheritAssets], false) && string.IsNullOrWhiteSpace(item[assetField]));
            return inheritedAssetItem?[assetField];
        }

        private ScriptLocation GetScriptLocation(Item scriptItem)
        {
            ScriptLocation scriptLocation = ScriptLocation.Head;
            if (scriptItem != null)
            {
                var location = scriptItem.TargetItem(Templates.ScriptAsset.Fields.ScriptLocation);
                if (location != null)
                {
                    scriptLocation = location.ID.Equals(new Sitecore.Data.ID("{751F0BFF-A39C-4242-B089-CA53D605263E}")) ? ScriptLocation.Body : ScriptLocation.Head;
                }
            }
            return scriptLocation;
        }
    }
}