using Little_Choice_Based_RPG.Resources.Components.SystemComponents;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Components.ComponentAccessors
{
    internal static class PrivateInteractionsAccessor
    {
        extension(IPropertyContainer container)
        {
            /// <summary> Properties used exclusively by the PrivateInteractions System. This allows private actions about an entity to be assigned to their user. </summary>
            public PrivateInteractionsComponent PrivateInteractions => container.Properties.Get<PrivateInteractionsComponent>();
        }
    }
}
