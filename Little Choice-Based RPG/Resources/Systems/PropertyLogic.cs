using Little_Choice_Based_RPG.Resources.Systems.SystemEventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems
{
    public class PropertyLogic
    {
        private SystemSubscriptionEventBus currentSystemSubscriptionEventBus;
        public PropertyLogic(SystemSubscriptionEventBus setSystemSubscriptionEventBus)
        {
            currentSystemSubscriptionEventBus = setSystemSubscriptionEventBus;
        }
    }
}
