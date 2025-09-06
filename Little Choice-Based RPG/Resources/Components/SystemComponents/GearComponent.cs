using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Gear;
using Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Break;
using Little_Choice_Based_RPG.Types.PropertySystem.Components;
using Little_Choice_Based_RPG.Types.PropertySystem.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Components.SystemComponents
{
    internal class GearComponent : IPropertyComponent, ISystemComponent
    {
        /// <summary> Indicates whether the GearSystem is in use on this entity. </summary>
        public TransparentSystemReference? UsesSystem { get; set => PropertyHandler.ReplaceSystemReference(ref field, value, new GearSystem()); }

        /// <summary> Indicative of whether an entity has gear slots which allow it to hold and make use of gear. </summary>
        public TransparentProperty<bool>? HasGearSlots { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }

        /// <summary> Represents the ID in use by the helmet slot. Note: This is liable to change as it is not currently implemented. 
        /// Note2: It should probably go inside a nested class at the very least. </summary>
        public TransparentProperty<uint>? HelmetID { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }
    }
}