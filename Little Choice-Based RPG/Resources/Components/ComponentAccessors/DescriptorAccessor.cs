using Little_Choice_Based_RPG.Resources.Components.SystemComponents;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Components.ComponentAccessors
{
    internal static class DescriptorAccessor
    {
        extension(IPropertyContainer container)
        {
            /// <summary> Represents specific properties used by DescriptorSystem, which allows entities to be described in detail and inspected. </summary>
            public DescriptorComponent Descriptor => container.Properties.Get<DescriptorComponent>();
        }
    }
}
