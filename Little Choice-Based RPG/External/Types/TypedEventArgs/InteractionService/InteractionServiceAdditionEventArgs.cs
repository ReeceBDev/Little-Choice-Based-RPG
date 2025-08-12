using Little_Choice_Based_RPG.External.Types.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.External.Types.TypedEventArgs.InteractionService
{
    public class InteractionServiceAdditionEventArgs(InteractionServiceData setNewInteractionData) : EventArgs
    {
        public InteractionServiceData NewInteractionData { get; init; } = setNewInteractionData;
    }
}