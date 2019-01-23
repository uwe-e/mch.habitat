using Sitecore.Analytics;
using Sitecore.Analytics.Core;
using Sitecore.Analytics.Model;
using Sitecore.Analytics.Rules.Conditions;
using Sitecore.Analytics.Tracking;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCH.Foundation.Analytics.Rules.Conditions
{
    public class GoalWasTriggeredDuringPastOrCurrentInteraction<T> : HasEventOccurredCondition<T> where T : RuleContext
    {
        private System.Guid? m_goalGuid;
        private bool m_goalGuidIsInitialized;
        public string GoalId
        {
            get;
            set;
        }

        private System.Guid? GoalGuid
        {
            get
            {
                if (m_goalGuidIsInitialized)
                {
                    return m_goalGuid;
                }
                try
                {
                    m_goalGuid = new System.Guid?(new System.Guid(GoalId));
                }
                catch
                {
                    Log.Warn(string.Format("Could not convert value to guid: {0}", GoalId), base.GetType());
                }
                m_goalGuidIsInitialized = true;
                return m_goalGuid;
            }
        }

        protected override bool Execute(T ruleContext)
        {
            Assert.ArgumentNotNull(ruleContext, "ruleContext");
            Assert.IsNotNull(Tracker.Current, "Tracker.Current is not initialized");
            Assert.IsNotNull(Tracker.Current.Session, "Tracker.Current.Session is not initialized");
            Assert.IsNotNull(Tracker.Current.Session.Interaction, "Tracker.Current.Session.Interaction is not initialized");
            if (!GoalGuid.HasValue)
            {
                return false;
            }
            if (HasEventOccurredInInteraction(Tracker.Current.Session.Interaction))
            {
                return true;
            }
            Assert.IsNotNull(Tracker.Current.Contact, "Tracker.Current.Contact is not initialized");
            KeyBehaviorCache keyBehaviorCache = Tracker.Current.Contact.GetKeyBehaviorCache();
            return FilterKeyBehaviorCacheEntries(keyBehaviorCache).Any<KeyBehaviorCacheEntry>((KeyBehaviorCacheEntry entry) => {
                Guid id = entry.Id;
                Guid? goalGuid = GoalGuid;
                if (!goalGuid.HasValue)
                {
                    return false;
                }
                return id == goalGuid.GetValueOrDefault();
            });
        }

        protected override System.Collections.Generic.IEnumerable<KeyBehaviorCacheEntry> GetKeyBehaviorCacheEntries(KeyBehaviorCache keyBehaviorCache)
        {
            Assert.ArgumentNotNull(keyBehaviorCache, "keyBehaviorCache");
            return keyBehaviorCache.Goals;
        }

        protected override bool HasEventOccurredInInteraction(IInteractionData interaction)
        {
            Assert.ArgumentNotNull(interaction, "interaction");
            Assert.IsNotNull(interaction.Pages, "interaction.Pages is not initialized.");
            return interaction.Pages.SelectMany(
                (Page page) => page.PageEvents)
                .Any((PageEventData pageEvent) => pageEvent.IsGoal && pageEvent.PageEventDefinitionId == GoalGuid);
        }

    }
}