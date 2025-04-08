using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.Interactions.InteractDelegate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources
{
    public abstract class PropertyContainer : IPropertyContainer
    {
        protected static uint globalCounter;

        public List<IInvokableInteraction> Interactions = new List<IInvokableInteraction>();

        private readonly static Dictionary<string, PropertyType> requiredProperties = new Dictionary<string, PropertyType>()
        {
            { "ID", PropertyType.UInt32 },
        };

        private readonly static Dictionary<string, PropertyType> optionalProperties = new Dictionary<string, PropertyType>()
        {

        };

        private Dictionary<string, object> defaultProperties = new Dictionary<string, object>()
        {
            {"ID", ++globalCounter},
        };

        static PropertyContainer()
        {
            //Define new required and optional ValidProperties for this class
            DeclareNewPropertyTypes(requiredProperties);
            DeclareNewPropertyTypes(optionalProperties);
        }

        private protected PropertyContainer(Dictionary<string, object> derivedProperties)
        {
            //Apply default properties for this class to the current list of derivedProperties
            ApplyDefaultProperties(derivedProperties, defaultProperties);

            //Create a new property list on Properties.
            Properties = new PropertyHandler(this);

            //Store the final list of properties
            foreach (KeyValuePair<string, object> property in derivedProperties)
                Properties.CreateProperty(property.Key, property.Value);

            //Validate required properties have been set on entityProperties
            ValidateRequiredProperties(requiredProperties);
        }

        private protected static void DeclareNewPropertyTypes(Dictionary<string, PropertyType> newProperties)
        {
            //Define required ValidProperties
            foreach (var property in newProperties)
                PropertyValidation.CreateValidProperty(property.Key, property.Value);
        }

        private protected static void ApplyDefaultProperties(Dictionary<string, object> derivedProperties, Dictionary<string, object> defaultProperties)
        {
            foreach (KeyValuePair<string, object> property in defaultProperties)
                if (!derivedProperties.ContainsKey(property.Key))
                    derivedProperties.Add(property.Key, property.Value);
        }

        private protected void ValidateRequiredProperties(Dictionary<string, PropertyType> testRequiredProperties)
        {
            foreach (var property in testRequiredProperties)
                if (!Properties.HasExistingPropertyName(property.Key))
                    throw new Exception($"A required property has not been defined in {Properties}. Property name: {property.Key}");
        }

        public PropertyHandler Properties { get; set; }
    }
}
