using Little_Choice_Based_RPG.Resources.Systems.Damage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems.SystemEventBus
{
    public struct SystemSubscriptionRequestEventArgs(object targetObject, string systemReferenceName);
    public class SystemSubscriptionEventBus
    {
        private static readonly SystemSubscriptionEventBus singletonInstance = new SystemSubscriptionEventBus();

        public event EventHandler<SystemSubscriptionRequestEventArgs> SystemSubcriptionRequest;

        public void Subscribe(object targetObject, string systemReferenceName) =>
            OnSystemSubscriptionRequest(targetObject, systemReferenceName);

        private protected void OnSystemSubscriptionRequest(object targetObject, string systemReferenceName) =>
            SystemSubcriptionRequest?.Invoke(this, new SystemSubscriptionRequestEventArgs(targetObject, systemReferenceName));

        public static SystemSubscriptionEventBus Instance
        {
            get
            {
                return singletonInstance;
            }
        }
    }
}
