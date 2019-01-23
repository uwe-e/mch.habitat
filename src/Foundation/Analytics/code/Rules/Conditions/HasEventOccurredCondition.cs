using Sitecore.Analytics.Tracking;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCH.Foundation.Analytics.Rules.Conditions
{
    public abstract class HasEventOccurredCondition<T> : WhenCondition<T> where T : RuleContext
    {
        protected virtual System.Collections.Generic.IEnumerable<KeyBehaviorCacheEntry> FilterKeyBehaviorCacheEntries(KeyBehaviorCache keyBehaviorCache)
        {
            Assert.ArgumentNotNull(keyBehaviorCache, "keyBehaviorCache");
            System.Collections.Generic.IEnumerable<KeyBehaviorCacheEntry> enumerable = keyBehaviorCache.Campaigns
                .Concat(keyBehaviorCache.Channels)
                .Concat(keyBehaviorCache.CustomValues)
                .Concat(keyBehaviorCache.Goals)
                .Concat(keyBehaviorCache.Outcomes)
                .Concat(keyBehaviorCache.PageEvents)
                .Concat(keyBehaviorCache.Venues);
            return Assert.ResultNotNull<System.Collections.Generic.IEnumerable<KeyBehaviorCacheEntry>>(
                this.GetKeyBehaviorCacheEntries(keyBehaviorCache).Intersect(enumerable, new KeyBehaviorCacheEntry.KeyBehaviorCacheEntryEqualityComparer()));
        }

        protected abstract System.Collections.Generic.IEnumerable<KeyBehaviorCacheEntry> GetKeyBehaviorCacheEntries(KeyBehaviorCache keyBehaviorCache);

        protected abstract bool HasEventOccurredInInteraction(IInteractionData interaction);
    }
}