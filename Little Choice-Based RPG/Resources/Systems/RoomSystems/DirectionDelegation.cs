using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory;
using Little_Choice_Based_RPG.Resources.Systems.InformationalSystems.Descriptor;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.TripleParameterDelegates;
using Little_Choice_Based_RPG.Types.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.TripleParameterDelegates.InteractionUsingCurrentPlayerAndCurrentRoomAndPreassignedRoom;
using static Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.TripleParameterDelegates.InteractionUsingTwoGameObjectsAndCurrentPlayer;

namespace Little_Choice_Based_RPG.Resources.Systems.RoomSystems
{
    public static class DirectionDelegation
    {
        public static InteractionUsingCurrentPlayerAndCurrentRoomAndPreassignedRoom NewChangeRoom(PropertyContainer destination, CardinalDirection? direction)
        {
            string directionName = direction.Value.ToString() ?? "Travel";

            InteractionUsingCurrentPlayerAndCurrentRoomAndPreassignedRoomDelegate changeRoomDelegate = InventoryProcessor.MoveBetweenInventories;

            //(Delegate setDelegate, string setInteractTitle, string setInteractDescriptor, string requestDescription, GameObject preassignedParameter2, InteractionRole setInteractRole = InteractionRole.Explore, List<EntityProperty>? gameObjectRequestFilter = null)
            var changeRoom = new InteractionUsingCurrentPlayerAndCurrentRoomAndPreassignedRoom
            (
                changeRoomDelegate,
                $"{directionName} - {DescriptorProcessor.GetDescriptor(destination, "DirectionSystem.Interaction.Travel.Title")}",
                DescriptorProcessor.GetDescriptor(destination, "DirectionSystem.Interaction.Travel.Description"),
                (Room) destination
            );

            return changeRoom;
        }
    }
}
