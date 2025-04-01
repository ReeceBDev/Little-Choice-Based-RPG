using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.EntityProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.Interactions.InteractDelegate
{
    public class GameObjectRequestEventArgs : EventArgs
    {
        public List<EntityProperty> filters { get; set; }
    }

    /// <summary> Provides a way to present options and choices to the player by exposing a delegate with pre-defined parameters. </summary>
    public class InteractionWithTargetObject : Interaction
    {
         /// <summary> Create a delegate which will ask the player for a target GameObject within their location. </summary>
        public delegate void InteractUsingTargetObject(GameObject target_GameObject);

        /// <summary> Stores the delegate to be invoked later with Invoke(). </summary>
        public InteractUsingTargetObject storedDelegate;

        static InteractionWithTargetObject()
        {
            InteractionValidation.CreateValidDelegate("InteractUsingTargetObject", [InteractionParameter.Target_GameObject]);

        }
        /// <summary> Creates a new interaction for players to be presented with in ChoiceHandler. </summary>
        public InteractionWithTargetObject(InteractUsingTargetObject setDelegate, string setInteractTitle, string setInteractDescriptor, InteractionRole setInteractRole = InteractionRole.Explore)
            : base(setInteractTitle, setInteractDescriptor, setInteractRole)
        {
            storedDelegate = setDelegate;
        }

        /// <summary> Invokes the delegate using its required parameters. </summary>
        public override void Invoke()
        {
            EntityProperty testFilter = new EntityProperty("TestFilter", true);
            GameObjectRequestEventArgs testArgs = new GameObjectRequestEventArgs();
            testArgs.filters.Add(testFilter);

            OnGameObjectRequest(testArgs);
            GameObject target_GameObject = ;

            storedDelegate(target_GameObject);
        }
        public event EventHandler<GameObjectRequestEventArgs> GameObjectRequest;
        protected virtual void OnGameObjectRequest(GameObjectRequestEventArgs gameObjectFilter)
        {
            GameObjectRequest?.Invoke(this, gameObjectFilter);
            GameObjectRequest(this, gameObjectFilter);
        }
    }
}
