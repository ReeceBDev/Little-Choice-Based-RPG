using Little_Choice_Based_RPG.Resources.Components.SystemComponents;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Components.ComponentAccessors
{
    internal static class GearAccessor
    {
        extension(IPropertyContainer container)
        {
            /// <summary> Represents specific properties used by the GearSystem. This enables entities to carry and utilise gear, such as augmentation or defensive armour. </summary>
            public GearComponent Gear => container.Properties.Get<GearComponent>();
        }
    }
}
