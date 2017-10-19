using Feature.PushNotifications.Contracts;
using Feature.PushNotifications.Models;
using Feature.PushNotifications.Processors;
using Sitecore.Analytics;
using Sitecore.Analytics.Automation;
using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Feature.PushNotifications.Actions
{
    public class SendNotificationAction : IAutomationAction
    {
        public AutomationActionResult Execute(AutomationActionContext context)
        {
            IContactNotificationSubscription profileEntry = Tracker.Current.Contact.GetFacet<IContactNotificationSubscription>("PushNotifications");
            


            Item _notificationOne = Sitecore.Configuration.Factory.GetDatabase("web").GetItem(new ID("{19875827-6AB9-4539-9B90-00FC5475BFAC}"));
            INotification _notification = new Notification(_notificationOne);

            new NotificationSender().SendNotification(profileEntry, _notification);


            return AutomationActionResult.Continue;
        }
    }
}