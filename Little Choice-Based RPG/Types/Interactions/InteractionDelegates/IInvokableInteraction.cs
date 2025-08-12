using Little_Choice_Based_RPG.External.Types;
using Little_Choice_Based_RPG.Managers.PlayerControl;

namespace Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates
{
    /// <summary> Provides a point to invoke delegates inheriting the abstract class InteractDelegate. </summary>
    internal interface IInvokableInteraction
    {
        /// <summary> The title shown when a player gets listed their choice options. </summary>
        public string InteractionTitle { get; init; }

        /// <summary> The descriptor shown after a player selected their choice. </summary>
        public string InteractDescriptor { get; init; }

        /// <summary> Describes how an Interaction should be presented by the User Interface, for example, if it belongs to a context-menu. </summary>
        public InteractionRole InteractionContext { get; init; }

        /// <summary> Represents the method being invoked, in the specific way designated by delegates' type. 
        /// Only used for equivalence. Mirrors the delegate in the derived class.</summary>
        public Delegate DelegateRecord { get; init; }

        /// <summary> Allocates an identity to one specific instance. Must be unique. Should not be used in equivalence. </summary>
        public ulong UniqueInstanceID { get; init; }

        /// <summary> Invokes the delegate using its required parameters. </summary>
        public void AttemptInvoke(PlayerController sourceInvocationMutexIdentity);
        public abstract void ResetInteraction(PlayerController sourceInvocationMutexIdentity);
        public void GiveRequiredParameter(object newParameter, PlayerController sourceInvocationMutexIdentity);
    }
}
