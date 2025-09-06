using Little_Choice_Based_RPG.Types.PropertySystem.Components;
using Little_Choice_Based_RPG.Types.PropertySystem.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Components.CommonComponents
{
    internal class WeightComponent : IPropertyComponent
    {
        /// <summary> Represents the weight an object has in KG. May also reasonably represent size and volume, since volume is not a distinctive value. </summary>
        public TransparentProperty<decimal>? WeightInKG { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }
    }
}
