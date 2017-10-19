
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

            string planId = context.Rendering.Parameters["EngagementPlanId"];
            string stateId = context.Rendering.Parameters["EngagementStateId"];
            string goalId = context.Rendering.Parameters["GoalTriggerId"];


            if (IsEngagementPlan(planId) && IsStateOfId(planId, stateId))
            {
                _script.Replace("|EngagementPlanId|", planId);
                _script.Replace("|EngagementStateId|", stateId);
            }
            else
            { 
                _script.Replace("|EngagementPlanId|", string.Empty);
                _script.Replace("|EngagementStateId|", string.Empty);
            }
            if (IsGoal(goalId))
                _script.Replace("|GoalTriggerId|", goalId);
            else
                _script.Replace("|GoalTriggerId|", string.Empty);

            return Content(_script.ToString());
        }

        #region Script Rendering Validation Methods

        private const string ENGAGEMENT_STATE_ID = "{8CE2707A-3742-4A89-933B-065E5BE02BC9}";
        private const string ENGAGEMENT_PLAN_ID = "{6E5B63D6-2401-4A52-8B4D-CFEF5E4E9752}";

        private bool IsEngagementPlan(string _id)
        {
            if (string.IsNullOrWhiteSpace(_id))
                return false;

            var item = Sitecore.Context.Database.GetItem(new ID(_id));

            ///Engagement plan Template Id
            return item.TemplateID.ToString() == ENGAGEMENT_PLAN_ID;
        }

        private bool IsStateOfId(string _planId, string _stateId)
        {
            if (string.IsNullOrWhiteSpace(_stateId))
                return false;
            var stateItem = Sitecore.Context.Database.GetItem(new ID(_stateId));
            if (stateItem.TemplateID.ToString() != ENGAGEMENT_STATE_ID)
                return false;
            var planItem = Sitecore.Context.Database.GetItem(new ID(_planId));

            return stateItem.ParentID == planItem.ID;


        }
        private bool IsGoal(string _id)
        {
            return false;
            throw new NotImplementedException();
        }

        #endregion

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
            //EnrollInEngagementPlan(new ID(ENGAGEMENT_PLAN_ID), new ID(ASKED_FOR_UPDATES));
            return Content("");
        }

        
        private void EnrollInEngagementPlan(ID _plan, ID _state)
        {
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