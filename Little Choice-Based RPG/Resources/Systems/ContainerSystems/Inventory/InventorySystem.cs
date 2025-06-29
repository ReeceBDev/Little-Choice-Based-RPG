﻿using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Resources.Systems.SystemEventBus;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Resources.PropertyContainerEventArgs;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Weightbearing;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory.InventoryExtensions;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PrivateInteractionsSystems.PrivateInteractionsExtensions;
using Little_Choice_Based_RPG.Resources.Entities;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PublicInteractionsSystems.PublicInteractionsExtensions;

namespace Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory
{
    /// <summary> Allows PropertyContainers to contain other GameObject. This takes them out of the Rooms' own contents. Requires WeightbearingLogic. </GameObject> </summary>
    public class InventorySystem : PropertyLogic
    {
        static InventorySystem()
        {
            PropertyValidation.CreateValidProperty("Descriptor.InventorySystem.Interaction.Pickup.Title", PropertyType.String);
            PropertyValidation.CreateValidProperty("Descriptor.InventorySystem.Interaction.Pickup.Invoking", PropertyType.String);
            PropertyValidation.CreateValidProperty("Descriptor.InventorySystem.Interaction.Drop.Title", PropertyType.String);
            PropertyValidation.CreateValidProperty("Descriptor.InventorySystem.Interaction.Drop.Invoking", PropertyType.String);

            PropertyValidation.CreateValidProperty("Descriptor.InventorySystem.Interaction.Open.Title", PropertyType.String);
            PropertyValidation.CreateValidProperty("Descriptor.InventorySystem.Interaction.Open.Invoking", PropertyType.String);
            PropertyValidation.CreateValidProperty("Descriptor.InventorySystem.Interaction.LoadIntoInventory.Title", PropertyType.String);
            PropertyValidation.CreateValidProperty("Descriptor.InventorySystem.Interaction.LoadIntoInventory.Invoking", PropertyType.String);
        }

        protected override void InitialiseNewSubscriber(PropertyContainer sourceContainer, PropertyHandler sourceProperties)
        {
            //The subscriber requires Component.Weightbearing. This then checks for the required weightbearing values itself :)
            if (!sourceProperties.HasExistingPropertyName("Component.WeightbearingSystem"))
            {
                sourceProperties.CreateProperty("Component.WeightbearingSystem", true); //Creates the weightbearing component.
                SystemSubscriptionEventBus.Subscribe(sourceContainer, "WeightbearingSystem"); //Subscribes the source to the component.

                //Reregister this system.
                //This ensures Weightbearing System is called before Inventory System, as InventorySystem requires Component.Weightbearing and assumes it has been executed.
                SystemSubscriptionEventBus.Unsubscribe(sourceContainer, "InventorySystem");
                SystemSubscriptionEventBus.Subscribe(sourceContainer, "InventorySystem");

                //Break, as this logic will be continued in the re-subscription.
                return;
            }

            //Give the subscriber an ItemContainer
            if (!sourceContainer.Extensions.Contains("ItemContainer")) //If an ItemContainer isnt already assigned
            {
                //Generate new ItemContainer
                ItemContainer newInventory = new ItemContainer();

                //Then add the ItemContainer to the source's extensions.
                sourceContainer.Extensions.AddExtension(newInventory);
            }
            else //If an ItemContainer is already assigned
            {
                //Find the correct one (by unique ID on each inventory?)
                //Load it/assign it or whatever else is necessary
                //Update inventory weight limit from source object

                throw new NotImplementedException("Not yet implemented. This is waiting for the loading of saved states!");
            }

            //Validate the Open Interactions are present on this object.
            InventoryPropertyValidation.ValidateOpenInventoryDescriptors(sourceContainer);

            //if the subscriber is not a player or room, then give them an Open interaction.
            if ((sourceContainer is not Player) && (sourceContainer is not Room))
                PublicInteractions.TryAddPublicInteraction(sourceContainer, InventoryDelegation.GenerateOpenInteraction((GameObject)sourceContainer));
        }

        protected override void OnObjectChanged(object sender, ObjectChangedEventArgs the)
        {
            //If it was a player... 
            if (the.Source is Player player)
            {
                switch (the.Property, the.Change)
                {
                    //If it was a player... Getting an item
                    case ("ItemContainer.Added", GameObject item):
                        {
                            //Add a new putdown for this item
                            PrivateInteractions.TryAddPrivateInteraction(player, item, InventoryDelegation.NewPutdown(item));

                            //Reassess their other pickups, to see if they can still carry it all.
                            foreach (var interactionKeyValuePair in ((PrivateInteractions)player.Extensions.Get("PrivateInteractions")).PrivateInteractionsList.ToList())
                            {
                                //If the interaction target is any GameObject and is a PickUp interaction.
                                if (interactionKeyValuePair.Key.Key is GameObject targetItem &&
                                    interactionKeyValuePair.Key.Value.DelegateRecord.Method.Equals(InventoryDelegation.NewPickup(targetItem).DelegateRecord.Method))
                                {
                                    //If the player can't carry it any longer, remove the interaction.
                                    if (!WeightbearingProcessor.TargetCanCarry(player, targetItem))
                                        switch (PrivateInteractions.TryRemovePrivateInteraction(player, targetItem, interactionKeyValuePair.Key.Value))
                                        {
                                            case 1: throw new Exception($"TryRemovePrivateInteraction() Returned error code: 1. Tried to remove an interaction but the player {player} did not have the extension \"PrivateInteractions\" to begin with!");
                                            case 2: throw new Exception($"TryRemovePrivateInteraction() Returned error code: 2. Tried to remove an interaction but it did not exist on the player {player}!");
                                            case 3: throw new Exception($"TryRemovePrivateInteraction() Returned error code: 3. Tried to remove an interaction but this did not succeed for an unknown reason! {player}!");
                                        }
                                }

                            }
                        }
                        break;

                    //If it was a player... Dropping an item...
                    case ("ItemContainer.Removed", GameObject item):
                        {
                            //Remove the old putdown
                            PrivateInteractions.TryRemovePrivateInteraction(player, item, InventoryDelegation.NewPutdown(item));                          
                            break;
                        }
                }
            }



            if (the.Source is Room room)
            {
                List<GameObject> roomContents = new();

                //If the ItemContainer is being accessed, cache it.
                if (the.Property.StartsWith("ItemContainer."))
                    roomContents = ((ItemContainer)room.Extensions.Get("ItemContainer")).Inventory;


                //Note: Handle player conditions, and break for each one, so that players can't be picked up by others implicitly.
                // In other words, do not run the item conditions at the same time. :)
                switch (the.Property, the.Change)
                {
                    //If it was a room... Getting a player
                    case ("ItemContainer.Added", Player newPlayer):
                        {
                            //Give the player a pickup for everything in the room, if they can carry it
                            foreach (var eachItem in roomContents)
                            {
                                if (WeightbearingProcessor.TargetCanCarry(newPlayer, eachItem))
                                    PrivateInteractions.TryAddPrivateInteraction(newPlayer, eachItem, InventoryDelegation.NewPickup(eachItem));
                            }
                        }
                        break;

                    //If it was a room... Losing a player
                    case ("ItemContainer.Removed", Player lostPlayer):
                        {
                            //Remove from every player in the room their pickups, if they have any.
                            foreach (var eachItem in roomContents)
                            {
                                PrivateInteractions.TryRemovePrivateInteraction(lostPlayer, eachItem, InventoryDelegation.NewPickup(eachItem));
                            }
                        }
                        break;


                    //If it was a room... Getting an item
                    case ("ItemContainer.Added", GameObject newItem):
                        {
                            //Give every player in the room its pickup, if they can carry it.
                            foreach (var eachItem in roomContents)
                            {
                                if (eachItem is Player eachPlayer)
                                {
                                    if (WeightbearingProcessor.TargetCanCarry(eachPlayer, newItem))
                                        PrivateInteractions.TryAddPrivateInteraction(eachPlayer, newItem, InventoryDelegation.NewPickup(newItem));

                                    //Reassess their other pickups, to see if they can carry more, now.
                                    foreach (GameObject testItem in roomContents)
                                    {
                                        //Make sure it's not a player
                                        if (!(testItem is Player))
                                            continue;

                                        //Check if it can be carried
                                        if (!WeightbearingProcessor.TargetCanCarry(eachPlayer, testItem))
                                            continue;

                                        //Make sure the player doesn't have an existing pickup for the item already.
                                        var currentPlayerInteractions = ((PrivateInteractions)eachPlayer.Extensions.Get("PrivateInteractions")).privateInteractionsList;

                                        if (!currentPlayerInteractions.Any(interaction => (interaction.Key.Key == testItem) &&
                                            interaction.Key.Value.DelegateRecord.Method.Equals(InventoryDelegation.NewPickup(testItem).DelegateRecord.Method)))
                                            continue;

                                        //if all tests were successful, give the player a pick - up for it.
                                        PrivateInteractions.TryAddPrivateInteraction(eachPlayer, testItem, InventoryDelegation.NewPickup(testItem));
                                    }
                                }
                            }
                        }
                        break;

                    //If it was a room... Losing an item
                    case ("ItemContainer.Removed", GameObject itemLost):
                        {
                            //Remove from every player in the room its pickup, if they have it
                            foreach (var eachItem in roomContents)
                            {
                                if (eachItem is Player eachPlayer)
                                    PrivateInteractions.TryRemovePrivateInteraction(eachPlayer, itemLost, InventoryDelegation.NewPickup(itemLost));
                            }
                        }
                        break;
                }
            }
        }
    }
}
