using Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Break;
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
    internal class RepairComponent : IPropertyComponent, ISystemComponent
    {
        /// <summary> Interaction details available under the RepairSystem. </summary>
        public InteractionsClass Interactions { get; } = new();


        /// <summary> Initialises RepairSystem on this entity. Allows broken entities to be repaired by some means. </summary>
        public TransparentSystemReference? UsesSystem { get; set => PropertyHandler.ReplaceSystemReference(ref field, value, new RepairSystem()); }

        //RepairSystem logic
        /// <summary> Lets players choose to repair by choice, by presenting them with an interaction. Even if this is false, enntites might still be repaired by other means. </summary>
        public TransparentProperty<bool>? RepairableByChoice { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); } //Lets players choose to break it by choice. 


        //Descriptors
        public class InteractionsClass
        {
            /// <summary> The repair interaction repairs a previously broken object and describes the repairation process. </summary>
            public RepairableClass Repairable { get; } = new();


            public class RepairableClass
            {
                /// <summary> Describes the Repair option presented to the player, if one is present. </summary>
                public TransparentProperty<string>? Title { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }

                /// <summary> Describes the action of repair the target when a player uses the Repair interaction. </summary>
                public TransparentProperty<string>? Invoking { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }

                /// <summary> Defines the part used in the repair. Must match the type on the component (subject to change upon fleshing out this system). </summary>
                public TransparentProperty<string>? RequiredComponent { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }

                /// <summary> Requires tools if true, but may be repaired by hand if false. </summary>
                public TransparentProperty<bool>? RequiresTool { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }

                /// <summary> Defines which tool is required to repair. Must match the type on the repair tool (subject to change upon fleshing out this system). </summary>
                public TransparentProperty<string>? RequiredToolType { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }
            }
        }
    }
}