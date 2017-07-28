namespace Sitecore.Feature.Metadata.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Sitecore.Data.Fields;
    using Sitecore.Data.Items;
    using Sitecore.Feature.Metadata.Infrastructure.Pipelines.GetPageMetadata;
    using Sitecore.Feature.Metadata.Models;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.Pipelines;
    using Sitecore.Web.UI.WebControls;

    [Service]
    public class MetadataRepository
    {
        [ThreadStatic]
        private static MetadataRepository m_current;

        public static MetadataRepository Current => m_current ?? (m_current = new MetadataRepository());

        private readonly List<Meta> _items = new List<Meta>();
        internal List<Meta> Items => _items;

        public void Clear()
        {
            this._items.Clear();
        }

        public IMetadata Get(Item item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var args = new GetPageMetadataArgs(new MetadataViewModel(), item);
            CorePipeline.Run("metadata.getPageMetadata", args);

            return args.Metadata;
        }

        public Meta Add(Meta meta)
        {
            if (meta == null)
            {
                throw new ArgumentNullException(nameof(meta));
            }

            if (meta.AddOnceToken != null)
            {
                if (Items.Any(x => x.AddOnceToken != null && x.AddOnceToken == meta.AddOnceToken))
                {
                    return null;
                }
            }
            if (meta.Content != null)
            {
                if (Items.Any(x => x.Content != null && x.Content == meta.Content))
                {
                    return null;
                }
            }

            // Passed the checks, add the requirement.
            Items.Add(meta);
            return meta;
        }
    }
}