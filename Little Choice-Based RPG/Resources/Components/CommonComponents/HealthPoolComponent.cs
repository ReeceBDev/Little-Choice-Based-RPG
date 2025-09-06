using Little_Choice_Based_RPG.Types.PropertySystem.Components;
using Little_Choice_Based_RPG.Types.PropertySystem.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Components.CommonComponents
{
    internal class HealthPoolComponent : IPropertyComponent
    {
        /// <summary> Represents if something is alive and intact or dead and destroyed. May sensibly apply to inanimate objects provided they have structural integrity. </summary>
        public TransparentProperty<bool>? IsLiving { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }

        /// <summary> Represents the healthiness or structural integrity of an entity. May sensibly apply to inanimate objects and is not exclusive to living creatures. </summary>
        public TransparentProperty<uint>? Health { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }
    }
}