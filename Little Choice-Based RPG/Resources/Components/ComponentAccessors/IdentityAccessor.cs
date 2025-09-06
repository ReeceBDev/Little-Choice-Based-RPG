using Little_Choice_Based_RPG.Resources.Components.CommonComponents;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Components.ComponentAccessors
{
    internal static class IdentityAccessor
    {
        
        extension(IPropertyContainer container)
        {
            /// <summary> Properties used in identifying entities, such as unique identification or standard flavour text like a human-readable name. </summary>
            public IdentityComponent Identity => container.Properties.Get<IdentityComponent>() ?? container.Properties.AddComponent(new IdentityComponent());
        }
    }
}