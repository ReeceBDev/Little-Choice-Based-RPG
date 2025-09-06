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
    internal class BreakComponent : IPropertyComponent, ISystemComponent
    {
        /// <summary> Interaction details available under the BreakSystem. </summary>
        public InteractionsClass Interactions { get; } = new();

        /// <summary> Descriptor-related information under specific conditions handled by the BreakSystem. </summary>
        public DescriptorsClass Descriptors { get; } = new();


        /// <summary> Initialises BreakSystem on this entity. </summary>
        public TransparentSystemReference? UsesSystem { get; set => PropertyHandler.ReplaceSystemReference(ref field, value, new BreakSystem()); }

        //BreakSystem logic
        /// <summary> Lets players choose to break by choice, by presenting them with an interaction. Even if this is false, items can still be broken by other means. </summary>
        public TransparentProperty<bool>? BreakableByChoice { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }


        //Descriptors
        public class InteractionsClass
        {
            /// <summary> The break interaction breaks a previously intact object and describes the damage it sustains. </summary>
            public BreakableClass Breakable { get; } = new();

            public class BreakableClass
            {
                /// <summary> Describes the Break option presented to the player, if one is present. </summary>
                public TransparentProperty<string>? Title { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }

                /// <summary> Describes the action of breaking the target when a player uses the Break interaction. </summary>
                public TransparentProperty<string>? Invoking { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); } 
            }
        }

        public class DescriptorsClass
        {
            /// <summary> Descriptors to be used when broken. These should replace their existing descriptors of the same names, respectively. </summary>
            public BrokenClass Broken { get; } = new();

            public class BrokenClass
            {
                /// <summary> Represents what an item looks like when broken, as observed from a distance. </summary>
                public TransparentProperty<string>? Generic { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }

                /// <summary> Represents what an item looks like up close, when broken. </summary>
                public TransparentProperty<string>? Inspect { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }
            }
        }
    }
}
