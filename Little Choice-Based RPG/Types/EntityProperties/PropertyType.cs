using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.EntityProperties
{
    /// <summary> Defines all possible types that may be contained within a property. Properties only contain one PropertyType each. </summary>
    enum PropertyType
    {
        Boolean,
        String,
        UInt32,
        Decimal
    }
}
