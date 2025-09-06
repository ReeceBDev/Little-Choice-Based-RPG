using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Weightbearing;
using Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Repair;
using Little_Choice_Based_RPG.Types.PropertySystem.Components;
using Little_Choice_Based_RPG.Types.PropertySystem.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Components.SystemComponents
{
    internal class WeightbearingComponent : IPropertyComponent, ISystemComponent
    {
        /// <summary> Indicates whether an entity is using the WeightbearingSystem. This is responsible for enabling entities to take and bear the weight of others. </summary>
        public TransparentSystemReference? UsesSystem { get; set => PropertyHandler.ReplaceSystemReference(ref field, value, new WeightbearingSystem()); }


        /// <summary> The weight in kilogrammes currently held by an entity. </summary>
        public TransparentProperty<decimal>? WeightHeldInKG { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }

        /// <summary> The strength of an entity in kilogrammes. This will usually respresent how much weight an entity might take, bear, hold, or move. </summary>
        public TransparentProperty<decimal>? StrengthInKG { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }

    }
}