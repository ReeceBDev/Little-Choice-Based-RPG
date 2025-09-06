using Little_Choice_Based_RPG.Resources.Components.SystemComponents;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Components.ComponentAccessors
{
    internal static class PublicInteractionsAccessor
    {
        extension(IPropertyContainer container)
        {
            /// <summary> Properties used exclusively by the PublicInteractions System. This allows publicly accessible actions to be tied to an entity. </summary>
            public PublicInteractionsComponent PublicInteractions => container.Properties.Get<PublicInteractionsComponent>();
        }
    }
}
