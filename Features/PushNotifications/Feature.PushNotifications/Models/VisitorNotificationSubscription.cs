using Feature.PushNotifications.Contracts;
using Sitecore.Analytics.Model.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Feature.PushNotifications.Models
{
    [Serializable]
    public class VisitorNotificationSubscription : Facet, IContactNotificationSubscription
    {
        public VisitorNotificationSubscription()
        {
            EnsureAttribute<string>("AuthorizationToken");
            EnsureAttribute<string>("Endpoint");
            EnsureAttribute<DateTime>("Expiration");
            EnsureAttribute<string>("PublicKey");

        }
        public string AuthorizationToken
        {
           get { return base.GetAttribute<string>("AuthorizationToken"); }
           set { base.SetAttribute("AuthorizationToken", value); }
        }

        public string Endpoint
        {
            get { return base.GetAttribute<string>("Endpoint"); }
            set { base.SetAttribute("Endpoint", value); }
        }

        public DateTime Expiration
        {
            get { return base.GetAttribute<DateTime>("Expiration"); }
            set { base.SetAttribute("Expiration", value); }
        }

        public string P256dh
        {
            get { return base.GetAttribute<string>("PublicKey"); }
            set { base.SetAttribute("PublicKey", value); }
        }
    }
    //public class NotificationSubscriptionCollection : Facet, INotificationSubscriptionCollection
    //{
    //    public NotificationSubscriptionCollection()
    //    {
    //        EnsureCollection<IContactNotificationSubscription>("PushNotifications");
    //    }
    //    public IElementCollection<IContactNotificationSubscription> Subscriptions
    //    {
    //        get
    //        {
    //            return GetCollection<IContactNotificationSubscription>("PushNotifications");
    //        }
    //    }
    //}


}