using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Feature.PushNotifications.Models
{
    public class PushKeySetModel
    {
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }
}