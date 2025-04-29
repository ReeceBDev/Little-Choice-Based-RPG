using Little_Choice_Based_RPG.Managers.Player_Manager;
using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface;
using Little_Choice_Based_RPG.Resources;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.TypedEventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.InteractionUsingNothing;

namespace Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.TripleParameterDelegates
{
    /// <summary> Provides a way to present options and choices to the player by exposing a delegate with pre-defined parameters. </summary>
    public class InteractionUsingCurrentPlayerAndCurrentRoomAndPreassignedRoom : Interaction
    {
        /// <summary> Delegate to be invoked later with Invoke(). </summary>
        public InteractionUsingCurrentPlayerAndCurrentRoomAndPreassignedRoomDelegate storedDelegate;

        private Player? invocationParameter1 = null;
        
        private Room? invocationParameter2 = null;

        private Room? invocationParameter3 = null;

        private InteractionUsingNothing abortInteraction;

        static InteractionUsingCurrentPlayerAndCurrentRoomAndPreassignedRoom()
        {
            //InteractionValidation.CreateValidDelegate("InteractUsingTargetObject", [InteractionParameter.Target_GameObject]);
        }
        /// <summary> Creates a new interaction for players to be presented with in ChoiceHandler. Requests a GameObject from the player. </summary>
        public InteractionUsingCurrentPlayerAndCurrentRoomAndPreassignedRoom(InteractionUsingCurrentPlayerAndCurrentRoomAndPreassignedRoomDelegate setDelegate, string setInteractTitle, string setInteractDescriptor, Room setPredefinedRoom, InteractionRole setInteractRole = InteractionRole.Explore)
            : base(setDelegate, setInteractTitle, setInteractDescriptor, setInteractRole)
        {
            storedDelegate = setDelegate;

            if (setPredefinedRoom is null)
                throw new Exception("This interaction requires a Pre-Defined room! This must be set where instantiated!");

            //Set invocationParameter2 as the predefined Room.
            invocationParameter2 = setPredefinedRoom;
        }

        /// <summary> Invokes the delegate, requesting further user input if necessary. </summary>
        public override void AttemptInvoke(PlayerController sourceInvocationMutexIdentity)
        {
            //Hold mutex if not already held
            if (invocationMutexIdentity == null)
                invocationMutexIdentity = sourceInvocationMutexIdentity; //Hold mutex

            //Check if a held mutex matches the current identity.
            if (invocationMutexIdentity != sourceInvocationMutexIdentity)
                return; //The sender identity sourceInvocationMutexIdentity did not match the current invocationMutexIdentity");


            //Set invocationParameter1 as the CurrentPlayer.
            invocationParameter1 = sourceInvocationMutexIdentity.CurrentPlayer;

            //Set invocationParameter3 as the player's CurrentRoom.
            invocationParameter3 = sourceInvocationMutexIdentity.CurrentRoom;

            //If all requestable parameters are already filled, invoke
            if (invocationParameter1 != null)
            {
                Invoke(sourceInvocationMutexIdentity);
                return;
            }
            else
                throw new Exception("Tried to invoke a fully pre-defined interaction, but didn't have all the required Parameters?! Something went seriously wrong!");
        }

        /// <summary> Adds each invocation parameter in through this method. Upon all parameters being set, it will invoke automatically.
        /// Requires the sender identity to match the invocation mutex. </summary>
        public override void GiveRequiredParameter(object newParameter, PlayerController sourceInvocationMutexIdentity)
            => throw new Exception("Tried to GiveRequiredParameter on a fully pre-defined interaction! Something went seriously wrong!");

        public override void ResetInteraction(PlayerController sourceInvocationMutexIdentity)
        {
            if (invocationMutexIdentity != sourceInvocationMutexIdentity)
                return; //The sender identity sourceInvocationMutexIdentity did not match the current invocationMutexIdentity");

            //This interaction is fully pre-defined so no parameters need to be reset, ever!
            invocationMutexIdentity = null; //Release mutex.
        }

        protected override void Invoke(PlayerController sourceInvocationMutexIdentity)
        {
            if (invocationMutexIdentity != sourceInvocationMutexIdentity)
                return; //The sender identity sourceInvocationMutexIdentity did not match the current invocationMutexIdentity");

            if (invocationParameter1 == null)
                throw new Exception("Tried to invoke, but invocationParameter1, a Player, wasnt set.");

            if (invocationParameter2 == null)
                throw new Exception("Tried to invoke, but invocationParameter2, a Room, wasnt set.");

            if (invocationParameter3 == null)
                throw new Exception("Tried to invoke, but invocationParameter3, a Room, wasnt set.");

            // invoke
            storedDelegate(sourceInvocationMutexIdentity, invocationParameter1, invocationParameter2, invocationParameter3);
            ResetInteraction(sourceInvocationMutexIdentity);
        }

        protected virtual void OnGameObjectRequest(FilterableRequestEventArgs gameObjectFilters)
        {
            GameObjectRequest?.Invoke(this, gameObjectFilters);
        }

        /// <summary> Create a delegate which, unless pre-assigned, will ask the player for a target GameObject within their location. CurrentPlayer and CurrentRoom are provided upon invocation. </summary>
        public delegate void InteractionUsingCurrentPlayerAndCurrentRoomAndPreassignedRoomDelegate(PlayerController invocationMutexIdentity, Player currentPlayer, Room currentRoom, Room targetRoom);

        public event EventHandler<FilterableRequestEventArgs> GameObjectRequest;
    }
}
