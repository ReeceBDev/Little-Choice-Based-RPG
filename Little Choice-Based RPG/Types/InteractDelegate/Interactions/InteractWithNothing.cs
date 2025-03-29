using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.InteractDelegate.InteractDelegates
{
    /// <summary> Provides a way to present options and choices to the player by exposing a delegate with pre-defined parameters. </summary>
    public class InteractWithNothing : InteractDelegate
    {
        /// <summary> Create a delegate which uses no additional parameters. </summary>
        public delegate void InteractUsingNothing();

        /// <summary> Stores the delegate to be invoked later with Invoke(). </summary>
        public InteractUsingNothing storedDelegate;

        static InteractWithNothing()
        {
            InteractionValidation.CreateValidDelegate("InteractUsingNothing", []);
        }

        /// <summary> Creates a new interaction for players to be presented with in ChoiceHandler. </summary>
        public InteractWithNothing(InteractUsingNothing setDelegate, string setInteractTitle, string setInteractDescriptor, InteractionContext setInteractRole = InteractionContext.Explore)
            : base(setInteractTitle, setInteractDescriptor, setInteractRole)
        {
            storedDelegate = setDelegate;
        }

        /// <summary> Invokes the delegate using its required parameters. </summary>
        public override void Invoke()
        {
            storedDelegate();
        }
    }
}
