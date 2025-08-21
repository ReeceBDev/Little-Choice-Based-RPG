using Little_Choice_Based_RPG.Resources.Entities;
using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory;
using Little_Choice_Based_RPG.Resources.Systems.InformationalSystems.Descriptor;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.TripleParameterDelegates;
using Little_Choice_Based_RPG.Types.Navigation;
using static Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.TripleParameterDelegates.InteractionUsingCurrentPlayerAndCurrentRoomAndPreassignedRoom;

namespace Little_Choice_Based_RPG.Resources.Systems.RoomSystems
{
    internal static class DirectionDelegation
    {
        public static InteractionUsingCurrentPlayerAndCurrentRoomAndPreassignedRoom NewChangeRoom(PropertyContainer destination, CardinalDirection? direction)
        {
            string directionName = direction.Value.ToString() ?? "Travel";

            InteractionUsingCurrentPlayerAndCurrentRoomAndPreassignedRoomDelegate changeRoomDelegate = InventoryProcessor.MoveBetweenInventories;

            //(Delegate setDelegate, string setInteractTitle, string setInteractDescriptor, string requestDescription, GameObject preassignedParameter2, InteractionRole setInteractRole = InteractionRole.Explore, List<EntityProperty>? gameObjectRequestFilter = null)
            var changeRoom = new InteractionUsingCurrentPlayerAndCurrentRoomAndPreassignedRoom
            (
                changeRoomDelegate,
                (uint)destination.Properties.GetPropertyValue("ID"),
                $"{directionName} - {DescriptorProcessor.GetDescriptor(destination, "DirectionSystem.Interaction.Travel.Title")}",
                DescriptorProcessor.GetDescriptor(destination, "DirectionSystem.Interaction.Travel.Description"),
                (Room) destination
            );

            return changeRoom;
        }
    }
}
