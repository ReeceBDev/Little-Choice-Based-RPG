﻿using Little_Choice_Based_RPG.Managers.Player_Manager;
using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface;
using Little_Choice_Based_RPG.Resources;
using Little_Choice_Based_RPG.Types.EntityProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates
{
    /// <summary> Provides a point to invoke delegates inheriting the abstract class InteractDelegate. </summary>
    public interface IInvokableInteraction
    {
        /// <summary> Invokes the delegate using its required parameters. </summary>
        public void AttemptInvoke(PlayerController sourceInvocationMutexIdentity);
        public abstract void ResetInteraction(PlayerController sourceInvocationMutexIdentity);
        public void GiveRequiredParameter(object newParameter, PlayerController sourceInvocationMutexIdentity);

        /// <summary> The originating PropertyContainer </summary>
        

        /// <summary> The title shown when a player gets listed their choice options. </summary>
        public string InteractionTitle { get; init; }

        /// <summary> The descriptor shown after a player selected their choice. </summary>
        public string InteractDescriptor { get; init; }

        /// <summary> Describes how an Interaction should be presented by the User Interface, for example, if it belongs to a context-menu. </summary>
        public InteractionRole InteractionContext { get; init; }

        /// <summary> Represents the method being invoked, in the specific way designated by delegates' type. 
        /// Only used for equivalence. Mirrors the delegate in the derived class.</summary>
        public Delegate DelegateRecord { get; init; }
    }
}
