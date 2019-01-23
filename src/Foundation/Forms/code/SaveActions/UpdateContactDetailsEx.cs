using Sitecore.WFFM.Abstractions.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Sitecore.WFFM.Abstractions.Actions;
using Sitecore.Diagnostics;
using Sitecore.WFFM.Abstractions.Analytics;

namespace Sitecore.Foundation.Forms.SaveActions
{
    public class UpdateContactDetailsEx : Sitecore.WFFM.Actions.SaveActions.UpdateContactDetails
    {
        private readonly IAnalyticsTracker m_analyticsTracker;
        private readonly IAuthentificationManager m_authentificationManager;
        private readonly ILogger m_logger;
        private readonly IFacetFactory m_facetFactory;

        public UpdateContactDetailsEx(IAnalyticsTracker analyticsTracker, IAuthentificationManager authentificationManager, ILogger logger, IFacetFactory facetFactory) : base(analyticsTracker, authentificationManager, logger,facetFactory)
        {
            m_analyticsTracker = analyticsTracker;
            m_authentificationManager = authentificationManager;
            m_logger = logger;
            m_facetFactory = facetFactory;
        }

        protected override void UpdateContact(AdaptedResultList fields)
        {
            Assert.ArgumentNotNull(fields, "adaptedFields");
            Assert.IsNotNullOrEmpty(Mapping, "Empty mapping xml.");
            Assert.IsNotNull(m_analyticsTracker.CurrentContact, "Tracker.Current.Contact");
            //if (!this.authentificationManager.IsActiveUserAuthenticated)
            //{
            //    this.logger.Warn("[UPDATE CONTACT DETAILS Save action] User is not authenticated to edit contact details.", this);
            //    return;
            //}
            IEnumerable<FacetNode> enumerable = this.ParseMapping(Mapping, fields);
            IContactFacetFactory contactFacetFactory = m_facetFactory.GetContactFacetFactory();
            foreach (FacetNode current in enumerable)
            {
                contactFacetFactory.SetFacetValue(m_analyticsTracker.CurrentContact, current.Key, current.Path, current.Value, true);
            }
        }
    }
}