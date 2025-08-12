using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.External.Types.Services
{
    public readonly record struct RunningTaskData(Task<bool> Task, CancellationTokenSource CancellationSource);
}
