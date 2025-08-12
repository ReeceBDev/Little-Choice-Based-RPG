using Little_Choice_Based_RPG.Resources.Entities;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Resources.Systems.InformationalSystems.Descriptor;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.DoubleParameterDelegates;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.TripleParameterDelegates;
using static Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.DoubleParameterDelegates.InteractionUsingGameObjectAndCurrentPlayer;
using static Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.TripleParameterDelegates.InteractionUsingGameObjectAndCurrentPlayerAndCurrentRoom;
using static Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.TripleParameterDelegates.InteractionUsingTwoGameObjectsAndCurrentPlayer;

namespace Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory
{
    internal static class InventoryDelegation
    {
        public static IInvokableInteraction GenerateMoveIntoGameObjectContainer(Player targetPlayer, PropertyContainer targetContainer)
        {
            return GenerateMoveIntoContainerInteraction((GameObject)targetContainer);
        }

        public static InteractionUsingGameObjectAndCurrentPlayerAndCurrentRoom NewPickup(GameObject targetObject)
        {
            InteractionUsingGameObjectAndCurrentPlayerAndCurrentRoomDelegate pickupFromRoomDelegate = InventoryProcessor.PickupFromRoom;

            var pickupTargetFromRoom = new InteractionUsingGameObjectAndCurrentPlayerAndCurrentRoom
            (
                pickupFromRoomDelegate,
                DescriptorProcessor.GetDescriptor(targetObject, "InventorySystem.Interaction.Pickup.Title"),
                DescriptorProcessor.GetDescriptor(targetObject, "InventorySystem.Interaction.Pickup.Invoking"),
                targetObject
            );

            return pickupTargetFromRoom;
        }

        public static InteractionUsingGameObjectAndCurrentPlayerAndCurrentRoom NewPutdown(GameObject targetObject)
        {
            InteractionUsingGameObjectAndCurrentPlayerAndCurrentRoomDelegate dropIntoRoomDelegate = InventoryProcessor.DropIntoRoom;

            var putdownTargetIntoRoom = new InteractionUsingGameObjectAndCurrentPlayerAndCurrentRoom
            (
                dropIntoRoomDelegate,
                DescriptorProcessor.GetDescriptor(targetObject, "InventorySystem.Interaction.Drop.Title"),
                DescriptorProcessor.GetDescriptor(targetObject, "InventorySystem.Interaction.Drop.Invoking"),
                targetObject
            );

            return putdownTargetIntoRoom;
        }

        public static InteractionUsingGameObjectAndCurrentPlayer GenerateOpenInteraction(GameObject targetObject)
        {
            InteractionUsingGameObjectAndCurrentPlayerDelegate openContainerDelegate = InventoryProcessor.OpenContainer;

            var putdownTargetIntoRoom = new InteractionUsingGameObjectAndCurrentPlayer
            (
                openContainerDelegate,
                DescriptorProcessor.GetDescriptor(targetObject, "InventorySystem.Interaction.Open.Title"),
                DescriptorProcessor.GetDescriptor(targetObject, "InventorySystem.Interaction.Open.Invoking"),
                targetObject
            );

            return putdownTargetIntoRoom;
        }

        public static InteractionUsingTwoGameObjectsAndCurrentPlayer GenerateMoveIntoContainerInteraction(GameObject targetContainer)
        {
            InteractionUsingTwoGameObjectsAndCurrentPlayerDelegate moveIntoContainerDelegate = InventoryProcessor.MoveIntoContainer;

            //(Delegate setDelegate, string setInteractTitle, string setInteractDescriptor, string requestDescription, GameObject preassignedParameter2, InteractionRole setInteractRole = InteractionRole.Explore, List<EntityProperty>? gameObjectRequestFilter = null)
            var moveIntoContainer = new InteractionUsingTwoGameObjectsAndCurrentPlayer
            (
                moveIntoContainerDelegate,
                DescriptorProcessor.GetDescriptor(targetContainer, "InventorySystem.Interaction.LoadIntoInventory.Title"),
                DescriptorProcessor.GetDescriptor(targetContainer, "InventorySystem.Interaction.LoadIntoInventory.Invoking"),
                $"Select an object to put inside the {targetContainer}",
                targetContainer
            );

            return moveIntoContainer;
        }
    }
}
