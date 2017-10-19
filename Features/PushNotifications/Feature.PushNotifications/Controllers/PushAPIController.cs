
using Feature.PushNotifications.Contracts;
using Feature.PushNotifications.Models;
using Feature.PushNotifications.Processors;
using Feature.PushNotifications.Providers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitecore.Analytics;
using Sitecore.Analytics.Automation.Data;
using Sitecore.Analytics.Automation.MarketingAutomation;
using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Feature.PushNotifications.Controllers
{
    public class PushAPIController : Controller
    {

        private string _NotificationScriptMarkup = @"
<script type='text/javascript' src='/Scripts/notification_permission.js'></script>
<input type='hidden' id='PushAPIPublicKey' value='|PushAPIPublicKey|' /> 
<input type='hidden' id='EngagementPlanId' value='|EngagementPlanId|' />
<input type='hidden' id='EngagementPlanState' value='|EngagementStateId|' />
<input type='hidden' id='GoalTriggerId' value='|GoalTriggerId|' />
";

        public ActionResult GetScriptAndKey()
        {
            StringBuilder _script = new StringBuilder(_NotificationScriptMarkup);
            _script.Replace("|PushAPIPublicKey|", KeyProvider.PublicKey);

            var context = Sitecore.Mvc.Presentation.RenderingContext.CurrentOrNull;
            if (context == null)
                throw new InvalidOperationException("Cannot Invoke method without Sitecore Rendering Context");

            _script.Replace("|EngagementPlanId|", context.Rendering.Parameters["EngagementPlanId"]);
            _script.Replace("|EngagementStateId|", context.Rendering.Parameters["EngagementStateId"]);
            _script.Replace("|GoalTriggerId|", context.Rendering.Parameters["GoalTriggerId"]);

            return Content(_script.ToString());
        }

        [HttpPost]
        public ActionResult GetPublicKey()
        {
            return Json(new { publicKey = KeyProvider.PublicKey });
        }

        

        public ActionResult RecordSubscription(string _pushAuthorization)
        {
            if (!Tracker.IsActive)
                Tracker.StartTracking();

            dynamic _subscriptionObject = JsonConvert.DeserializeObject(_pushAuthorization);
            string _endpoint = (string)_subscriptionObject.endpoint;
            
            string _key = (string)_subscriptionObject.keys.p256dh;
            string _auth = (string)_subscriptionObject.keys.auth;
            string _expiration = (string)_subscriptionObject.expirationType;

            
            var profileEntry = Tracker.Current.Contact.GetFacet<IContactNotificationSubscription>("PushNotifications");
            
            profileEntry.P256dh = _key;
            profileEntry.AuthorizationToken = _auth;
            profileEntry.Endpoint = _endpoint;

            //if (! string.IsNullOrWhiteSpace(_engagementPlan))
            EnrollInEngagementPlan(new ID(ENGAGEMENT_PLAN_ID), new ID(ASKED_FOR_UPDATES));
            return Content("");
        }

        private const string ASKED_FOR_UPDATES = "{5916D3D8-C9C5-4EC6-B0FE-6205F108A39C}";
        private const string ENGAGEMENT_PLAN_ID = "{EB4187F3-DC59-4DEF-BBFF-8E6DBD3669D9}";
        private void EnrollInEngagementPlan(ID _plan, ID _state)
        {
            Tracker.Current.Session.Identify(string.Format("extranet\\dusty{0}@dusty.com", DateTime.Now.Hour));
            AutomationStateManager manager = Tracker.Current.Session.CreateAutomationStateManager();
            manager.EnrollInEngagementPlan(_plan, _state);
        }
        public ActionResult SendSubscriptionMessage()
        {
            if (!Tracker.IsActive)
                Tracker.StartTracking();
            

            IContactNotificationSubscription profileEntry = Tracker.Current.Contact.GetFacet<IContactNotificationSubscription>("PushNotifications");
            if (profileEntry == null || string.IsNullOrEmpty(profileEntry.Endpoint))
                return Content("<!-- No notification recorded on profile -->");


            Item _notificationOne = Sitecore.Context.Database.GetItem(new ID("{19875827-6AB9-4539-9B90-00FC5475BFAC}"));
            INotification _notification = new Notification(_notificationOne);

            new NotificationSender().SendNotification(profileEntry, _notification);

            return Content("");
        }

        private IPushKeySetProvider KeyProvider
        {
            get { return Factory.KeySetProvider(); }
        }
    }
}