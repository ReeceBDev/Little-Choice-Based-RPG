using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.EntityProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Entities.Immaterial
{
    public class Immaterial : GameObject
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
            { "IsImmaterial", true }
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
