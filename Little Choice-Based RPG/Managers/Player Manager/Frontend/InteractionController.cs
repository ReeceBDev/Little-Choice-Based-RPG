using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface;
using Little_Choice_Based_RPG.Managers.World;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Types;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.Interactions.InteractDelegate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.Interaction
{
    public static class InteractionController
    {
        /// <summary> Returns a list of interactions based on the required context. </summary>
        public static List<IInvokableInteraction>? GetInteractions(InteractionRole ofContext, Room currentRoom) //May return nothing, so I've declared the return type as nullable.
        {
            var relevantInteractions = new List<IInvokableInteraction>();
            var targetObjects = new List<GameObject>(currentRoom.GetRoomObjects());

            foreach (GameObject target in targetObjects)
            {
                foreach (IInvokableInteraction interaction in target.InteractionChoices)
                {
                    if (interaction.InteractionContext == ofContext)
                        relevantInteractions.Add(interaction);
                }
            }

            return relevantInteractions;
        }

        /// <summary> Returns a list of all possible interactions on a particular GameObject. </summary>
        public static List<IInvokableInteraction>? GetInteractions(GameObject ofGameObject, Room currentRoom) //May return nothing, so I've declared the return type as nullable.
        {
            var relevantInteractions = new List<IInvokableInteraction>();
            var targetObjects = new List<GameObject>(currentRoom.GetRoomObjects());

            if (!currentRoom.RoomContainsObject(ofGameObject))
                throw new ArgumentException($"The requested GameObject does not exist in this room! There is no local GameObject matching {ofGameObject} in room {currentRoom}.");

            foreach (IInvokableInteraction newInteraction in ofGameObject.InteractionChoices)
            {
                relevantInteractions.Add(newInteraction);
            }

            return relevantInteractions;
        }
    }
}