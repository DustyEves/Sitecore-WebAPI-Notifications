using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Feature.PushNotifications.Providers
{
    public static class ScriptProvider
    {
        public static void Register(BundleCollection _collection )
        {
            _collection.Add(new ScriptBundle("notifications").Include("/Scripts/notification_permission.js"));
        }
    }
}