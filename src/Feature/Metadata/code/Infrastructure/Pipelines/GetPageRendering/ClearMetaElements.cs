using Sitecore.Feature.Metadata.Repositories;
using Sitecore.Mvc.Pipelines.Response.GetPageRendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Metadata.Infrastructure.Pipelines.GetPageRendering
{
    public class ClearMetaElements : GetPageRenderingProcessor
    {
        public override void Process(GetPageRenderingArgs args)
        {
            MetadataRepository.Current.Clear();
        }
    }
}