using Little_Choice_Based_RPG.Resources.Components.EntityComponents;
using Little_Choice_Based_RPG.Resources.Components.SystemComponents;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Components.ComponentAccessors
{
    internal static class InventoryAccessor
    {
        extension(IPropertyContainer container)
        {
            /// <summary> Properties used exclusively by the InventorySystem. This facilitates the inventory functionality to players, rooms and objects such as chests. </summary>
            public InventoryComponent Inventory => container.Properties.Get<InventoryComponent>();
        }
    }
}
