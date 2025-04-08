using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface;
using Little_Choice_Based_RPG.Resources;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.EntityProperties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Little_Choice_Based_RPG.Types.Interactions.InteractDelegate
{
    /// <summary> Provides a way to present options and choices to the player by exposing a delegate with pre-defined parameters. </summary>
    public abstract class Interaction : IInvokableInteraction
    {
        protected IUserInterface? invocationMutexIdentity;

        static Interaction()
        {
            AddSelfIntoDelegateValidation();
        }

        /// <summary> Creates a new interaction for players to be presented with in ChoiceHandler. </summary>
        public Interaction(PropertyContainer setSourceContainer, string setInteractTitle, string setInteractDescriptor, InteractionRole setInteractRole = InteractionRole.Explore)
        {
            SourceContainer = setSourceContainer;
            InteractionContext = setInteractRole;
            InteractionTitle = setInteractTitle;
            InteractDescriptor = setInteractDescriptor;
        }

        private static void AddSelfIntoDelegateValidation()
        {
            /*
        // Use reflection to add these to the list in InteractionValidation. 
        // I can then validate them, too, to see if they match perfectly with the InteractionParameters.
        // Doing it this way around lets the delegates also be strongly typed and already seen by VS :)
        //
        // I CAN ALSO then use reflection to check if all the delegateValidations are handled by ChoiceHandler :)
        i.e. DelegateValidation.CreateValidDelegate("InteractUsingNothing", []);
        i.e. DelegateValidation.CreateValidDelegate("InteractUsingTargetObject", [DelegateParameter.Target_GameObject]);
            */
        }
        /// <summary> Invokes the delegate if able. Requests required parameters if unable. </summary>
        public abstract void AttemptInvoke(IUserInterface sourceInvocationMutexIdentity);
        public abstract void CancelInteraction(IUserInterface sourceInvocationMutexIdentity, PropertyContainer sourceContainer);
        public abstract void GiveRequiredParameter(object newParameter, IUserInterface sourceInvocationMutexIdentity);
        protected abstract void Invoke(IUserInterface sourceInvocationMutexIdentity);

        /// <summary> The originating PropertyContainer </summary>
        public PropertyContainer SourceContainer { get; init; }

        /// <summary> The title shown when a player gets listed their choice options. </summary>
        public string InteractionTitle { get; init; }

        /// <summary> The descriptor shown after a player selected their choice. </summary>
        public string InteractDescriptor { get; init; }

        /// <summary> Describes how an Interaction should be presented by the User Interface, for example, if it belongs to a context-menu. </summary>
        public InteractionRole InteractionContext { get; init; }
    }
}
