using Little_Choice_Based_RPG.Managers.Player_Manager;
using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface;
using Little_Choice_Based_RPG.Managers.World;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Resources.EventArgs;
using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Resources.Systems.DamageSystems;
using Little_Choice_Based_RPG.Resources.Systems.SystemEventBus;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.Interactions.Delegates;
using Little_Choice_Based_RPG.Types.PropertyExtensions.Extensions;
using static Little_Choice_Based_RPG.Types.Interactions.Delegates.InteractionUsingNothing;
using static Little_Choice_Based_RPG.Types.Interactions.Delegates.SingleParameterDelegates.InteractionUsingGameObjectAndCurrentPlayerAndCurrentRoom;

namespace Little_Choice_Based_RPG.Resources.Systems.ContainerSystems
{
    /// <summary> Allows PropertyContainers to contain other GameObject. This takes them out of the Rooms' own contents. Requires WeightbearingLogic. </GameObject> </summary>
    public class InventorySystem : PropertyLogic
    {

        protected override void InitialiseNewSubscriber(PropertyContainer sourceContainer, PropertyHandler sourceProperties)
        {
            //The subscriber requires Component.Weightbearing. This then checks for the required weightbearing values itself :)
            if (!sourceProperties.HasExistingPropertyName("Component.WeightbearingLogic"))
            {
                sourceProperties.CreateProperty("Component.WeightbearingLogic", true); //Creates the weightbearing component.
                SystemSubscriptionEventBus.Subscribe(sourceContainer, "WeightbearingLogic"); //Subscribes the source to the component.
            }

            //Give the subscriber an ItemContainer
            if (!sourceContainer.Extensions.ContainsExtension("Inventory")) //If an ItemContainer isnt already assigned
            {
                //Generate new Inventory
                ItemContainer newInventory = new ItemContainer();

                //Then add the inventory to the source's extensions.
                sourceContainer.Extensions.AddExtension(newInventory);
            }
            else //If an ItemContainer is already assigned
            {
                //Find the correct one (by unique ID on each inventory?)
                //Load it/assign it or whatever else is necessary
                //Update inventory weight limit from source object

                throw new NotImplementedException("Not yet implemented. This is waiting for the loading of saved states!");
            }
        }

        protected override void OnObjectChanged(object sender, ObjectChangedEventArgs args)
        {
            //If it was a player...
            if (args.SourceContainer.Properties.GetPropertyValue("Type").Equals("Player"))
            {
                //Getting an item
                if (args.ValueChanged.Equals("ItemContainer.Added"))
                {
                    //Remove its pickup
                    throw new NotImplementedException();
                    GivePrivateInteraction((Player)args.SourceContainer, GeneratePrivatePutdownInteractionUsingCurrentRoomAndGameObject((Player)args.SourceContainer, (GameObject)args.ValueChanged));
                }

                //Dropping an item
                if (args.ValueChanged.Equals("ItemContainer.Removed") && WeightbearingLogic.CheckIfCarriable(args.SourceContainer, (GameObject)args.ValueChanged))
                {
                    //Remove its putdown
                    throw new NotImplementedException();
                    GivePrivateInteraction((Player)args.SourceContainer, GeneratePrivatePickupInteractionUsingCurrentRoomAndGameObject((Player)args.SourceContainer, (GameObject)args.ValueChanged));
                }
            }
        }

        /// <summary> Generates interactions for a player upon their rooms' contents changing. </summary>
        public void OnRoomUpdate(Player targetPlayer, ObjectChangedEventArgs roomChangedData)
        {
            if (roomChangedData.IdentifierChanged == "ItemContainer.Added" && WeightbearingLogic.CheckIfCarriable(targetPlayer, (GameObject)roomChangedData.ValueChanged))
                GivePrivateInteraction(targetPlayer, GeneratePrivatePickupInteractionUsingCurrentRoomAndGameObject(targetPlayer, (GameObject) roomChangedData.ValueChanged));

            if (roomChangedData.IdentifierChanged == "ItemContainer.Removed")
            {
                //Remove the same interaction
                throw new NotImplementedException();
            }
        }

        /// <summary> Generates interactions for a player for a specific Room </summary>
        public void GivePlayerRoomPickups(Player targetPlayer, Room targetRoom)
        {
            ItemContainer roomContents = (ItemContainer)targetRoom.Extensions.GetExtension("Inventory");

            //Create pickups
            foreach (GameObject target in roomContents.Inventory)
            {
                if (WeightbearingLogic.CheckIfCarriable(targetPlayer, target))
                    GivePrivateInteraction(targetPlayer, GeneratePrivatePickupInteractionUsingCurrentRoomAndGameObject(targetPlayer, target));
            }
        }

        public void RemovePlayerRoomPickups(Player targetPlayer)
        {
            //Remove pickups
            throw new NotImplementedException();
        }

        public static void StoreInInventory(PlayerController mutexHolder, PropertyContainer targetContainer, GameObject targetEntity)
        {
            if (!targetContainer.Extensions.ContainsExtension("Inventory"))
                throw new Exception($"The target itemContainer {targetContainer} does not contain the inventory extension!");

            if (!WeightbearingLogic.CheckIfCarriable(targetContainer, targetEntity))
                throw new Exception($"The target itemContainer {targetContainer} does not have the strength to carry the {targetEntity}!");

            ItemContainer targetInventory = (ItemContainer)targetContainer.Extensions.GetExtension("Inventory");

            targetInventory.Add(targetEntity);
        }

        public static void RemoveFromInventory(PlayerController mutexHolder, PropertyContainer targetContainer, GameObject targetEntity)
        {
            if (!targetContainer.Extensions.ContainsExtension("Inventory"))
                throw new Exception($"The target itemContainer {targetContainer} does not contain the inventory extension!");

            ItemContainer targetInventory = (ItemContainer)targetContainer.Extensions.GetExtension("Inventory");

            targetInventory.Remove(targetEntity);
        }

        public static void MoveBetweenInventories(PlayerController mutexHolder, GameObject target,
                PropertyContainer source, PropertyContainer destination)
        {
            if (!source.Extensions.ContainsExtension("Inventory"))
                throw new Exception($"Source container {source} is missing the Inventory extension!");

            if (!destination.Extensions.ContainsExtension("Inventory"))
                throw new Exception($"Destination container {destination} is missing the Inventory extension!");

            if (!WeightbearingLogic.CheckIfCarriable(destination, target))
                throw new Exception($"The target itemContainer {destination} does not have the strength to carry the {target}!");


            RemoveFromInventory(mutexHolder, source, target);
            StoreInInventory(mutexHolder, destination, target);
        }

        private InteractionUsingNothing GeneratePickupInteractionUsingNothing(GameObject targetObject)
        {
            InteractionUsingGameObjectAndCurrentPlayerAndCurrentRoomDelegate pickupUsingNothingDelegate = MoveBetweenInventories();

            InteractionUsingNothing repairUsingNothing = new InteractionUsingNothing(
                repairUsingNothingDelegate,
                sourceContainer,
                (string)sourceProperties.GetPropertyValue("Descriptor.Repair.Interaction.Title"),
                (string)sourceProperties.GetPropertyValue("Descriptor.Repair.Interaction.Invoking")
            );

            return repairUsingNothing;
        }

        private InteractionUsingNothing GeneratePutdownInteractionUsingNothing(GameObject targetObject)
        {

        }
    }
}
