using Little_Choice_Based_RPG.Resources.Entities;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PrivateInteractionsSystems
{
    /// <summary> Represents a pending private interaction change. Unlike public state changes, private state changes use interaction KVPs which take both an interaction and its subject.
    /// This allows the subject an interaction relates to, to later be identified. The subject is used when removing interactions from the private list. </summary>
    internal readonly record struct PrivateInteractionStateChange(KeyValuePair<PropertyContainer, IInvokableInteraction> InteractionKVP, bool IsAvailable, long TimeStamp);
}
