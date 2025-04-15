using Little_Choice_Based_RPG.Resources.Systems.DamageSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems.SystemEventBus
{
    public record struct SystemSubscriptionRequestEventArgs(PropertyContainer targetPropertyContainer, string systemReferenceName);
    public static class SystemSubscriptionEventBus
    {

        public static event EventHandler<SystemSubscriptionRequestEventArgs> SystemSubcriptionRequest;

        public static void Subscribe(PropertyContainer targetObject, string systemReferenceName) =>
            SystemSubcriptionRequest?.Invoke(targetObject, new SystemSubscriptionRequestEventArgs(targetObject, systemReferenceName));
    }
}
