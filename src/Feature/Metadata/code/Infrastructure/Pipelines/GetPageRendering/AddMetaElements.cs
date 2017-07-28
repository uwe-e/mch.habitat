using Sitecore.Data.Items;
using Sitecore.Feature.Metadata.Repositories;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Mvc.Pipelines.Response.GetPageRendering;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Metadata.Infrastructure.Pipelines.GetPageRendering
{
    public class AddMetaElements : Sitecore.Foundation.Assets.Pipelines.GetPageRendering.AddPageAssets
    {
        protected override void AddAssets(Item item)
        {
            base.AddAssets(item);
            //Add the global assets to the repository
            AddAssetsToRepository(Sitecore.Context.Site.GetGlobalFolder());
            //each page can have an asset folder. This folder contains one ore more scripts or stylesheets.
            //These assets are added into the page content
            AddAssetsToRepository(item);
        }

        private void AddAssetsToRepository(Item item)
        {
            if (item != null)
            {
                var metaElements = item.Children.Where(itm => itm.IsDerived(Sitecore.Foundation.Assets.Templates.AssetFolder.ID)).FirstOrDefault();
                if (metaElements != null)
                {
                    var meta = metaElements.Children.Where(itm => itm.IsDerived(Templates.MetaBase.ID)).ToList();
                    if (meta != null)
                    {
                        meta.ForEach(itm =>
                        {
                            if (itm != null)
                            {
                                if (itm.IsDerived(Templates.Meta.ID))
                                {
                                    MetadataRepository.Current.Add(new Models.Meta(
                                        itm.Fields[Templates.MetaBase.Fields.Name].Value,
                                        itm.Fields[Templates.Meta.Fields.Content].Value));
                                }
                                if (itm.IsDerived(Templates.MetaVersioned.ID))
                                {
                                    MetadataRepository.Current.Add(new Models.Meta(
                                        itm.Fields[Templates.MetaBase.Fields.Name].Value,
                                        itm.Fields[Templates.MetaVersioned.Fields.Content].Value));
                                }
                            }
                        });
                    }
                }
            }
        }
    }
}