﻿using Little_Choice_Based_RPG.Managers.PlayerControl;

namespace Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.SingleParameterDelegates
{
    /// <summary> Provides a way to present options and choices to the player by exposing a delegate with pre-defined parameters. </summary>
    public class SystemInteractionUsingCurrentPlayerController : Interaction
    {
        private PlayerController? invocationParameter1 = null;

        /// <summary> Stores the delegate to be invoked later with Invoke(). </summary>
        public SystemInteractionUsingCurrentPlayerControllerDelegate storedDelegate;

        static SystemInteractionUsingCurrentPlayerController()
        {
            //InteractionValidation.CreateValidDelegate("InteractUsingNothing", []);
        }

        /// <summary> Creates a new interaction for players to be presented with in ChoiceHandler. </summary>
        public SystemInteractionUsingCurrentPlayerController(SystemInteractionUsingCurrentPlayerControllerDelegate setDelegate, string setInteractTitle, string setInteractDescriptor, InteractionRole setInteractRole = InteractionRole.Explore)
            : base(setDelegate, setInteractTitle, setInteractDescriptor, setInteractRole)
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

            //Set invocationParameter1 as the current player.
            invocationParameter1 = sourceInvocationMutexIdentity;

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

            if (invocationParameter1 == null)
                throw new Exception("Tried to invoke, but invocationParameter1, a PlayerController, wasnt set.");

            storedDelegate.Invoke(invocationParameter1);
            ResetInteraction(sourceInvocationMutexIdentity);
        }

        public override void ResetInteraction(PlayerController sourceInvocationMutexIdentity)
        {
            if (invocationMutexIdentity != sourceInvocationMutexIdentity)
                return; //The sender identity sourceInvocationMutexIdentity did not match the current invocationMutexIdentity");

            //Reset parameters back to unset
            invocationParameter1 = null;
            invocationMutexIdentity = null; //Release mutex.
        }

        /// <summary> Create a delegate which uses no additional parameters. </summary>
        public delegate void SystemInteractionUsingCurrentPlayerControllerDelegate(PlayerController currentPlayer);
    }
}
