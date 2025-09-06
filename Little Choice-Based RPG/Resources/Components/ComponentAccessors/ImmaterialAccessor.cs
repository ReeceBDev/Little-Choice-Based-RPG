using Little_Choice_Based_RPG.Resources.Components.CommonComponents;
using Little_Choice_Based_RPG.Types.PropertySystem.Components;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using Little_Choice_Based_RPG.Types.PropertySystem.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Components.ComponentAccessors
{
    internal static class ImmaterialAccessor
    {
        extension(IPropertyContainer container)
        {
            /// <summary> Used for marking an entity as immaterial and therefore it should have no physical basis in the game-world, i.e. Positional markers, transitions. </summary>
            public ImmaterialComponent Immaterial => container.Properties.Get<ImmaterialComponent>();
        }
    }
}
