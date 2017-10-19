using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Feature.PushNotifications.Models;

namespace Feature.PushNotifications.Providers
{
    public class FixedKeySetProvider : IPushKeySetProvider
    {
        string publicKey = "PUBLIC KEY HERE";
        string privateKey = "PRIVATE KEY HERE";

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
            return new PushKeySetModel { PublicKey = publicKey, PrivateKey = privateKey };
        }

    }
}