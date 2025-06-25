using Little_Choice_Based_RPG.Resources.Entities;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PrivateInteractionsSystems.PrivateInteractionsExtensions;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PublicInteractionsSystems.PublicInteractionsExtensions;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.Interactions;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;

namespace Little_Choice_Based_RPG.Managers.PlayerControl
{
    public static class InteractionRetriever
    {
        /// <summary> Returns a dictionary of public interactions in a room, and their IDs, by each entity in that room.
        /// Interactions may be optionally filtered by a required InteractionRole. Results may additionally be filtered by matching properties. </summary>
        public static List<IInvokableInteraction> GetInteractions(Room currentRoom, InteractionRole? role = null, List<EntityProperty>? setFilters = null)
        {
            var relevantInteractions = new List<IInvokableInteraction>();
            List<GameObject> targetObjects = InventoryProcessor.GetInventoryEntities(currentRoom, setFilters);

            foreach (GameObject target in targetObjects)
            {
                if (target.Extensions.Contains("PublicInteractions"))
                    foreach (var interactionKeyValuePair in ((PublicInteractions) target.Extensions.Get("PublicInteractions")).PublicInteractionsList)
                    {
                        //If an interaction context has been set. but does not match, skip.
                        if (role is not null && interactionKeyValuePair.Key.InteractionContext != role)
                            continue;

                        relevantInteractions.Add(interactionKeyValuePair.Key);
                    }
            }

            return relevantInteractions;
        }

        /// <summary> Returns a dictionary of all possible public interactions on a particular GameObject and their IDs. </summary>
        public static List<IInvokableInteraction> GetInteractions(GameObject targetObject)
        {
            List<IInvokableInteraction> relevantInteractions = new();

            if (targetObject.Extensions.Contains("PublicInteractions"))
                foreach (var interactionKeyValuePair in ((PublicInteractions)targetObject.Extensions.Get("PublicInteractions")).PublicInteractionsList)
                {
                    relevantInteractions.Add(interactionKeyValuePair.Key);
                }                

            return relevantInteractions;
        }


        /// <summary> Returns a dictionary of private interactions on a player and their IDs. Optionally return only those with a matching interaction context. </summary>
        public static Dictionary<IInvokableInteraction, ulong> GetPrivateInteractions(Player currentPlayer, InteractionRole? role = null)
        {
            if (!currentPlayer.Extensions.Contains("PrivateInteractions"))
                throw new ArgumentException($"The current player, {currentPlayer} does not contain an extension named \"PrivateInteractions\" and therefore can't hold the private interactions! Extensions list: {currentPlayer.Extensions}.");


            var retrievedInteractions = new Dictionary<IInvokableInteraction, ulong>();
            PrivateInteractions playerInteractions = (PrivateInteractions)currentPlayer.Extensions.Get("PrivateInteractions");
            
            //Grab each interaction which matches the interaction context
            foreach (KeyValuePair<KeyValuePair<PropertyContainer, IInvokableInteraction>, ulong> interactionKeyValuePair in playerInteractions.privateInteractionsList)
            {
                //If an interaction context has been set. but does not match, skip.
                if (role is not null && interactionKeyValuePair.Key.Value.InteractionContext != role)
                    continue;

                retrievedInteractions.Add(interactionKeyValuePair.Key.Value, interactionKeyValuePair.Value);
            }

            return retrievedInteractions;
        }

        /// <summary> Returns a dictionary of all private interactions on a player, and their IDs, for a particular GameObject. </summary>
        public static Dictionary<IInvokableInteraction, ulong> GetPrivateInteractions(Player currentPlayer, GameObject targetObject)
        {
            if (!currentPlayer.Extensions.Contains("PrivateInteractions"))
                throw new ArgumentException($"The current player, {currentPlayer} does not contain an extension named \"PrivateInteractions\" and therefore can't hold the private interactions! Extensions list: {currentPlayer.Extensions}.");


            var retrievedInteractions = new Dictionary<IInvokableInteraction, ulong>();
            PrivateInteractions playerInteractions = (PrivateInteractions)currentPlayer.Extensions.Get("PrivateInteractions");


            //Grab each interaction which matches the targetObject
            foreach (KeyValuePair<KeyValuePair<PropertyContainer, IInvokableInteraction>, ulong> interactionKeyValuePair in playerInteractions.privateInteractionsList)
            {
                if (interactionKeyValuePair.Key.Key == targetObject)
                    retrievedInteractions.Add(interactionKeyValuePair.Key.Value, interactionKeyValuePair.Value);
            }

            return retrievedInteractions;
        }
    }
}