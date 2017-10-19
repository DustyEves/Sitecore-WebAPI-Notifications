using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Windsor.Installer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Feature.PushNotifications.Providers
{
    public class Factory
    {
        private static object syncRoot = new object();
        private static WindsorContainer container = new WindsorContainer();


        public static void Install(IRegistration[] _registrations)
        {
            lock(syncRoot)
                container.Register(_registrations);
        }

        public static void Install(string configFilePath)
        {
            lock (syncRoot)
                container.Install(Configuration.FromXmlFile(configFilePath));
        }

        private static void RegisterDefault()
        {
            lock (syncRoot)
                container.Register(Component.For<IPushKeySetProvider>().ImplementedBy<ConfigKeyProvider>());
        }




        internal static IPushKeySetProvider KeySetProvider()
        {
            if (!container.Kernel.HasComponent(typeof(IPushKeySetProvider)))
                RegisterDefault();

            return container.Resolve<IPushKeySetProvider>();
        }
    }
}