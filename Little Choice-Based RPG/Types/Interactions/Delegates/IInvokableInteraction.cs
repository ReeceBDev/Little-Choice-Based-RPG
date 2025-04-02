﻿using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.Interactions.InteractDelegate
{
    /// <summary> Provides a point to invoke delegates inheriting the abstract class InteractDelegate. </summary>
    public interface IInvokableInteraction
    {
        /// <summary> Invokes the delegate using its required parameters. </summary>
        public void AttemptInvoke(IUserInterface setInvocationMutexIdentity);
        public void CancelInteraction(IUserInterface setInvocationMutexIdentity);
        public void GiveRequiredParameter(object newParameter, IUserInterface setInvocationMutexIdentity);

        /// <summary> The title shown when a player gets listed their choice options. </summary>
        public SanitizedString InteractionTitle { get; init; }

        /// <summary> The descriptor shown after a player selected their choice. </summary>
        public SanitizedString InteractDescriptor { get; init; }

        /// <summary> Describes how an Interaction should be presented by the User Interface, for example, if it belongs to a context-menu. </summary>
        public InteractionRole InteractionContext { get; init; }
    }
}
