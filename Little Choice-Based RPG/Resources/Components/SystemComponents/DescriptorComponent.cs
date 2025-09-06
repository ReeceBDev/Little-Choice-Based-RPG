using Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Break;
using Little_Choice_Based_RPG.Resources.Systems.InformationalSystems.Descriptor;
using Little_Choice_Based_RPG.Types.PropertySystem.Components;
using Little_Choice_Based_RPG.Types.PropertySystem.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Components.SystemComponents
{
    internal class DescriptorComponent : IPropertyComponent, ISystemComponent
    {
        /// <summary> Describes an objects appearance at a distance of about a few meters away, as casual onlooker who may not be paying much attention. </summary>
        public GenericClass Generic { get; } = new();

        /// <summary> Describes an objects appearance up close, as a direct inspector with hands on the object who is very interested in learning as much as able. </summary>
        public InspectClass Inspect { get; } = new();


        /// <summary> Initialises DescriptorSystem on this entity. Allows entities to be described in detail when navigating the world or inspecting closely. </summary>
        public TransparentSystemReference? UsesSystem { get; set => PropertyHandler.ReplaceSystemReference(ref field, value, new DescriptorSystem()); }


        //Generic
        public class GenericClass
        {
            /// <summary> The current descriptor of an entity as observed in the world from a reasonable distance of only a few meters. </summary>
            public TransparentProperty<string>? Current { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }

            /// <summary> The fallback descriptor of an entity as observed in the world from a reasonable distance of only a few meters. </summary>
            public TransparentProperty<string>? Default { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }
        }

        //Inspect
        public class InspectClass
        {
            /// <summary> Additive descriptors are assembled from several components and can help to describe complex objects like shelves cluttered with items. </summary>
            public AdditiveClass Additive { get; } = new();

            /// <summary> The current descriptor of an entity as observed in the world at an immediately close distance, or held and rotated in the hands. </summary>
            public TransparentProperty<string>? Current { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }

            /// <summary> The fallback descriptor of an entity as observed in the world at an immediately close distance, or held and rotated in the hands. </summary>
            public TransparentProperty<string>? Default { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }


            public class AdditiveClass
            {
                /// <summary> Dictates if a descriptor uses the Additive descriptor subsystem. Provides complex descriptions made up from several components. </summary>
                public TransparentProperty<bool>? IsAdditive { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }

                /// <summary> The first component of an additive descriptor. A partial and introductory description which sits before the infix. </summary>
                public TransparentProperty<string>? Suffix { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }

                /// <summary> The second component of an additive descriptor. A partial contextual description which sits between the prefix and suffix. </summary>
                public TransparentProperty<string>? Infix { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }

                /// <summary> The third component of an additive descriptor. A partial closing description which sits after the infix. </summary>
                public TransparentProperty<string>? Prefix { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }

                /// <summary> The default of an additive descriptor. 
                /// Represents an entity with less colour such as an empty set of shelves which would otherwise be described complexly with the additive system. 
                /// (Note: This class might be redundant and best replaced with the standard Default descriptor as a fallback, instead...?) </summary>
                public TransparentProperty<string>? Empty { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }
            }
        }
    }
}