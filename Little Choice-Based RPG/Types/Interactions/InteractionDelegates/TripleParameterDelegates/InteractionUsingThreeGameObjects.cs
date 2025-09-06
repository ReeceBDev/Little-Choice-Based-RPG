using Little_Choice_Based_RPG.External.Types;
using Little_Choice_Based_RPG.Managers.PlayerControl;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.Archive;
using Little_Choice_Based_RPG.Types.TypedEventArgs;
using static Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.InteractionUsingNothing;

namespace Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.TripleParameterDelegates
{
    /// <summary> Provides a way to present options and choices to the player by exposing a delegate with pre-defined parameters. </summary>
    internal class InteractionUsingThreeGameObjects : Interaction
    {
        /// <summary> Delegate to be invoked later with Invoke(). </summary>
        public InteractUsingThreeGameObjectsDelegate storedDelegate;

        private GameObject? storedPreassignedParameter1 = null;
        private GameObject? storedPreassignedParameter2 = null;
        private GameObject? storedPreassignedParameter3 = null;

        private GameObject? invocationParameter1 = null;
        private GameObject? invocationParameter2 = null;
        private GameObject? invocationParameter3 = null;

        private List<EntityProperty>? invocationParameter1Filters = null;
        private List<EntityProperty>? invocationParameter2Filters = null;
        private List<EntityProperty>? invocationParameter3Filters = null;

        private string invocationParameter1Description;
        private string invocationParameter2Description;
        private string invocationParameter3Description;

        private InteractionUsingNothing abortInteraction;

        static InteractionUsingThreeGameObjects()
        {
            //InteractionValidation.CreateValidDelegate("InteractUsingTargetObject", [InteractionParameter.Target_GameObject]);
        }
        /// <summary> Creates a new interaction for players to be presented with in ChoiceHandler. Requests three GameObject from the player. Each GameObjectRequest may be filtered. </summary>
        public InteractionUsingThreeGameObjects(InteractUsingThreeGameObjectsDelegate setDelegate, uint? setAssociatedObjectID, string setInteractTitle, string setInteractDescriptor, string firstRequestDescription, string secondRequestDescription, string thirdRequestDescription, List<EntityProperty>? setFirstGameObjectFilter = null, List<EntityProperty>? setSecondGameObjectFilter = null, List<EntityProperty>? setThirdGameObjectFilter = null, InteractionRole setInteractRole = InteractionRole.Explore)
            : base(setDelegate, setInteractTitle, setInteractDescriptor, setAssociatedObjectID, setInteractRole)
        {
            storedDelegate = setDelegate;

            invocationParameter1Description = firstRequestDescription;
            invocationParameter2Description = secondRequestDescription;
            invocationParameter3Description = thirdRequestDescription;

            if (setFirstGameObjectFilter != null)
                invocationParameter1Filters = setFirstGameObjectFilter;

            if (setSecondGameObjectFilter != null)
                invocationParameter2Filters = setSecondGameObjectFilter;

            if (setThirdGameObjectFilter != null)
                invocationParameter3Filters = setThirdGameObjectFilter;
        }

        /// <summary> Creates a new interaction for players to be presented with in ChoiceHandler. The first GameObject is pre-assigned. Requests a second and third GameObject from the player. The GameObjectRequest may be filtered. </summary>
        public InteractionUsingThreeGameObjects(InteractUsingThreeGameObjectsDelegate setDelegate, uint? setAssociatedObjectID, string setInteractTitle, string setInteractDescriptor, GameObject preassignedParameter1, string secondRequestDescription, string thirdRequestDescription, List<EntityProperty>? setSecondGameObjectFilter = null, List<EntityProperty>? setThirdGameObjectFilter = null, InteractionRole setInteractRole = InteractionRole.Explore)
            : base(setDelegate, setInteractTitle, setInteractDescriptor, setAssociatedObjectID, setInteractRole)
        {
            storedDelegate = setDelegate;

            //Set invocationParameter1 to preassignedParameter1 and save the preassignment in case of cancellation:
            invocationParameter1 = preassignedParameter1;
            storedPreassignedParameter1 = preassignedParameter1;

            invocationParameter2Description = secondRequestDescription;
            invocationParameter3Description = thirdRequestDescription;

            if (setSecondGameObjectFilter != null)
                invocationParameter2Filters = setSecondGameObjectFilter;

            if (setThirdGameObjectFilter != null)
                invocationParameter3Filters = setThirdGameObjectFilter;
        }

        /// <summary> Creates a new interaction for players to be presented with in ChoiceHandler. The second GameObject is pre-assigned. Requests the first and third GameObjects from the player. The GameObjectRequest may be filtered. </summary>
        public InteractionUsingThreeGameObjects(InteractUsingThreeGameObjectsDelegate setDelegate, uint? setAssociatedObjectID, string setInteractTitle, string setInteractDescriptor, string firstRequestDescription, GameObject preassignedParameter2, string thirdRequestDescription, List<EntityProperty>? setFirstGameObjectFilter = null, List<EntityProperty>? setThirdGameObjectFilter = null, InteractionRole setInteractRole = InteractionRole.Explore)
            : base(setDelegate, setInteractTitle, setInteractDescriptor, setAssociatedObjectID, setInteractRole)
        {
            storedDelegate = setDelegate;

            invocationParameter1Description = firstRequestDescription;

            //Set invocationParameter2 to preassignedParameter2 and save the preassignment in case of cancellation:
            invocationParameter2 = preassignedParameter2;
            storedPreassignedParameter2 = preassignedParameter2;

            invocationParameter3Description = thirdRequestDescription;

            if (setFirstGameObjectFilter != null)
                invocationParameter2Filters = setFirstGameObjectFilter;

            if (setThirdGameObjectFilter != null)
                invocationParameter3Filters = setThirdGameObjectFilter;
        }

        /// <summary> Creates a new interaction for players to be presented with in ChoiceHandler. The third GameObject is pre-assigned. Requests the first and second GameObjects from the player. The GameObjectRequest may be filtered. </summary>
        public InteractionUsingThreeGameObjects(InteractUsingThreeGameObjectsDelegate setDelegate, uint? setAssociatedObjectID, string setInteractTitle, string setInteractDescriptor, string firstRequestDescription, string secondRequestDescription, GameObject preassignedParameter3, List<EntityProperty>? setFirstGameObjectFilter = null, List<EntityProperty>? setSecondGameObjectFilter = null, InteractionRole setInteractRole = InteractionRole.Explore)
            : base(setDelegate, setInteractTitle, setInteractDescriptor, setAssociatedObjectID, setInteractRole)
        {
            storedDelegate = setDelegate;

            invocationParameter1Description = firstRequestDescription;
            invocationParameter2Description = secondRequestDescription;

            //Set invocationParameter3 to preassignedParameter3 and save the preassignment in case of cancellation:
            invocationParameter3 = preassignedParameter3;
            storedPreassignedParameter3 = preassignedParameter3;


            if (setFirstGameObjectFilter != null)
                invocationParameter2Filters = setFirstGameObjectFilter;

            if (setSecondGameObjectFilter != null)
                invocationParameter3Filters = setSecondGameObjectFilter;
        }


        /// <summary> Creates a new interaction for players to be presented with in ChoiceHandler. The first and second GameObjects are pre-assigned. Requests third GameObject from the player. The GameObjectRequest may be filtered. </summary>
        public InteractionUsingThreeGameObjects(InteractUsingThreeGameObjectsDelegate setDelegate, uint? setAssociatedObjectID, string setInteractTitle, string setInteractDescriptor, GameObject preassignedParameter1, GameObject preassignedParameter2, string thirdRequestDescription, List<EntityProperty>? setThirdGameObjectFilter = null, InteractionRole setInteractRole = InteractionRole.Explore)
            : base(setDelegate, setInteractTitle, setInteractDescriptor, setAssociatedObjectID, setInteractRole)
        {
            storedDelegate = setDelegate;

            //Set invocationParameter1 to preassignedParameter1 and save the preassignment in case of cancellation:
            invocationParameter1 = preassignedParameter1;
            storedPreassignedParameter1 = preassignedParameter1;

            //Set invocationParameter2 to preassignedParameter2 and save the preassignment in case of cancellation:
            invocationParameter2 = preassignedParameter2;
            storedPreassignedParameter2 = preassignedParameter2;

            invocationParameter3Description = thirdRequestDescription;

            if (setThirdGameObjectFilter != null)
                invocationParameter3Filters = setThirdGameObjectFilter;
        }

        /// <summary> Creates a new interaction for players to be presented with in ChoiceHandler. The first and third GameObjects are pre-assigned. Requests the second GameObject from the player. The GameObjectRequest may be filtered. </summary>
        public InteractionUsingThreeGameObjects(InteractUsingThreeGameObjectsDelegate setDelegate, uint? setAssociatedObjectID, string setInteractTitle, string setInteractDescriptor, GameObject preassignedParameter1, string secondRequestDescription, GameObject preassignedParameter3, List<EntityProperty>? setSecondGameObjectFilter = null, InteractionRole setInteractRole = InteractionRole.Explore)
            : base(setDelegate, setInteractTitle, setInteractDescriptor, setAssociatedObjectID, setInteractRole)
        {
            storedDelegate = setDelegate;

            //Set invocationParameter1 to preassignedParameter1 and save the preassignment in case of cancellation:
            invocationParameter1 = preassignedParameter1;
            storedPreassignedParameter1 = preassignedParameter1;

            invocationParameter2Description = secondRequestDescription;

            //Set invocationParameter3 to preassignedParameter3 and save the preassignment in case of cancellation:
            invocationParameter3 = preassignedParameter3;
            storedPreassignedParameter3 = preassignedParameter3;

            if (setSecondGameObjectFilter != null)
                invocationParameter3Filters = setSecondGameObjectFilter;
        }

        /// <summary> Creates a new interaction for players to be presented with in ChoiceHandler. The second and third GameObjects are pre-assigned. Requests the first GameObject from the player. The GameObjectRequest may be filtered. </summary>
        public InteractionUsingThreeGameObjects(InteractUsingThreeGameObjectsDelegate setDelegate, uint? setAssociatedObjectID, string setInteractTitle, string setInteractDescriptor, string firstRequestDescription, GameObject preassignedParameter2, GameObject preassignedParameter3, List<EntityProperty>? setFirstGameObjectFilter = null, InteractionRole setInteractRole = InteractionRole.Explore)
            : base(setDelegate, setInteractTitle, setInteractDescriptor, setAssociatedObjectID, setInteractRole)
        {
            storedDelegate = setDelegate;

            invocationParameter1Description = firstRequestDescription;

            //Set invocationParameter2 to preassignedParameter2 and save the preassignment in case of cancellation:
            invocationParameter2 = preassignedParameter2;
            storedPreassignedParameter2 = preassignedParameter2;

            //Set invocationParameter3 to preassignedParameter3 and save the preassignment in case of cancellation:
            invocationParameter3 = preassignedParameter3;
            storedPreassignedParameter3 = preassignedParameter3;

            if (setFirstGameObjectFilter != null)
                invocationParameter3Filters = setFirstGameObjectFilter;
        }

        /// <summary> Creates a new interaction for players to be presented with in ChoiceHandler. All three GameObjects are pre-assigned. The GameObjectRequest may be filtered. </summary>
        public InteractionUsingThreeGameObjects(InteractUsingThreeGameObjectsDelegate setDelegate, uint? setAssociatedObjectID, string setInteractTitle, string setInteractDescriptor, GameObject preassignedParameter1, GameObject preassignedParameter2, GameObject preassignedParameter3, InteractionRole setInteractRole = InteractionRole.Explore)
            : base(setDelegate, setInteractTitle, setInteractDescriptor, setAssociatedObjectID, setInteractRole)
        {
            storedDelegate = setDelegate;

            //Set invocationParameter1 to preassignedParameter1 and save the preassignment in case of cancellation:
            invocationParameter1 = preassignedParameter1;
            storedPreassignedParameter1 = preassignedParameter1;

            //Set invocationParameter2 to preassignedParameter2 and save the preassignment in case of cancellation:
            invocationParameter2 = preassignedParameter2;
            storedPreassignedParameter2 = preassignedParameter2;

            //Set invocationParameter3 to preassignedParameter3 and save the preassignment in case of cancellation:
            invocationParameter3 = preassignedParameter3;
            storedPreassignedParameter3 = preassignedParameter3;
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
            if (invocationParameter1 != null && invocationParameter2 != null && invocationParameter3 != null)
            {
                Invoke(sourceInvocationMutexIdentity);
                return;
            }

            //Otherwise, fill each remaining parameter with GameObjectRequests:

            //Create abort delegate using this invocationMutexIdentity
            InteractionUsingNothingDelegate abortInteractionDelegate = new InteractionUsingNothingDelegate(ResetInteraction);
            abortInteraction = new InteractionUsingNothing(abortInteractionDelegate, (uint)sourceInvocationMutexIdentity.CurrentPlayer.Properties.GetPropertyValue("ID"), "Cancel selection", "Cancelling this interaction...", InteractionRole.System);

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

            if (invocationParameter3 != null)
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

                //Request the third parameter if it is still required.
                if (invocationParameter3 == null)
                {
                    RequestThirdParameter();
                    return; //Break after the GameObjectRequest in order to avoid two OnGameObjectRequests potentially occuring at once.
                }
            }

            //Set the second parameter if unset.
            if (invocationParameter2 == null)
            {
                if (!(newParameter is GameObject))
                    throw new ArgumentException("The second parameter tried to be set but it didn't match the correct type.");

                invocationParameter2 = (GameObject)newParameter;

                //Request the third parameter if it is still required.
                if (invocationParameter3 == null)
                {
                    RequestThirdParameter();
                    return; //Break after the GameObjectRequest in order to avoid two OnGameObjectRequests potentially occuring at once.
                }
            }

            //Set the third parameter if unset.
            if (invocationParameter3 == null)
            {
                if (!(newParameter is GameObject))
                    throw new ArgumentException("The third parameter tried to be set but it didn't match the correct type.");

                invocationParameter3 = (GameObject)newParameter;
            }

            //Once all parameters have been set, invoke and then reset the parameters back to unset.
            Invoke(sourceInvocationMutexIdentity);
        }

        public override void ResetInteraction(PlayerController sourceInvocationMutexIdentity)
        {
            if (invocationMutexIdentity != sourceInvocationMutexIdentity)
                return; //The sender identity sourceInvocationMutexIdentity did not match the current invocationMutexIdentity");

            //Reset parameters back to unset
            invocationParameter1 = storedPreassignedParameter1;
            invocationParameter2 = storedPreassignedParameter2;
            invocationParameter3 = storedPreassignedParameter3;
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

            if (invocationParameter3 == null)
                throw new Exception("Tried to invoke, but invocationParameter3 wasnt set.");

            // invoke
            storedDelegate(sourceInvocationMutexIdentity, invocationParameter1, invocationParameter2, invocationParameter3);
            ResetInteraction(sourceInvocationMutexIdentity);
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

        private void RequestThirdParameter()
        {
            var gameObjectRequest = new FilterableRequestEventArgs(invocationParameter3Description, abortInteraction);

            if (invocationParameter3Filters != null)
                gameObjectRequest.filters.AddRange(invocationParameter3Filters);

            OnGameObjectRequest(gameObjectRequest); //Request the third Parameter.
        }

        /// <summary> Create a delegate which, unless pre-assigned, will ask the player for up to three target GameObjects within their location. </summary>
        public delegate void InteractUsingThreeGameObjectsDelegate(PlayerController invocationMutexIdentity, GameObject first_GameObject, GameObject second_GameObject, GameObject third_GameObject);

        public event EventHandler<FilterableRequestEventArgs> GameObjectRequest;
    }
}
