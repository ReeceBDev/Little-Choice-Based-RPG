using Little_Choice_Based_RPG.Types.PropertySystem.Components;
using Little_Choice_Based_RPG.Types.PropertySystem.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Components.CommonComponents
{
    internal class ImmaterialComponent : IPropertyComponent
    {
        /// <summary> Marks an entity as immaterial and therefore having no physical appearance or interactiblity in the world. 
        /// This could be used for positional metadata like transitions, triggers and navigation markers. </summary>
        public TransparentProperty<bool>? IsImmaterial { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }
    }
}
