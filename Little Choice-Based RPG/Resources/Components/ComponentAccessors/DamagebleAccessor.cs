using Little_Choice_Based_RPG.Resources.Components.CommonComponents;
using Little_Choice_Based_RPG.Resources.Components.EntityComponents;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Components.ComponentAccessors
{
    internal static class DamagebleAccessor
    {
        extension(IPropertyContainer container)
        {
            /// <summary> Used to indicate the state of an items integrity outside of death, such as being repairable or entirely decimated. </summary>
            public DamagableComponent Damagable => container.Properties.Get<DamagableComponent>();
        }
    }
}
