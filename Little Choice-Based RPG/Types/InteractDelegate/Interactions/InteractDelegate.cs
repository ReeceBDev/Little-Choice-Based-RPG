using Little_Choice_Based_RPG.Resources.Choices;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.EntityProperty;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Little_Choice_Based_RPG.Types.InteractDelegate.InteractDelegates
{
    /// <summary> Provides a way to present options and choices to the player by exposing a delegate with pre-defined parameters. </summary>
    public abstract class InteractDelegate : IInvokableInteraction
    {
        static InteractDelegate()
        {
            AddSelfIntoDelegateValidation();
        }

        /// <summary> Creates a new interaction for players to be presented with in ChoiceHandler. </summary>
        public InteractDelegate(string setInteractTitle, string setInteractDescriptor, InteractionContext setInteractRole = InteractionContext.Explore)
        {
            InteractionRole = setInteractRole;
            InteractionTitle.Value = setInteractTitle;
            InteractDescriptor.Value = setInteractDescriptor;
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

        /// <summary> Invokes the delegate using its required parameters. </summary>
        public abstract void Invoke();


        /// <summary> The title shown when a player gets listed their choice options. </summary>
        public SanitizedString InteractionTitle { get; private protected set; } = new SanitizedString(string.Empty);

        /// <summary> The descriptor shown after a player selected their choice. </summary>
        public SanitizedString InteractDescriptor { get; private protected set; } = new SanitizedString(string.Empty);

        /// <summary> Describes how an Interaction should be presented by the User Interface, for example, if it belongs to a context-menu. </summary>
        public InteractionContext InteractionRole { get; private protected set; }
    }
}
