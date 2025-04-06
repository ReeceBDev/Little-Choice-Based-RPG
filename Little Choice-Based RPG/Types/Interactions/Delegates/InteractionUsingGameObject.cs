using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.TypedEventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Little_Choice_Based_RPG.Types.Interactions.InteractDelegate.InteractionUsingNothing;

namespace Little_Choice_Based_RPG.Types.Interactions.InteractDelegate
{
    /// <summary> Provides a way to present options and choices to the player by exposing a delegate with pre-defined parameters. </summary>
    public class InteractionUsingGameObject : Interaction
    {
        /// <summary> Delegate to be invoked later with Invoke(). </summary>
        private InteractionUsingGameObjectDelegate storedDelegate;

        private GameObject? invocationParameter1 = null;
        private List<EntityProperty>? invocationParameter1Filters = null;
        private string invocationParameter1Description;

        private InteractionUsingNothing abortInteraction;

        static InteractionUsingGameObject()
        {
            InteractionValidation.CreateValidDelegate("InteractUsingTargetObject", [InteractionParameter.Target_GameObject]);
        }
        /// <summary> Creates a new interaction for players to be presented with in ChoiceHandler. </summary>
        public InteractionUsingGameObject(InteractionUsingGameObjectDelegate setDelegate, string setInteractTitle, string setInteractDescriptor, string setGameObjectRequestDescription, InteractionRole setInteractRole = InteractionRole.Explore, List<EntityProperty>? setGameObjectFilter = null)
            : base(setInteractTitle, setInteractDescriptor, setInteractRole)
        {
            storedDelegate = setDelegate;

            invocationParameter1Description = setGameObjectRequestDescription;

            if (setGameObjectFilter != null)
                invocationParameter1Filters = setGameObjectFilter;
        }

        /// <summary> Invokes the delegate, requesting further user input if necessary. </summary>
        public override void AttemptInvoke(IUserInterface sourceInvocationMutexIdentity)
        {
            //Hold mutex if not already held
            if (invocationMutexIdentity == null)
                invocationMutexIdentity = sourceInvocationMutexIdentity; //Hold mutex

            //Check if a held mutex matches the current identity.
            if (invocationMutexIdentity != sourceInvocationMutexIdentity)
                return; //The sender identity sourceInvocationMutexIdentity did not match the current invocationMutexIdentity");

            //reset invocation parameters when first invoking.
            invocationParameter1 = null;

            //Create abort delegate using this invocationMutexIdentity
            InteractionUsingNothingDelegate abortInteractionDelegate = new InteractionUsingNothingDelegate(CancelInteraction);
            abortInteraction = new InteractionUsingNothing(abortInteractionDelegate, "Cancel selection", "Cancelling this interaction...", InteractionRole.System);

            var gameObjectRequest = new FilterableRequestEventArgs(invocationParameter1Description, abortInteraction);

            if (invocationParameter1Filters != null)
                gameObjectRequest.filters.AddRange(invocationParameter1Filters);

            OnGameObjectRequest(gameObjectRequest);

            // Invocation continues in AddInvocationParameter upon the GameObject parameter being filled.
        }

        /// <summary> Adds each invocation parameter in through this method. Upon all parameters being set, it will invoke automatically.
        /// Requires the sender identity to match the invocation mutex. </summary>
        public override void GiveRequiredParameter(object newParameter, IUserInterface sourceInvocationMutexIdentity)
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
        public override void CancelInteraction(IUserInterface sourceInvocationMutexIdentity)
        {
            if (invocationMutexIdentity != sourceInvocationMutexIdentity)
                return; //The sender identity sourceInvocationMutexIdentity did not match the current invocationMutexIdentity");

            //Reset parameters back to unset
            invocationParameter1 = null;
            invocationMutexIdentity = null; //Release mutex.
        }

        protected override void Invoke(IUserInterface sourceInvocationMutexIdentity)
        {
            if (invocationMutexIdentity != sourceInvocationMutexIdentity)
                return; //The sender identity sourceInvocationMutexIdentity did not match the current invocationMutexIdentity");

            if (invocationParameter1 == null)
                throw new Exception("Tried to invoke, but invocationParameter1 wasnt set.");

            // invoke
            storedDelegate(sourceInvocationMutexIdentity, invocationParameter1);

            //Reset parameters back to unset
            invocationParameter1 = null;
            invocationMutexIdentity = null; //Release mutex.
        }

        protected virtual void OnGameObjectRequest(FilterableRequestEventArgs gameObjectFilters)
        {
            GameObjectRequest?.Invoke(this, gameObjectFilters);
        }

        /// <summary> Create a delegate which will ask the player for a target GameObject within their location. </summary>
        public delegate void InteractionUsingGameObjectDelegate(IUserInterface invocationMutexIdentity, GameObject target_GameObject);

        public event EventHandler<FilterableRequestEventArgs> GameObjectRequest;
    }
}
