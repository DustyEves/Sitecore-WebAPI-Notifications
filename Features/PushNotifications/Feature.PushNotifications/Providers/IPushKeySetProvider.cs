using Feature.PushNotifications.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.PushNotifications.Providers
{
    public interface IPushKeySetProvider
    {
        PushKeySetModel GetKeys();

        /// <summary>
        /// Push API Public Key
        /// </summary>
        string PublicKey { get; }

        /// <summary>
        /// Push API Private Key
        /// </summary>
        string PrivateKey { get; }

    }
}
