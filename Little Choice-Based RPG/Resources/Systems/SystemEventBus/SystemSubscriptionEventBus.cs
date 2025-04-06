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
        public event EventHandler<SystemSubscriptionRequestEventArgs> SystemSubcriptionRequest;

        public void Subscribe(object targetObject, string systemReferenceName)
        {
            SystemSubcriptionRequest(this, new SystemSubscriptionRequestEventArgs(targetObject, systemReferenceName));
        }
    }
}
