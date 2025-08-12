using Little_Choice_Based_RPG.External.Types.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.External.Types.TypedEventArgs.InteractionService
{
    public class InteractionServiceRemovalEventArgs(ulong setInteractionID) : EventArgs
    {
        public ulong OldInteractionID { get; } = setInteractionID;
    }
}
