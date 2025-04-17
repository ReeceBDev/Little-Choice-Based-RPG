using Little_Choice_Based_RPG.Managers.Player_Manager;
using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface;
using Little_Choice_Based_RPG.Resources;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.Interactions.InteractDelegate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.Interactions.Delegates
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
        public InteractionUsingNothing(InteractionUsingNothingDelegate setDelegate, PropertyContainer setSourceContainer, string setInteractTitle, string setInteractDescriptor, InteractionRole setInteractRole = InteractionRole.Explore)
            : base(setSourceContainer, setInteractTitle, setInteractDescriptor, setInteractRole)
        {
            storedDelegate = setDelegate;
        }

        /// <summary> Invokes the delegate if able. Requests required parameters if unable. </summary>
        public override void AttemptInvoke(PlayerController sourceInvocationMutexIdentity)
        {
            //Hold mutex if not already held
            if (invocationMutexIdentity == null)
                invocationMutexIdentity = sourceInvocationMutexIdentity; //Hold mutex

            //Check if a held mutex matches the current identity.
            if (invocationMutexIdentity != sourceInvocationMutexIdentity)
                return; //The sender identity sourceInvocationMutexIdentity did not match the current invocationMutexIdentity");

            Invoke(sourceInvocationMutexIdentity);
        }

        public override void GiveRequiredParameter(object newParameter, PlayerController sourceInvocationMutexIdentity)
        {
            // Not needed here since there are no parameters.
            // It is still required for the interface to support the Interactions with parameters.
        }

        protected override void Invoke(PlayerController sourceInvocationMutexIdentity)
        {
            if (invocationMutexIdentity == null)
                throw new Exception("The invocationMutexIdentity on this interaction has not been set. There is nothing to compare the sourceInvocationMutexIdentity to!");

            if (invocationMutexIdentity != sourceInvocationMutexIdentity)
                return; //The sender identity sourceInvocationMutexIdentity did not match the current invocationMutexIdentity");

            storedDelegate.Invoke(sourceInvocationMutexIdentity, SourceContainer);

            SourceContainer.Interactions.Remove(this); //Remove self
            invocationMutexIdentity = null; //Release mutex
        }

        public override void CancelInteraction(PlayerController sourceInvocationMutexIdentity, PropertyContainer sourceContainer)
        {
            if (invocationMutexIdentity != sourceInvocationMutexIdentity)
                return; //The sender identity sourceInvocationMutexIdentity did not match the current invocationMutexIdentity");

            //Reset parameters back to unset
            invocationMutexIdentity = null; //Release mutex.
        }

        /// <summary> Create a delegate which uses no additional parameters. </summary>
        public delegate void InteractionUsingNothingDelegate(PlayerController newInvocationMutexIdentity, PropertyContainer sourceContainer);
    }
}
