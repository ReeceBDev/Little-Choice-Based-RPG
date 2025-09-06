using Little_Choice_Based_RPG.Resources.Components.CommonComponents;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Components.ComponentAccessors
{
    internal static class HealthPoolAccessor
    {
        extension(IPropertyContainer container)
        {
            /// <summary> Properties which represent anything with a tangible integrity or health pool, such as living creatures. </summary>
            public HealthPoolComponent Healthpool => container.Properties.Get<HealthPoolComponent>();
        }
    }
}
