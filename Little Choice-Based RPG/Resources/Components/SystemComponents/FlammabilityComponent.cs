using Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Break;
using Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Flammability;
using Little_Choice_Based_RPG.Types.PropertySystem.Components;
using Little_Choice_Based_RPG.Types.PropertySystem.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Components.SystemComponents
{
    internal class FlammabilityComponent : IPropertyComponent, ISystemComponent
    {
        /// <summary> Initialises FlammabilitySystem on this entity. </summary>
        public TransparentSystemReference? UsesSystem { get; set => PropertyHandler.ReplaceSystemReference(ref field, value, new FlammabilitySystem()); }

        /// <summary> Marks an entity as flammable, allowing it to catch fire from nearby flames. </summary>
        public TransparentProperty<bool>? IsFlammable { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }

        /// <summary> Indicates that an entity is currently in flames. </summary>
        public TransparentProperty<bool>? IsBurning { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }

        /// <summary> Indicates that an entity has recieved some burn damage and allows it to be additively described accordingly. </summary>
        public TransparentProperty<bool>? IsScorched { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }

        /// <summary> Marks an entity as having been burnt-out, destroyed and no longer aflame. Indicative that there is nothing left to burn. </summary>
        public TransparentProperty<bool>? IsBurntOut { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }
    }
}