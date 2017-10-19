using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.PushNotifications.Contracts
{
    public interface INotification
    {
        /// <summary>
        /// Notification Title
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Notification Body, size limited on certain devices
        /// </summary>
        string Body { get; }

        /// <summary>
        /// URL For notification icon
        /// </summary>
        string Icon { get; }

        /// <summary>
        /// URL for black and white icon image
        /// </summary>
        string Badge { get; }

        /// <summary>
        /// URL or MailTo Link
        /// </summary>
        string Subject { get; }
    }
}
