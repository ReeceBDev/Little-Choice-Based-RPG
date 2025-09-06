using Little_Choice_Based_RPG.Resources.Components.SystemComponents;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Components.ComponentAccessors
{
    internal static class FlammabilityAccessor
    {
        extension(IPropertyContainer container)
        {
            /// <summary> Represents specific properties used by FlammabilitySystem, which allows entities to burn and ignite nearby flammable entities while burning. </summary>
            public FlammabilityComponent Flammability => container.Properties.Get<FlammabilityComponent>();
        }
    }
}
