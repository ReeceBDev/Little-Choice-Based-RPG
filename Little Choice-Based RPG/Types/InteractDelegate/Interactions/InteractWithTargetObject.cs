using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.InteractDelegate.InteractDelegates
{
    /// <summary> Provides a way to present options and choices to the player by exposing a delegate with pre-defined parameters. </summary>
    public class InteractWithTargetObject : InteractDelegate
    {
        /// <summary> Create a delegate which will ask the player for a target GameObject within their location. </summary>
        public delegate void InteractUsingTargetObject(GameObject target_GameObject);

        /// <summary> Stores the delegate to be invoked later with Invoke(). </summary>
        public InteractUsingTargetObject storedDelegate;

        static InteractWithTargetObject()
        {
            InteractionValidation.CreateValidDelegate("InteractUsingTargetObject", [InteractionParameter.Target_GameObject]);

        }
        /// <summary> Creates a new interaction for players to be presented with in ChoiceHandler. </summary>
        public InteractWithTargetObject(InteractUsingTargetObject setDelegate, string setInteractTitle, string setInteractDescriptor, InteractionContext setInteractRole = InteractionContext.Explore)
            : base(setInteractTitle, setInteractDescriptor, setInteractRole)
        {
            storedDelegate = setDelegate;
        }

        /// <summary> Invokes the delegate using its required parameters. </summary>
        public override void Invoke()
        {
            GameObject target_GameObject = InteractionParameterHandler.GetParameter<GameObject>(InteractionParameter.Target_GameObject);
            storedDelegate(target_GameObject);
        }
    }
}
