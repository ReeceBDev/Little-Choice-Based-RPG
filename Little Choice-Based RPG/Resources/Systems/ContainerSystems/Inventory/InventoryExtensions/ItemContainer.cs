using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.PropertyExtensions;
using Little_Choice_Based_RPG.Types.PropertyExtensions.PropertyExtensionEventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory.InventoryExtensions
{
    public class ItemContainer : IPropertyExtension
    {
        public void Add(GameObject target)
        {
            Inventory.Add(target);
            PropertyExtensionChanged?.Invoke(this, new PropertyExtensionChangedArgs("ItemContainer.Added", target));
        }

        public void Remove(GameObject target)
        {
            Inventory.Remove(target);
            PropertyExtensionChanged?.Invoke(this, new PropertyExtensionChangedArgs("ItemContainer.Removed", target));
        }

        public void NewLocalUserMessage(string userMessage)
        {
            BroadcastLocalUserMessage?.Invoke(null, userMessage + "§f"); //§f Resets any custom colours back to white at the end of the statement.
        }

        public static void NewGlobalUserMessage(string userMessage)
        {
            BroadcastGlobalUserMessage?.Invoke(null, userMessage + "§f"); //§f Resets any custom colours back to white at the end of the statement.
        }

        public static event EventHandler<string> BroadcastGlobalUserMessage; //Broadcasts a message to the players within any and all containers, yes, all.
        public event EventHandler<string> BroadcastLocalUserMessage; //Broadcasts a message to the players within this container.
        public event EventHandler<PropertyExtensionChangedArgs> PropertyExtensionChanged;
        public string UniqueIdentifier { get; init; } = "ItemContainer";
        public List<GameObject> Inventory { get; private set; } = new List<GameObject>();
    }
}
