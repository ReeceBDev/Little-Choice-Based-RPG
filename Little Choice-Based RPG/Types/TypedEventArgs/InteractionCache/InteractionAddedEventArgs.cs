using Little_Choice_Based_RPG.External.Types.Services;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.TypedEventArgs.InteractionCache
{
    internal class InteractionAddedEventArgs(IInvokableInteraction setNewInteraction) : EventArgs
    {
        public IInvokableInteraction NewInteraction { get; init; } = setNewInteraction;
        public long Timestamp = DateTime.UtcNow.Ticks;
    }
}