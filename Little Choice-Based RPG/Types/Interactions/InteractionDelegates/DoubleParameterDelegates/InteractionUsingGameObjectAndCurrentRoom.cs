using Little_Choice_Based_RPG.External.Types;
using Little_Choice_Based_RPG.Managers.PlayerControl;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.TypedEventArgs;
using static Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.InteractionUsingNothing;

namespace Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.DoubleParameterDelegates
{
    /// <summary> Provides a way to present options and choices to the player by exposing a delegate with pre-defined parameters. </summary>
    internal class InteractionUsingGameObjectAndCurrentRoom : Interaction
    {
        /// <summary> Delegate to be invoked later with Invoke(). </summary>
        public InteractionUsingGameObjectAndCurrentRoomDelegate storedDelegate;

        private GameObject? storedPresassignedParameter1 = null;
        private GameObject? invocationParameter1 = null;
        private List<EntityProperty>? invocationParameter1Filters = null;
        private string invocationParameter1Description;

        private Room? invocationParameter2 = null;

        private InteractionUsingNothing abortInteraction;

        static InteractionUsingGameObjectAndCurrentRoom()
        {
            //InteractionValidation.CreateValidDelegate("InteractUsingTargetObject", [InteractionParameter.Target_GameObject]);
        }
        /// <summary> Creates a new interaction for players to be presented with in ChoiceHandler. Provides the player's currentroom upon invocation as the Room. Requests a GameObject from the player. </summary>
        public InteractionUsingGameObjectAndCurrentRoom(InteractionUsingGameObjectAndCurrentRoomDelegate setDelegate, uint? setAssociatedObjectID, string setInteractTitle, string setInteractDescriptor, string setGameObjectRequestDescription, List<EntityProperty>? setGameObjectFilter = null, InteractionRole setInteractRole = InteractionRole.Explore)
            : base(setDelegate, setInteractTitle, setInteractDescriptor, setAssociatedObjectID, setInteractRole)
        {
            storedDelegate = setDelegate;

            invocationParameter1Description = setGameObjectRequestDescription;

            if (setGameObjectFilter != null)
                invocationParameter1Filters = setGameObjectFilter;
        }

        /// <summary> Creates a new interaction for players to be presented with in ChoiceHandler. Provides the player's currentroom upon invocation as the Room. Provides a Pre-assigned GameObject.</summary>
        public InteractionUsingGameObjectAndCurrentRoom(InteractionUsingGameObjectAndCurrentRoomDelegate setDelegate, uint? setAssociatedObjectID, string setInteractTitle, string setInteractDescriptor, GameObject preassignedParameter1, InteractionRole setInteractRole = InteractionRole.Explore)
            : base(setDelegate, setInteractTitle, setInteractDescriptor, setAssociatedObjectID, setInteractRole)
        {
            storedDelegate = setDelegate;

            //Set invocationParameter1 to preassignedParameter1 and save the preassignment in case of cancellation:
            invocationParameter1 = preassignedParameter1;
            storedPresassignedParameter1 = preassignedParameter1;
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

            //Set invocationParameter2 as the player's CurrentRoom.
            invocationParameter2 = sourceInvocationMutexIdentity.CurrentRoom;

            //If all parameters are already filled, invoke
            if (invocationParameter1 != null && invocationParameter2 != null)
            {
                Invoke(sourceInvocationMutexIdentity);
                return;
            }

            //Otherwise, request the missing parameter:

            //Create abort delegate using this invocationMutexIdentity
            InteractionUsingNothingDelegate abortInteractionDelegate = new InteractionUsingNothingDelegate(ResetInteraction);
            abortInteraction = new InteractionUsingNothing(abortInteractionDelegate, (uint)sourceInvocationMutexIdentity.CurrentPlayer.Properties.GetPropertyValue("ID"), "Cancel selection", "Cancelling this interaction...", InteractionRole.System);

            var gameObjectRequest = new FilterableRequestEventArgs(invocationParameter1Description, abortInteraction);

            if (invocationParameter1Filters != null)
                gameObjectRequest.filters.AddRange(invocationParameter1Filters);

            OnGameObjectRequest(gameObjectRequest);

            // Invocation continues in GiveRequiredParameter automatically upon the GameObject request being fulfilled.
        }

        /// <summary> Adds each invocation parameter in through this method. Upon all parameters being set, it will invoke automatically.
        /// Requires the sender identity to match the invocation mutex. </summary>
        public override void GiveRequiredParameter(object newParameter, PlayerController sourceInvocationMutexIdentity)
        {
            if (invocationMutexIdentity != sourceInvocationMutexIdentity)
                return; //The sender identity sourceInvocationMutexIdentity did not match the current invocationMutexIdentity");

            if (newParameter == null)
                throw new ArgumentException("newParameter was null!");

            if (invocationParameter1 != null)
                throw new Exception("All of the parameters were already filled up! This is not supposed to happen. Had the parameters been reset in this.AttemptInvoke?");

            //Set the first parameter if unset.
            if (invocationParameter1 == null)
            {
                if (!(newParameter is GameObject))
                    throw new ArgumentException("The first parameter tried to be set but it didn't match the correct type.");

                invocationParameter1 = (GameObject)newParameter;
            }

            //Once all parameters have been set, invoke and then reset the parameters back to unset.
            Invoke(sourceInvocationMutexIdentity);
        }
        public override void ResetInteraction(PlayerController sourceInvocationMutexIdentity)
        {
            if (invocationMutexIdentity != sourceInvocationMutexIdentity)
                return; //The sender identity sourceInvocationMutexIdentity did not match the current invocationMutexIdentity");

            //Reset parameters back to unset
            invocationParameter1 = storedPresassignedParameter1;
            invocationParameter2 = null;
            invocationMutexIdentity = null; //Release mutex.
        }

        protected override void Invoke(PlayerController sourceInvocationMutexIdentity)
        {
            if (invocationMutexIdentity != sourceInvocationMutexIdentity)
                return; //The sender identity sourceInvocationMutexIdentity did not match the current invocationMutexIdentity");

            if (invocationParameter1 == null)
                throw new Exception("Tried to invoke, but invocationParameter1 wasnt set.");

            if (invocationParameter2 == null)
                throw new Exception("Tried to invoke, but invocationParameter2 wasnt set.");

            // invoke
            storedDelegate(sourceInvocationMutexIdentity, invocationParameter1, invocationParameter2);

            ResetInteraction(sourceInvocationMutexIdentity);
        }

        protected virtual void OnGameObjectRequest(FilterableRequestEventArgs gameObjectFilters)
        {
            GameObjectRequest?.Invoke(this, gameObjectFilters);
        }

        /// <summary> Create a delegate which, unless pre-assigned, will ask the player for a target GameObject within their location. CurrentRoom is assigned automatically upon invocation.</summary>
        public delegate void InteractionUsingGameObjectAndCurrentRoomDelegate(PlayerController invocationMutexIdentity, GameObject target_GameObject, Room current_Room);

        public event EventHandler<FilterableRequestEventArgs> GameObjectRequest;
    }
}
