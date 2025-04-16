using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.RoomTypes
{
    public record struct ConditionalDescriptor(string Descriptor, List<EntityState> RequiredEntityStates, uint Priority = 6);
}
