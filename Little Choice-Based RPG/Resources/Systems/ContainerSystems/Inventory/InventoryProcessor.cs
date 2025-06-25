using Little_Choice_Based_RPG.External.ConsoleEndpoint.ConsoleMenus;
using Little_Choice_Based_RPG.External.ConsoleEndpoint.ConsoleMenus.ConsoleSubMenus;
using Little_Choice_Based_RPG.Managers.PlayerControl;
using Little_Choice_Based_RPG.Resources.Entities;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory.InventoryExtensions;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Weightbearing;
using Little_Choice_Based_RPG.Types.EntityProperties;

namespace Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory
{
    public static class InventoryProcessor
    {
        public static void StoreInInventory(PlayerController mutexHolder, PropertyContainer targetContainer, GameObject targetEntity)
        {
            if (!targetContainer.Extensions.Contains("ItemContainer"))
                throw new Exception($"The target itemContainer {targetContainer} does not contain the ItemContainer extension!");

            if (!WeightbearingProcessor.TargetCanCarry(targetContainer, targetEntity))
                throw new Exception($"The target itemContainer {targetContainer} does not have the strength to carry the {targetEntity}!");

            ItemContainer targetInventory = (ItemContainer)targetContainer.Extensions.Get("ItemContainer");

            targetInventory.Add(targetEntity);
        }

        public static void RemoveFromInventory(PlayerController mutexHolder, PropertyContainer targetContainer, GameObject targetEntity)
        {
            if (!targetContainer.Extensions.Contains("ItemContainer"))
                throw new Exception($"The target itemContainer {targetContainer} does not contain the ItemContainer extension!");

            ItemContainer targetInventory = (ItemContainer)targetContainer.Extensions.Get("ItemContainer");

            targetInventory.Remove(targetEntity);
        }

        public static void MoveBetweenInventories(PlayerController mutexHolder, GameObject target,
                PropertyContainer origin, PropertyContainer destination)
        {
            if (!origin.Extensions.Contains("ItemContainer"))
                throw new Exception($"Source container {origin} is missing the ItemContainer extension!");

            if (!destination.Extensions.Contains("ItemContainer"))
                throw new Exception($"Destination container {destination} is missing the ItemContainer extension!");

            if (!WeightbearingProcessor.TargetCanCarry(destination, target))
                throw new Exception($"The target itemContainer {destination} does not have the strength to carry the {target}!");


            RemoveFromInventory(mutexHolder, origin, target);
            StoreInInventory(mutexHolder, destination, target);
        }

        public static void OpenContainer(PlayerController mutexHolder, GameObject targetContainer, Player currentPlayer)
        {
            if (!mutexHolder.CurrentConsoleEndpoint.CurrentMenu.Equals(typeof(ExploreMenu)))
                throw new Exception("Tried to create an OpenContainer on the player's current Menu, but it was not ExploreMenu...!");

            ExploreMenu currentMenu = (ExploreMenu) mutexHolder.CurrentConsoleEndpoint.CurrentMenu;

            OpenContainerSubMenu.GenerateOpenContainerSubMenu(targetContainer);
            throw new Exception("Haven't implemented the submenu yet to work with the refactored menu system!" +
                "To do this is pretty easy. Just go to the above method, and then work through passing it its required parameters, " +
                "or changing it where needed. Easy. " +
                "Remember: it probably needs to be passed the current menu, first!");
        }

        public static List<GameObject> GetInventoryEntities(PropertyContainer targetContainer, List<EntityProperty>? requiredProperties = null)
        {
            List<GameObject> validObjects = new List<GameObject>();

            if (!targetContainer.Extensions.Contains("ItemContainer"))
                throw new ArgumentException($"The PropertyContainer {targetContainer} didn't contain an extension named ItemContainer!");

            ItemContainer currentItemContainer = (ItemContainer)targetContainer.Extensions.Get("ItemContainer");

            foreach (GameObject entity in currentItemContainer.Inventory)
            {
                if (!entity.Properties.HasPropertyAndValue("IsImmaterial", true))
                {
                    if (requiredProperties != null)
                    {
                        //Check the required properties are all contained within the entity
                        List<EntityProperty> validProperties = new();

                        //foreach (EntityProperty currentProperty in requiredProperties)
                        foreach (EntityProperty requiredProperty in requiredProperties)
                        {
                            string propertyName = requiredProperty.PropertyName;
                            object propertyValue = requiredProperty.PropertyValue;

                            if (entity.Properties.HasPropertyAndValue(propertyName, propertyValue))
                                validProperties.Add(requiredProperty);
                        }

                        //if an object has all of the validProperties, then add the entity to validObjects
                        if (validProperties.Count == requiredProperties.Count)
                            validObjects.Add(entity);
                    }
                    else
                        validObjects.Add(entity); //If objects require no properties, then they all match and should be added.
                }
            }
            return validObjects;
        }

        public static void PickupFromRoom(PlayerController mutexHolder, GameObject target, Player currentPlayer, Room currentRoom)
            => MoveBetweenInventories(mutexHolder, target, currentRoom, currentPlayer);

        public static void DropIntoRoom(PlayerController mutexHolder, GameObject target, Player currentPlayer, Room currentRoom)
            => MoveBetweenInventories(mutexHolder, target, currentPlayer, currentRoom);

        public static void MoveIntoContainer(PlayerController mutexHolder, GameObject target, GameObject targetContainer, Player currentPlayer)
            => MoveBetweenInventories(mutexHolder, target, currentPlayer, targetContainer);
    }
}
