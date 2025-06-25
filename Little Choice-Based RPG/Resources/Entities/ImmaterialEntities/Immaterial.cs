using Little_Choice_Based_RPG.Types.EntityProperties;

namespace Little_Choice_Based_RPG.Resources.Entities.ImmaterialEntities
{
    public class Immaterial : PropertyContainer
    {
        private readonly static Dictionary<string, PropertyType> requiredProperties = new Dictionary<string, PropertyType>()
        {
            {"IsImmaterial", PropertyType.Boolean},
        };

        private readonly static Dictionary<string, PropertyType> optionalProperties = new Dictionary<string, PropertyType>()
        {

        };

        private readonly static Dictionary<string, object> defaultProperties = new Dictionary<string, object>()
        {
            { "IsImmaterial", true },
            { "Name", "Unknown Immaterial" }
        };

        static Immaterial()
        {
            //Define new required and optional ValidProperties for this class
            DeclareNewPropertyTypes(requiredProperties);
            DeclareNewPropertyTypes(optionalProperties);
        }

        private protected Immaterial(Dictionary<string, object>? derivedProperties = null)
            : base(SetLocalProperties(derivedProperties ??= new Dictionary<string, object>()))
        {
            //Update the default name to contain the type
            if (this.Properties.HasPropertyAndValue("Name", "Unknown Immaterial"))
                this.Properties.UpsertProperty("Name", $"Unknown Immaterial of type \"{GetType().Name.ToString()}\"");


            //Freeze IsImmaterial property.
            Properties.FreezeProperty("IsImmaterial");

            //Validate required properties have been set on entityProperties
            ValidateRequiredProperties(requiredProperties);
        }

        private static Dictionary<string, object> SetLocalProperties(Dictionary<string, object> derivedProperties)
        {
            //Apply default properties for this class to the current list of derivedProperties
            ApplyDefaultProperties(derivedProperties, defaultProperties);

            return derivedProperties; //Return is required to give (base) the derived list.
        }
    }
}
