using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities;
using Little_Choice_Based_RPG.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Little_Choice_Based_RPG.Types.EntityProperties;

namespace Little_Choice_Based_RPG.Resources.Entities.Conceptual.Descriptive_Derivatives
{
    public class DescriptiveObject : GameObject
    {

        private readonly static Dictionary<string, PropertyType> requiredProperties = new Dictionary<string, PropertyType>()
        { 
            {"Descriptor.Generic.Current", PropertyType.String}, //Generic Descriptor the object is currently using
            {"Descriptor.Inspect.Current", PropertyType.String}, //Inspect Descriptor the object is currently using

            {"Descriptor.Generic.Default", PropertyType.String},
            {"Descriptor.Inspect.Default", PropertyType.String},
        };

        private readonly static Dictionary<string, PropertyType> optionalProperties = new Dictionary<string, PropertyType>()
        {

        };

        private Dictionary<string, object> defaultProperties = new Dictionary<string, object>()
        {
            {"Descriptor.Generic.Default", "[ERROR] - There's something here but you don't know what it is?"},
            {"Descriptor.Inspect.Default", $"You inspect this further. Albeit, for all the wisdom you gather, it is still just an [ERROR] to the limits of your knowledge."}
        };

        static DescriptiveObject()
        {
            //Define new required and optional ValidProperties for this class
            DeclareNewPropertyTypes(requiredProperties);
            DeclareNewPropertyTypes(optionalProperties);
        }

        private protected DescriptiveObject(Dictionary<string, object>? derivedProperties = null)
            : base(SetLocalProperties(derivedProperties ??= new Dictionary<string, object>()))
        {
            //Freeze default descriptors.
            Properties.FreezeProperty("Descriptor.Generic.Default");
            Properties.FreezeProperty("Descriptor.Inspect.Default");

            //Set current descriptors to defaults.
            Properties.UpsertProperty("Descriptor.Generic.Current", Properties.GetPropertyValue("Descriptor.Generic.Default"));
            Properties.UpsertProperty("Descriptor.Inspect.Current", Properties.GetPropertyValue("Descriptor.Inspect.Default"));

            //Validate required properties have been set on entityProperties
            ValidateRequiredProperties(requiredProperties);
        }

        private static Dictionary<string, object> SetLocalProperties(Dictionary<string, object> derivedProperties)
        {
            //Apply default properties for this class to the current list of derivedProperties
            ApplyDefaultProperties(derivedProperties, defaultProperties);

            return derivedProperties; //Return is required to give (base) the derived list.
        }

        public void SetGenericDescriptor(string newGenericDescriptor) => Properties.UpsertProperty("Descriptor.Generic.Current", newGenericDescriptor);
        public void SetInspectDescriptor(string newInspectDescriptor) => Properties.UpsertProperty("Descriptor.Inspect.Current", newInspectDescriptor);
        public string InspectDescriptor => (string) Properties.GetPropertyValue("Descriptor.Inspect.Current");
        public string GenericDescriptor => (string) Properties.GetPropertyValue("Descriptor.Generic.Current");
    }
}
