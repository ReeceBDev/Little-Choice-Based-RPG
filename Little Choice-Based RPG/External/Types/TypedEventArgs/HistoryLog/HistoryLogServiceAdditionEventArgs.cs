using Little_Choice_Based_RPG.External.Types.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.External.Types.TypedEventArgs.HistoryLog
{
    public class HistoryLogServiceAdditionEventArgs(HistoryLogServiceData setNewLogData) : EventArgs
    {
        public HistoryLogServiceData NewLogEntry { get; init; } = setNewLogData;
    }
}
