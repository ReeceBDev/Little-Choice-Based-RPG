using Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Break;
using Little_Choice_Based_RPG.Resources.Systems.PlayerSystems;
using Little_Choice_Based_RPG.Types.PropertySystem.Components;
using Little_Choice_Based_RPG.Types.PropertySystem.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Components.SystemComponents
{
    internal class PlayerComponent : IPropertyComponent, ISystemComponent
    {
        /// <summary> Descriptors used by the PlayerSystem or relating to the player specifically. </summary>
        public DescriptorClass Descriptor = new();


        /// <summary> Represents whether an entity uses the PlayerSystem. </summary>
        public TransparentSystemReference? UsesSystem { get; set => PropertyHandler.ReplaceSystemReference(ref field, value, new PlayerSystem()); }

        /// <summary> Represents if the player can recieve visual information from the world around them. </summary>
        public TransparentProperty<bool>? CanHear { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }

        /// <summary> Represents if the player can recieve audiable information from the world around them. </summary>
        public TransparentProperty<bool>? CanSee { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }


        public class DescriptorClass
        {
            /// <summary> Describes the player from a third person perspective. </summary>
            public TransparentProperty<string>? Custom { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }
        }
    }
}
