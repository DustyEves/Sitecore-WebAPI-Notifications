using Feature.PushNotifications.Contracts;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Resources.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Feature.PushNotifications.Models
{
    public class Notification : INotification
    {

        private readonly ID NOTIFICATION_TEMPLATE_ID = new ID("{8A0D9F5E-D5E4-427F-9AA5-4DF2DC860C5D}");
        internal Notification(string _title, string _subject, string _body, string _icon, string _badge)
        {
            Title = _title;
            Body = _body;
            Icon = _icon;
            Badge = _badge;
            Subject = _subject;
        }

        public Notification(Item _notificationItem)
        {
            Assert.AreEqual(_notificationItem.TemplateID.ToString(), NOTIFICATION_TEMPLATE_ID.ToString(), "Invalid Notification Item");

            Title = _notificationItem["Title"];
            Body = _notificationItem["Body"];

            ImageField _iconField = (ImageField)_notificationItem.Fields["Icon"];
            ImageField _badgeField = (ImageField)_notificationItem.Fields["Badge"];

            LinkField _linkField = (LinkField)_notificationItem.Fields["Subject"];
            

            var options = new MediaUrlOptions { AlwaysIncludeServerUrl = true };

            Icon = MediaManager.GetMediaUrl(_iconField.MediaItem, options);
            Badge = MediaManager.GetMediaUrl(_badgeField.MediaItem, options);

            Subject = _linkField.Url;
        }

        public string Badge
        { get; private set; }

        public string Body
        { get; private set; }

        public string Icon
        { get; private set; }

        public string Title
        { get; private set; }

        public string Subject { get; private set; }
    }
}