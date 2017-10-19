using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Feature.PushNotifications.Models;
using Sitecore.Configuration;

namespace Feature.PushNotifications.Providers
{
    public class ConfigKeyProvider : IPushKeySetProvider
    {

        public string PublicKey
        {
            get { return GetKeys().PublicKey; }
        }

        public string PrivateKey
        {
            get { return GetKeys().PrivateKey; }
        }


        public PushKeySetModel GetKeys()
        {
            return new PushKeySetModel { 
                PublicKey = Settings.GetSetting("PushNotifications.PublicKey", null),
                PrivateKey = Settings.GetSetting("PushNotifications.PrivateKey", null)
            };
        }
    }
}