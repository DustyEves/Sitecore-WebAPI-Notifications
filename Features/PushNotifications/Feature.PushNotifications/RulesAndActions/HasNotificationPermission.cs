using Feature.PushNotifications.Contracts;
using Sitecore.Analytics;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Feature.PushNotifications.Rules
{
    public class HasNotificationPermission<T> : WhenCondition<T> where T: RuleContext
    {
        protected override bool Execute(T ruleContext)
        {
            Assert.ArgumentNotNull((object)ruleContext, "ruleContext");
            Assert.ArgumentNotNull((object)Tracker.Current, "Tracker.Current");

            if (Tracker.Current.Contact == null)
                return false;

            var profileEntry = Tracker.Current.Contact.GetFacet<IContactNotificationSubscription>("PushNotifications");

            bool answer = ! profileEntry.IsEmpty;
            return answer;
        }
    }
}