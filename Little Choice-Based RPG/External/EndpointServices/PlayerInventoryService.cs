using Little_Choice_Based_RPG.External.Types.Services;
using Little_Choice_Based_RPG.Managers.PlayerControl;
using Little_Choice_Based_RPG.Managers.Server;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory.InventoryExtensions;
using Little_Choice_Based_RPG.Resources.Systems.InformationalSystems.Descriptor;

namespace Little_Choice_Based_RPG.External.EndpointServices
{
    public sealed class PlayerInventoryService
    {

        PlayerController cachedPlayerController;

        public PlayerInventoryService(ulong authenticationToken)
        {
            PlayerController cachedPlayer = SessionManager.GetPlayerController(authenticationToken);
        }

        public List<uint> GetPlayerInventoryIDs() =>
            ((ItemContainer)cachedPlayerController.CurrentPlayer.Extensions.Get("ItemContainer"))
            .Inventory
            .ConvertAll(i => (uint)i.Properties.GetPropertyValue("ID"));

        public List<ItemDisplayData> GetPlayerInventoryDisplayData()
        {
            List<ItemDisplayData> inventoryDisplayData = new();

            List<GameObject> items = ((ItemContainer)cachedPlayerController.CurrentPlayer
                .Extensions.Get("ItemContainer"))
                .Inventory
                .ToList();

            foreach (var item in items)
            {
                uint itemID = (uint)item.Properties.GetPropertyValue("ID");
                string itemName = (string)item.Properties.GetPropertyValue("Name");
                decimal itemWeight = (decimal)item.Properties.GetPropertyValue("WeightInKG");

                inventoryDisplayData.Add(new ItemDisplayData(itemID, itemName, itemWeight));
            }

            return inventoryDisplayData;
        }

        public List<ItemDetails> GetItemDetails(uint id)
        {
            List<ItemDetails> itemData = new();

            //Find the item if it is nearby
            GameObject itemNearby = ((ItemContainer)cachedPlayerController.CurrentPlayer.Extensions.Get("ItemContainer")).Inventory
                .FirstOrDefault(i => id == (uint)i.Properties.GetPropertyValue("ID"));

            if (itemNearby is default(GameObject))
                itemNearby = ((ItemContainer)cachedPlayerController.CurrentRoom.Extensions.Get("ItemContainer")).Inventory
                .FirstOrDefault(i => id == (uint)i.Properties.GetPropertyValue("ID"));

            if (itemNearby is default(GameObject))
                return itemData; // If it is not nearby, return an empty list.


            //Populate the list with properties
            foreach (var property in itemNearby.Properties.EntityProperties.ToList())
                itemData.Add(new ItemDetails(property.PropertyName, property.PropertyValue.ToString()));

            //Populate the list with extension data
            foreach (var extension in itemNearby.Extensions.EntityExtensions.ToList())
                foreach (var property in extension.GetAllEntries())
                    itemData.Add(new ItemDetails(extension.UniqueIdentifier, property.ToString()));


            return itemData;
        }

        public string GetInspectDescriptor(uint itemID)
        {
            string inspectDescriptor = $"ERROR. Couldn't retrieve inspect descriptor for {itemID}. Unknown error.";

            //Find the item if it is nearby
            GameObject itemNearby = ((ItemContainer)cachedPlayerController.CurrentPlayer.Extensions.Get("ItemContainer")).Inventory
                .FirstOrDefault(i => itemID == (uint)i.Properties.GetPropertyValue("ID"));

            if (itemNearby is default(GameObject))
                itemNearby = ((ItemContainer)cachedPlayerController.CurrentRoom.Extensions.Get("ItemContainer")).Inventory
                .FirstOrDefault(i => itemID == (uint)i.Properties.GetPropertyValue("ID"));

            if (itemNearby is default(GameObject))
                return inspectDescriptor; // If it is not nearby, return an error placeholder descriptor.


            //Get the descriptor when found
            inspectDescriptor = DescriptorProcessor.GetDescriptor(itemNearby, "Descriptor.Inspect.Current");

            return inspectDescriptor;
        }

        public string GetItemName(uint itemID)
        {
            string itemName = $"ERROR. Couldn't retrieve item Name for {itemID}. Unknown error.";

            //Find the item if it is nearby
            GameObject itemNearby = ((ItemContainer)cachedPlayerController.CurrentPlayer.Extensions.Get("ItemContainer")).Inventory
                .FirstOrDefault(i => itemID == (uint)i.Properties.GetPropertyValue("ID"));

            if (itemNearby is default(GameObject))
                itemNearby = ((ItemContainer)cachedPlayerController.CurrentRoom.Extensions.Get("ItemContainer")).Inventory
                .FirstOrDefault(i => itemID == (uint)i.Properties.GetPropertyValue("ID"));

            if (itemNearby is default(GameObject))
                return itemName; // If it is not nearby, return an error placeholder.


            //Get the name when found
            itemName = (string) itemNearby.Properties.GetPropertyValue("Name");

            return itemName;
        }

        //public event EventHandler<uint> InventoryAddition; //Informs an endpoint that a new item ID was added to the player's inventory.
        //public event EventHandler<uint> InventoryRemoval; //Informs an endpoint that an item ID was removed from the player's inventory.
        //public event EventHandler InventoryRefreshRequired; //Instructs endpoints to re-request their cache.
    }
}
