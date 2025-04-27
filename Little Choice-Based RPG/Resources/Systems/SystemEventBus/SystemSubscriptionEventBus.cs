using Little_Choice_Based_RPG.Resources.Systems.DamageSystems;
using Little_Choice_Based_RPG.Types.EntityProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems.SystemEventBus
{
    public static class SystemSubscriptionEventBus
    {
        public static event EventHandler<SystemSubscriptionRequestEventArgs> SystemSubcriptionRequest;
        public static event EventHandler<SystemSubscriptionRequestEventArgs> SystemUnsubscriptionRequest;

        public static void Subscribe(PropertyContainer targetObject, string systemReferenceName) =>
            SystemSubcriptionRequest?.Invoke(targetObject, new SystemSubscriptionRequestEventArgs(targetObject, systemReferenceName));

        public static void Unsubscribe(PropertyContainer targetObject, string systemReferenceName) =>
            SystemUnsubscriptionRequest?.Invoke(targetObject, new SystemSubscriptionRequestEventArgs(targetObject, systemReferenceName));
    }
}
