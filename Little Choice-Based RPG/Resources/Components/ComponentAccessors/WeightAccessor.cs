using Little_Choice_Based_RPG.Resources.Components.CommonComponents;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Components.ComponentAccessors
{
    internal static class WeightAccessor
    {
        extension(IPropertyContainer container)
        {
            /// <summary> Assign entities with weight. It may be reasonable to assume that weight is synonymous with volume, and may provide an avenue of constraint. </summary>
            public WeightComponent Weight => container.Properties.Get<WeightComponent>();
        }
    }
}
