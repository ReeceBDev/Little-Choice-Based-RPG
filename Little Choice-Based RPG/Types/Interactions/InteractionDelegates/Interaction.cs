using Little_Choice_Based_RPG.Managers.PlayerControl;

namespace Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates
{
    /// <summary> Provides a way to present options and choices to the player by exposing a delegate with pre-defined parameters. </summary>
    public abstract class Interaction : IInvokableInteraction
    {
        static private ulong UniqueInstanceIDCounter;

        protected PlayerController? invocationMutexIdentity;

        /// <summary> Allocates an identity to one specific instance. Must be unique. Should not be used in equivalence. </summary>
        public ulong UniqueInstanceID { get; init; } = Interlocked.Increment(ref UniqueInstanceIDCounter);

        /// <summary> The title shown when a player gets listed their choice options. </summary>
        public string InteractionTitle { get; init; }

        /// <summary> The descriptor shown after a player selected their choice. </summary>
        public string InteractDescriptor { get; init; }

        /// <summary> Describes how an Interaction should be presented by the User Interface, for example, if it belongs to a context-menu. </summary>
        public InteractionRole InteractionContext { get; init; }

        /// <summary> Represents the method being invoked, in the specific way designated by delegates' type. 
        /// Only used for equivalence. Mirrors the delegate in the derived class. </summary>
        public Delegate DelegateRecord { get; init; }

        static Interaction()
        {
            AddSelfIntoDelegateValidation();
        }

        /// <summary> Creates a new interaction for players to be presented with in ChoiceHandler. </summary>
        public Interaction(Delegate setDelegateRecord, string setInteractTitle, string setInteractDescriptor, InteractionRole setInteractRole = InteractionRole.Explore)
        {
            DelegateRecord = setDelegateRecord; // Mirrors the delegate set in the derived class.

            
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
        public abstract void AttemptInvoke(PlayerController sourceInvocationMutexIdentity);
        public abstract void ResetInteraction(PlayerController sourceInvocationMutexIdentity);
        public abstract void GiveRequiredParameter(object newParameter, PlayerController sourceInvocationMutexIdentity);
        protected abstract void Invoke(PlayerController sourceInvocationMutexIdentity);

        public static bool operator !=(Interaction firstTarget, Interaction secondTarget) => !(firstTarget == secondTarget);
        public static bool operator ==(Interaction firstTarget, Interaction secondTarget)
        {
            if (firstTarget is null && secondTarget is null)
                return true;

            if (firstTarget is null && secondTarget is not null)
                return false;

            if (firstTarget is not null && secondTarget is null)
                return false;

            return firstTarget.Equals(secondTarget);
        }
        public override int GetHashCode() 
            => (this.GetType(), DelegateRecord).GetHashCode();
        public override bool Equals(object target) => this.Equals(target as Interaction);
        public bool Equals(Interaction target)
        {
            //Null equivalence
            if (target is null)
                return false;

            //Reference equivalence
            if (Object.ReferenceEquals(this, target))
                return true; //Returns true early, for optimisation.

            //Derived type equivalence
            if (this.GetType() != target.GetType())
                return false;

            //Delegate equivalence
            if (this.DelegateRecord != target.DelegateRecord)
                return false;

            //Note:
            //Explicitly avoid invocationMutexIdentity, InteractionTitle and InteractionDescriptor checks as these
            //s may change, yet the delegate should still be the same.
            //(InteractionTitle and InteractionDescriptor are transient properties from the object that represent a snapshot in time)
            //(Whereas this delegate is a way of delivering a specific interaction to a specific object in a specific way.)
            //(And as such, that delivery is what should be compared, not the title or descriptor!)

            return true;
        }

        /// <summary> 
        /// The PropertyContainer that invoking is performed on, or whatever PropertyContainer the delegate relates to the most. 
        /// If this delegate gets stored on a GameObject, then AssociatedSource is that GameObject. 
        /// If this is stored privately on a player, it is instead the target object that it would have effected. 
        /// This value may reference a player only if the player is directly effected by the system.
        /// This value may reference a room only if it is specifically the room which is the main target of operation.
        /// (Such as in en-masse operations that make sweeping changes to a room.)
        /// </summary>
        /// 
        //Where is this?  I must've removed it. In fact, I think I removed it from the IInvokableInteraction, too, since there was a similar comment. Might re-add.
    }
}
