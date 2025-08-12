using Little_Choice_Based_RPG.External.Types.Services;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.TypedEventArgs.InteractionCache
{
    internal class InteractionRemovedEventArgs(IInvokableInteraction setInteraction) : EventArgs
    {
        public IInvokableInteraction OldInteraction { get; init; } = setInteraction;
        public long Timestamp = DateTime.UtcNow.Ticks;
    }
}
