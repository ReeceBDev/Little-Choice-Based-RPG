using Little_Choice_Based_RPG.Resources.Components.SystemComponents;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Components.ComponentAccessors
{
    internal static class PlayerAccessor
    {
        extension(IPropertyContainer container)
        {
            /// <summary> Properties related to or necessary for a playable character. Primarly used by the PlayerSystem. </summary>
            public PlayerComponent Player => container.Properties.Get<PlayerComponent>();
        }
    }
}
