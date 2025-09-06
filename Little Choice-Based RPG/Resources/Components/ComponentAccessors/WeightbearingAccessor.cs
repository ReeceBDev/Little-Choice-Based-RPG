using Little_Choice_Based_RPG.Resources.Components.SystemComponents;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Components.ComponentAccessors
{
    internal static class WeightbearingAccessor
    {
        extension(IPropertyContainer container)
        {
            /// <summary> Properties used exclusively by the WeightbearingSystem. This enables entities to carry and bear the weight of other entities. </summary>
            public WeightbearingComponent Weightbearing => container.Properties.Get<WeightbearingComponent>();
        }
    }
}
