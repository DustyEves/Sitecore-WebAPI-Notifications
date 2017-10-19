using Feature.PushNotifications.Contracts;
using Feature.PushNotifications.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPush;

namespace Feature.PushNotifications.Processors
{
    public class NotificationSender
    {
       

        public void SendNotification( IContactNotificationSubscription subscription, INotification notification)
        {

            IPushKeySetProvider keyProvider = Factory.KeySetProvider();

            var pushSubscription = new PushSubscription(subscription.Endpoint, subscription.P256dh, subscription.AuthorizationToken);
            
            var vapidDetails = new VapidDetails(notification.Subject, keyProvider.PublicKey, keyProvider.PrivateKey);

            var webPushClient = new WebPushClient();
            try
            {
                string message = string.Format("{{ \"body\":\"{0}\", \"title\":\"{1}\", \"icon\":\"{2}\", \"badge\":\"{3}\" }}", 
                    notification.Body, 
                    notification.Title, 
                    notification.Icon,
                    notification.Badge
                    );
                webPushClient.SendNotificationAsync(pushSubscription, message, vapidDetails);
                //webPushClient.SendNotification(subscription, "payload", gcmAPIKey);
            }
            catch (WebPushException exception)
            {
                Console.WriteLine("Http STATUS code" + exception.StatusCode);
            }

        }
    }
}