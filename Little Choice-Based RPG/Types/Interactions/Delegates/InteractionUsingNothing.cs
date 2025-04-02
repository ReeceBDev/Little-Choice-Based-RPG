using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.Interactions.InteractDelegate
{
    /// <summary> Provides a way to present options and choices to the player by exposing a delegate with pre-defined parameters. </summary>
    public class InteractionUsingNothing : Interaction
    {
         /// <summary> Stores the delegate to be invoked later with Invoke(). </summary>
        public InteractionUsingNothingDelegate storedDelegate;

        static InteractionUsingNothing()
        {
            InteractionValidation.CreateValidDelegate("InteractUsingNothing", []);
        }

        /// <summary> Creates a new interaction for players to be presented with in ChoiceHandler. </summary>
        public InteractionUsingNothing(InteractionUsingNothingDelegate setDelegate, string setInteractTitle, string setInteractDescriptor, InteractionRole setInteractRole = InteractionRole.Explore)
            : base(setInteractTitle, setInteractDescriptor, setInteractRole)
        {
            storedDelegate = setDelegate;
        }

        /// <summary> Invokes the delegate if able. Requests required parameters if unable. </summary>
        public override void AttemptInvoke(IUserInterface setInvocationMutexIdentity)
        {
            if (invocationMutexIdentity != setInvocationMutexIdentity)
                return; //The sender identity setInvocationMutexIdentity did not match the current invocationMutexIdentity");

            invocationMutexIdentity = setInvocationMutexIdentity; //Hold mutex

            Invoke(setInvocationMutexIdentity);
        }

        public override void GiveRequiredParameter(object newParameter, IUserInterface setInvocationMutexIdentity)
        {
            // Not needed here since there are no parameters.
            // It is still required for the interface to support the Interactions with parameters.
        }

        protected override void Invoke(IUserInterface setInvocationMutexIdentity)
        {
            if (invocationMutexIdentity != setInvocationMutexIdentity)
                return; //The sender identity setInvocationMutexIdentity did not match the current invocationMutexIdentity");

            storedDelegate.Invoke(setInvocationMutexIdentity);

            invocationMutexIdentity = null; //Release mutex
        }

        public override void CancelInteraction(IUserInterface setInvocationMutexIdentity)
        {
            if (invocationMutexIdentity != setInvocationMutexIdentity)
                return; //The sender identity setInvocationMutexIdentity did not match the current invocationMutexIdentity");

            //Reset parameters back to unset
            invocationMutexIdentity = null; //Release mutex.
        }

        /// <summary> Create a delegate which uses no additional parameters. </summary>
        public delegate void InteractionUsingNothingDelegate(IUserInterface invocationMutexIdentity);
    }
}
