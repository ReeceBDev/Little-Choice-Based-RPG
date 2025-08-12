using Little_Choice_Based_RPG.Resources.Entities;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory.InventoryExtensions;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PrivateInteractionsSystems;
using Little_Choice_Based_RPG.Resources.Systems.RoomSystems.DirectionExtensions;
using Little_Choice_Based_RPG.Resources.Systems.SystemEventBus;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.Navigation;
using Little_Choice_Based_RPG.Types.TypedEventArgs.PropertyContainerEventArgs;

namespace Little_Choice_Based_RPG.Resources.Systems.RoomSystems
{
    internal class DirectionSystem : PropertyLogic
    {
        static DirectionSystem()
        {
            PropertyValidation.CreateValidProperty("DirectionSystem.Interaction.Travel.Description", PropertyType.String);
            PropertyValidation.CreateValidProperty("DirectionSystem.Interaction.Travel.Title", PropertyType.String);
        }


        protected override void InitialiseNewSubscriber(PropertyContainer sourceContainer, PropertyHandler sourceProperties)
        {
            //The subscriber requires Component.InventorySystem.
            if (!sourceProperties.HasExistingPropertyName("Component.InventorySystem"))
            {
                sourceProperties.CreateProperty("Component.InventorySystem", true); //Creates the inventorySystem component.
                SystemSubscriptionEventBus.Subscribe(sourceContainer, "InventorySystem"); //Subscribes the source to the component.

                //Reregister this system.
                //This ensures InventorySysetm System is called before DirectionSystem, as DirectionSystem requires Component.InventorySysetm and assumes it has been executed.
                SystemSubscriptionEventBus.Unsubscribe(sourceContainer, "DirectionSystem");
                SystemSubscriptionEventBus.Subscribe(sourceContainer, "DirectionSystem");

                //Break, as this logic will be continued in the re-subscription.
                return;
            }

            //Give the subscriber a RoomDirections extension.
            if (!sourceContainer.Extensions.Contains("RoomDirections")) //If RoomDirections isnt already assigned
            {
                //Generate new RoomDirections
                RoomConnections newExtension = new RoomConnections();

                //Then add the RoomDirections to the source's extensions.
                sourceContainer.Extensions.AddExtension(newExtension);
            }
            else //If an RoomDirections is already assigned
            {
                throw new NotImplementedException("Not yet implemented. This is waiting for the loading of saved states!");
            }

            //Require them to have a travel description
            if (!sourceContainer.Properties.HasExistingPropertyName("DirectionSystem.Interaction.Travel.Description"))
                throw new Exception($"Tried to intialise new subscriber {sourceContainer} to DirectionSystem but they lacked the required property DirectionSystem.Interaction.Travel.Description!");

            //Require them to have a travel interaction title
            if (!sourceContainer.Properties.HasExistingPropertyName("DirectionSystem.Interaction.Travel.Title"))
                throw new Exception($"Tried to intialise new subscriber {sourceContainer} to DirectionSystem but they lacked the required property DirectionSystem.Interaction.Travel.Title!");
        }

        protected override void OnObjectChanged(object sender, ObjectChangedEventArgs the)
        {
            //If it was a Room...
            if (the.Source is Room room)
            {
                List<RoomConnection> roomDirections = new();
                List<GameObject> roomContents = new();

                //If RoomDirections is being accessed, cache it.
                if (the.Property.StartsWith("RoomDirections."))
                    roomDirections = ((RoomConnections) room.Extensions.Get("RoomDirections")).LocalConnections;

                //If the ItemContainer is being accessed, cache it.
                if (the.Property.StartsWith("ItemContainer."))
                    roomContents = ((ItemContainer)room.Extensions.Get("ItemContainer")).Inventory;


                switch (the.Property, the.Change)
                {
                    //If Visibility of a RoomDescriptor were changed to true.
                    case ("RoomDirections.Changed", (RoomConnection targetDirection, "Visibility", true)):
                        {
                            //Try to give every player movement using it
                            foreach (Player eachPlayer in roomContents)
                                InteractionRegistrar.TryAddPrivateInteraction(eachPlayer, room, DirectionDelegation.NewChangeRoom(room, targetDirection.Direction));
                        }
                        break;

                    //If Visibility of a RoomDescriptor were changed to false.
                    case ("RoomDirections.Changed", (RoomConnection targetDirection, "Visibility", false)):
                        {
                            //Remove any movement every player has for it
                            foreach (Player eachPlayer in roomContents)
                                InteractionRegistrar.TryRemovePrivateInteraction(eachPlayer, room, DirectionDelegation.NewChangeRoom(room, targetDirection.Direction));
                        }
                        break;


                    //If it was a room... Getting a player
                    case ("ItemContainer.Added", Player newPlayer):
                        {
                            //Give the player a movement for every RoomDirection in the room, if it is usable.
                            foreach (var eachDirection in roomDirections)
                            {
                                if (eachDirection.IsVisible is true)
                                    InteractionRegistrar.TryAddPrivateInteraction(newPlayer, room, DirectionDelegation.NewChangeRoom(room, eachDirection.Direction));
                            }
                        }
                        break;

                    //If it was a room... Losing a player
                    case ("ItemContainer.Removed", Player lostPlayer):
                        {
                            //Remove from the player all of their old room's travel interactions, if they have any left.
                            foreach (var eachDirection in roomDirections)
                            {
                                InteractionRegistrar.TryRemovePrivateInteraction(lostPlayer, room, DirectionDelegation.NewChangeRoom(room, eachDirection.Direction));
                            }
                            break;
                        }
                }
            }
        }
    }
}