using Sitecore.Analytics.Model.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.PushNotifications.Contracts
{
    /// <summary>
    /// Contact Notification Subscription
    /// </summary>
    public interface IContactNotificationSubscription  : IFacet
    {
        /// <summary>
        /// Endpoint Address for notification
        /// </summary>
        string Endpoint { get; set; }

        /// <summary>
        /// Public key used to generate subscription
        /// </summary>
        string P256dh { get; set; }

        /// <summary>
        /// Authorization Token
        /// </summary>
        string AuthorizationToken { get; set; }


        /// <summary>
        /// Expiration if the permission expires
        /// </summary>
        DateTime Expiration { get; set; }

    }
    public interface INotificationSubscriptionCollection : IFacet
    {
        IElementCollection<IContactNotificationSubscription> Subscriptions { get; }
    }


}
