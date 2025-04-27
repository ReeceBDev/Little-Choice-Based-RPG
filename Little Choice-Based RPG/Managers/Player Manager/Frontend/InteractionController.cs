using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface;
using Little_Choice_Based_RPG.Managers.World;
using Little_Choice_Based_RPG.Resources;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory.InventoryExtensions;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PrivateInteractionsSystems.PrivateInteractionsExtensions;
using Little_Choice_Based_RPG.Types;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.Interactions;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Managers.Player_Manager.Frontend
{
    public static class InteractionController
    {
        /// <summary> Returns a list of interactions based on the required context. Objects may be filtered. </summary>
        public static List<IInvokableInteraction>? GetInteractions(Room currentRoom, InteractionRole ofContext, EntityProperty setFilter)
            => GetInteractions(currentRoom, ofContext, setFilter);

        /// <summary> Returns a list of interactions by interaction context. Objects may be filtered. </summary>
        public static List<IInvokableInteraction> GetInteractions(Room currentRoom, InteractionRole ofContext, List<EntityProperty>? setFilters = null)
        {
            var relevantInteractions = new List<IInvokableInteraction>();
            List<GameObject> targetObjects = InventoryProcessor.GetInventoryEntities(currentRoom, setFilters);

            foreach (GameObject target in targetObjects)
            {
                foreach (IInvokableInteraction interaction in target.Interactions)
                {
                    if (interaction.InteractionContext == ofContext)
                        relevantInteractions.Add(interaction);
                }
            }

            return relevantInteractions;
        }

        /// <summary> Returns a list of all possible interactions on a particular GameObject. </summary>
        public static List<IInvokableInteraction> GetInteractions(GameObject targetObject)
        {
            List<IInvokableInteraction> relevantInteractions = new();

            foreach (IInvokableInteraction newInteraction in targetObject.Interactions)
                relevantInteractions.Add(newInteraction);

            return relevantInteractions;
        }


        /// <summary> Returns a list of private interactions on a player by their interaction context. </summary>
        public static List<IInvokableInteraction> GetPrivateInteractions(Player currentPlayer, InteractionRole ofContext)
        {
            if (!currentPlayer.Extensions.Contains("PrivateInteractions"))
                throw new ArgumentException($"The current player, {currentPlayer} does not contain an extension named \"PrivateInteractions\" and therefore can't hold the private interactions! Extensions list: {currentPlayer.Extensions}.");


            var retrievedInteractions = new List<IInvokableInteraction>();
            PrivateInteractions playerInteractions = (PrivateInteractions)currentPlayer.Extensions.Get("PrivateInteractions");
            
            //Grab each interaction which matches the interaction context
            foreach (KeyValuePair<PropertyContainer, IInvokableInteraction> interactionKeyValuePair in playerInteractions.PrivateInteractionsList)
            {
                if (interactionKeyValuePair.Value.InteractionContext == ofContext)
                    retrievedInteractions.Add(interactionKeyValuePair.Value);
            }

            return retrievedInteractions;
        }

        /// <summary> Returns a list of all private interactions on a player, for a particular GameObject. </summary>
        public static List<IInvokableInteraction> GetPrivateInteractions(Player currentPlayer, GameObject targetObject)
        {
            if (!currentPlayer.Extensions.Contains("PrivateInteractions"))
                throw new ArgumentException($"The current player, {currentPlayer} does not contain an extension named \"PrivateInteractions\" and therefore can't hold the private interactions! Extensions list: {currentPlayer.Extensions}.");


            var retrievedInteractions = new List<IInvokableInteraction>();
            PrivateInteractions playerInteractions = (PrivateInteractions)currentPlayer.Extensions.Get("PrivateInteractions");


            //Grab each interaction which matches the targetObject
            foreach (KeyValuePair<PropertyContainer, IInvokableInteraction> interactionKeyValuePair in playerInteractions.PrivateInteractionsList)
            {
                if (interactionKeyValuePair.Key == targetObject)
                    retrievedInteractions.Add(interactionKeyValuePair.Value);
            }

            return retrievedInteractions;
        }
    }
}