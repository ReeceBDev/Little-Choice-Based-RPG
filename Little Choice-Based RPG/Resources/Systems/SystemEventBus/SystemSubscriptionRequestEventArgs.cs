using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems.SystemEventBus
{
    public record struct SystemSubscriptionRequestEventArgs(PropertyContainer targetPropertyContainer, string systemReferenceName);
}
