using Sitecore.Feature.Metadata.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Sitecore.Feature.Metadata.Services
{
    public class RenderMetaElementsService
    {
        private static RenderMetaElementsService m_current;
        public static RenderMetaElementsService Current => m_current ?? (m_current = new RenderMetaElementsService());

        public HtmlString RenderMetaElements()
        {
            var stringBuilder = new StringBuilder();
            var metaElements = MetadataRepository.Current.Items;
            foreach (var meta in metaElements)
            {
                //stringBuilder.Append("<meta name=\"{0}\" content=\"{1}\" />", meta.Name, meta.Content ?? "");
                stringBuilder.AppendFormat("<meta name=\"{0}\" content=\"{1}\" />", meta.Name, meta.Content ?? string.Empty).AppendLine();
            }

            return new HtmlString(stringBuilder.ToString());
        }
    }
}