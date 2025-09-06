using Little_Choice_Based_RPG.Types.PropertySystem.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.PropertySystem.Components
{
    /// <summary> Must be used on components which represent properties specific to a particular system. </summary>
    internal interface ISystemComponent
    {
        public TransparentSystemReference? UsesSystem { get; set; }
    }
}
