using Little_Choice_Based_RPG.Managers.Player_Manager;
using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface;
using Little_Choice_Based_RPG.Resources;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.TypedEventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.InteractionUsingNothing;

namespace Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.TripleParameterDelegates
{
    /// <summary> Provides a way to present options and choices to the player by exposing a delegate with pre-defined parameters. </summary>
    public class InteractionUsingTwoGameObjectsAndCurrentPlayer : Interaction
    {
        /// <summary> Delegate to be invoked later with Invoke(). </summary>
        public InteractionUsingTwoGameObjectsAndCurrentPlayerDelegate storedDelegate;

        private GameObject? storedPreassignedParameter1 = null;
        private GameObject? storedPreassignedParameter2 = null;

        private GameObject? invocationParameter1 = null;
        private GameObject? invocationParameter2 = null;
        private Player? invocationParameter3 = null;

        private List<EntityProperty>? invocationParameter1Filters = null;
        private List<EntityProperty>? invocationParameter2Filters = null;

        private string invocationParameter1Description;
        private string invocationParameter2Description;

        private InteractionUsingNothing abortInteraction;

        static InteractionUsingTwoGameObjectsAndCurrentPlayer()
        {
            InteractionValidation.CreateValidDelegate("InteractUsingTargetObject", [InteractionParameter.Target_GameObject]);
        }
        /// <summary> Creates a new interaction for players to be presented with in ChoiceHandler. Requests two GameObject from the player. Each GameObjectRequest may be filtered. </summary>
        public InteractionUsingTwoGameObjectsAndCurrentPlayer(InteractionUsingTwoGameObjectsAndCurrentPlayerDelegate setDelegate, PropertyContainer setSourceContainer, string setInteractTitle, string setInteractDescriptor, string firstRequestDescription, string secondRequestDescription, InteractionRole setInteractRole = InteractionRole.Explore, List<EntityProperty>? setFirstGameObjectFilter = null, List<EntityProperty>? setSecondGameObjectFilter = null)
            : base(setDelegate, setSourceContainer, setInteractTitle, setInteractDescriptor, setInteractRole)
        {
            storedDelegate = setDelegate;

            invocationParameter1Description = firstRequestDescription;
            invocationParameter2Description = secondRequestDescription;

            if (setFirstGameObjectFilter != null)
                invocationParameter1Filters = setFirstGameObjectFilter;

            if (setSecondGameObjectFilter != null)
                invocationParameter2Filters = setSecondGameObjectFilter;
        }

        /// <summary> Creates a new interaction for players to be presented with in ChoiceHandler. The first GameObject is pre-assigned. Requests a second GameObject from the player. The GameObjectRequest may be filtered. </summary>
        public InteractionUsingTwoGameObjectsAndCurrentPlayer(InteractionUsingTwoGameObjectsAndCurrentPlayerDelegate setDelegate, PropertyContainer setSourceContainer, string setInteractTitle, string setInteractDescriptor, GameObject preassignedParameter1, string requestDescription, InteractionRole setInteractRole = InteractionRole.Explore, List<EntityProperty>? gameObjectRequestFilter = null)
            : base(setDelegate, setSourceContainer, setInteractTitle, setInteractDescriptor, setInteractRole)
        {
            storedDelegate = setDelegate;

            //Set invocationParameter1 to preassignedParameter1 and save the preassignment in case of cancellation:
            invocationParameter1 = preassignedParameter1;
            storedPreassignedParameter1 = preassignedParameter1;

            invocationParameter2Description = requestDescription;

            if (gameObjectRequestFilter != null)
                invocationParameter2Filters = gameObjectRequestFilter;
        }

        /// <summary> Creates a new interaction for players to be presented with in ChoiceHandler. Requests the first GameObject from the player. The second GameObject is pre-assigned.  The GameObjectRequest may be filtered. </summary>
        public InteractionUsingTwoGameObjectsAndCurrentPlayer(InteractionUsingTwoGameObjectsAndCurrentPlayerDelegate setDelegate, PropertyContainer setSourceContainer, string setInteractTitle, string setInteractDescriptor, string requestDescription, GameObject preassignedParameter2, InteractionRole setInteractRole = InteractionRole.Explore, List<EntityProperty>? gameObjectRequestFilter = null)
            : base(setDelegate, setSourceContainer, setInteractTitle, setInteractDescriptor, setInteractRole)
        {
            storedDelegate = setDelegate;

            //Set invocationParameter2 to preassignedParameter2 and save the preassignment in case of cancellation:
            invocationParameter2 = preassignedParameter2;
            storedPreassignedParameter2 = preassignedParameter2;

            invocationParameter1Description = requestDescription;

            if (gameObjectRequestFilter != null)
                invocationParameter1Filters = gameObjectRequestFilter;
        }

        /// <summary> Invokes the delegate, requesting further user input if necessary. </summary>
        public override void AttemptInvoke(PlayerController sourceInvocationMutexIdentity)
        {
            //Hold mutex if not already held
            if (invocationMutexIdentity == null)
                invocationMutexIdentity = sourceInvocationMutexIdentity; //Hold mutex

            //Check if a held mutex matches the current identity.
            if (invocationMutexIdentity != sourceInvocationMutexIdentity)
                return; //The sender identity sourceInvocationMutexIdentity did not match the current invocationMutexIdentity");


            //If all parameters are already filled, invoke
            if (invocationParameter1 != null && invocationParameter2 != null)
            {
                Invoke(sourceInvocationMutexIdentity);
                return;
            }

            //Otherwise, fill each remaining parameter with GameObjectRequests:

            //Create abort delegate using this invocationMutexIdentity
            InteractionUsingNothingDelegate abortInteractionDelegate = new InteractionUsingNothingDelegate(CancelInteraction);
            abortInteraction = new InteractionUsingNothing(abortInteractionDelegate, AssociatedSource, "Cancel selection", "Cancelling this interaction...", InteractionRole.System);

            if (invocationParameter1 != null)
            {
                var gameObjectRequest = new FilterableRequestEventArgs(invocationParameter1Description, abortInteraction);

                if (invocationParameter1Filters != null)
                    gameObjectRequest.filters.AddRange(invocationParameter1Filters);

                OnGameObjectRequest(gameObjectRequest);
                return; //Break after the GameObjectRequest in order to avoid two OnGameObjectRequests potentially occuring at once.
            }


            if (invocationParameter2 != null)
            {
                var gameObjectRequest = new FilterableRequestEventArgs(invocationParameter2Description, abortInteraction);

                if (invocationParameter2Filters != null)
                    gameObjectRequest.filters.AddRange(invocationParameter2Filters);

                OnGameObjectRequest(gameObjectRequest);
                return; //Break after the GameObjectRequest in order to avoid two OnGameObjectRequests potentially occuring at once.
            }

            // Invocation continues in GiveRequiredParameter automatically upon the GameObject request being fulfilled.
        }

        /// <summary> Adds each invocation parameter in through this method. Upon all parameters being full, it will invoke automatically.
        /// Requires the sender identity to match the invocation mutex. </summary>
        public override void GiveRequiredParameter(object newParameter, PlayerController sourceInvocationMutexIdentity)
        {
            if (invocationMutexIdentity != sourceInvocationMutexIdentity)
                return; //The sender identity sourceInvocationMutexIdentity did not match the current invocationMutexIdentity");

            if (newParameter == null)
                throw new ArgumentException("newParameter was null!");

            if (invocationParameter1 != null && invocationParameter2 != null)
                throw new Exception("All of the parameters were already filled up! This is not supposed to happen. Had the parameters been reset last time this was partially completed and then cancelled? Were parameters already pre-assigned?");

            //Set invocationParameter3 as the current player.
            invocationParameter3 = sourceInvocationMutexIdentity.CurrentPlayer;

            //Set the first parameter if unset. 
            if (invocationParameter1 == null)
            {
                if (!(newParameter is GameObject))
                    throw new ArgumentException("The first parameter tried to be set but it didn't match the correct type.");

                invocationParameter1 = (GameObject)newParameter;

                //Request the second parameter if it is still required.
                if (invocationParameter2 == null)
                {
                    RequestSecondParameter();
                    return; //Break after the GameObjectRequest in order to avoid two OnGameObjectRequests potentially occuring at once.
                }
            }

            //Set the second parameter if unset.
            if (invocationParameter2 == null)
            {
                if (!(newParameter is GameObject))
                    throw new ArgumentException("The second parameter tried to be set but it didn't match the correct type.");

                invocationParameter2 = (GameObject)newParameter;
            }

            //Once all parameters have been set, invoke and then reset the parameters back to unset.
            Invoke(sourceInvocationMutexIdentity);
        }

        public override void CancelInteraction(PlayerController sourceInvocationMutexIdentity, PropertyContainer sourceContainer)
        {
            if (invocationMutexIdentity != sourceInvocationMutexIdentity)
                return; //The sender identity sourceInvocationMutexIdentity did not match the current invocationMutexIdentity");

            //Reset parameters back to unset
            invocationParameter1 = storedPreassignedParameter1;
            invocationParameter2 = storedPreassignedParameter2;
            invocationParameter3 = null;
            invocationMutexIdentity = null; //Release mutex.
        }

        protected override void Invoke(PlayerController sourceInvocationMutexIdentity)
        {
            if (invocationMutexIdentity != sourceInvocationMutexIdentity)
                return; //The sender identity sourceInvocationMutexIdentity did not match the current invocationMutexIdentity");

            if (invocationParameter1 == null)
                throw new Exception("Tried to invoke, but invocationParameter1, a GameObject, wasnt set.");

            if (invocationParameter2 == null)
                throw new Exception("Tried to invoke, but invocationParameter2, a GameObject, wasnt set.");

            if (invocationParameter3 == null)
                throw new Exception("Tried to invoke, but invocationParameter2, a Player, wasnt set.");

            // invoke
            storedDelegate(sourceInvocationMutexIdentity, AssociatedSource, invocationParameter1, invocationParameter2, invocationParameter3);

            AssociatedSource.Interactions.Remove(this); //Remove self
            invocationMutexIdentity = null; //Release mutex.
        }
        protected virtual void OnGameObjectRequest(FilterableRequestEventArgs gameObjectFilters)
        {
            GameObjectRequest?.Invoke(this, gameObjectFilters);
        }
        private void RequestSecondParameter()
        {
            var gameObjectRequest = new FilterableRequestEventArgs(invocationParameter2Description, abortInteraction);

            if (invocationParameter2Filters != null)
                gameObjectRequest.filters.AddRange(invocationParameter2Filters);

            OnGameObjectRequest(gameObjectRequest); //Request the second Parameter.
        }

        /// <summary> Create a delegate which, unless pre-assigned, will ask the player for two target GameObjects within their location. </summary>
        public delegate void InteractionUsingTwoGameObjectsAndCurrentPlayerDelegate(PlayerController invocationMutexIdentity, PropertyContainer sourceContainer, GameObject firstTarget_GameObject, GameObject secondTarget_GameObject, Player currentPlayer);

        public event EventHandler<FilterableRequestEventArgs> GameObjectRequest;
    }
}
