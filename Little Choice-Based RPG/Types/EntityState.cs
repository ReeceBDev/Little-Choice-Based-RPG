using Little_Choice_Based_RPG.Types.EntityProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types
{
    /// <summary> A possible state or existence of a GameObject. Note: An ID without an EntityProperty will just be checked for being present. </summary>
    public record struct EntityState(uint EntityReferenceID, List<EntityProperty>? RequiredProperties);
}
