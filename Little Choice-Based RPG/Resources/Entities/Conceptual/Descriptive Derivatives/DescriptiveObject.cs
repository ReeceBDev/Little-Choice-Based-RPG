﻿using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
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

        private readonly static Dictionary<string, object> defaultProperties = new Dictionary<string, object>()
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

        private protected DescriptiveObject(PropertyHandler? derivedProperties = null)
            : base(SetLocalProperties(derivedProperties ??= new PropertyHandler()))
        {
            //Freeze default descriptors.
            entityProperties.FreezeProperty("Descriptor.Generic.Default");
            entityProperties.FreezeProperty("Descriptor.Inspect.Default");

            //Set current descriptors to defaults.
            entityProperties.UpsertProperty("Descriptor.Generic.Current", entityProperties.GetPropertyValue("Descriptor.Generic.Default"));
            entityProperties.UpsertProperty("Descriptor.Inspect.Current", entityProperties.GetPropertyValue("Descriptor.Inspect.Default"));

            //Validate required properties have been set on entityProperties
            ValidateRequiredProperties(requiredProperties);
        }

        private static PropertyHandler SetLocalProperties(PropertyHandler derivedProperties)
        {
            //Apply default properties for this class to the current list of derivedProperties
            ApplyDefaultProperties(derivedProperties, defaultProperties);

            return derivedProperties; //Return is required to give (base) the derived list.
        }

        public void SetGenericDescriptor(string newGenericDescriptor) => entityProperties.UpsertProperty("Descriptor.Generic.Current", newGenericDescriptor);
        public void SetInspectDescriptor(string newInspectDescriptor) => entityProperties.UpsertProperty("Descriptor.Inspect.Current", newInspectDescriptor);
        public string InspectDescriptor => (string) entityProperties.GetPropertyValue("Descriptor.Inspect.Current");
        public string GenericDescriptor => (string) entityProperties.GetPropertyValue("Descriptor.Generic.Current");
    }
}
