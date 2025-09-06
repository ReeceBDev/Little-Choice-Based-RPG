using Little_Choice_Based_RPG.Resources.Components.SystemComponents;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Components.ComponentAccessors
{
    internal static class BreakAccessor
    {
        extension(IPropertyContainer container)
        {
            /// <summary> Represents specific properties used by BreakSystem, which allows entities to break and be broken. </summary>
            public BreakComponent Break => container.Properties.Get<BreakComponent>();
        }
    }
}
