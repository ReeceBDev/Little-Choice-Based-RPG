using Little_Choice_Based_RPG.Types.PropertySystem.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.PropertySystem.Components
{
    /// <summary> Marker interface used to mark a set of properties as an IEntityComponent.
    /// Note: Despite marker interfaces usually being a bit of an anti-pattern, in this case I'm using it to constrain generics. </summary>
    internal interface IPropertyComponent
    {
    }
}
