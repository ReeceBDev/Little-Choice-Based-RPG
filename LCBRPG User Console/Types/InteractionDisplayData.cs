using Little_Choice_Based_RPG.External.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCBRPG_User_Console.Types
{
    internal readonly record struct InteractionDisplayData(ulong InteractionID, string InteractionTitle, string PresentationContext, uint AssociatedObjectID);
}
