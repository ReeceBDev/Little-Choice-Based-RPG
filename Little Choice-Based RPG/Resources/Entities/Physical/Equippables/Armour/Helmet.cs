using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.EntityProperties;

namespace Little_Choice_Based_RPG.Resources.Entities.Physical.Equippables.Armour
{
    internal class Helmet : GameObject
    {
        private readonly static Dictionary<string, PropertyType> requiredProperties = new Dictionary<string, PropertyType>()
        {

        };

        private readonly static Dictionary<string, PropertyType> optionalProperties = new Dictionary<string, PropertyType>()
        {

        };

        private readonly static Dictionary<string, object> defaultProperties = new Dictionary<string, object>()
        {
            {"WeightInKG", 2.3m},
            {"Descriptor.Generic.Default", "Folorn, a mudcaked helmet rots here in its own scrap, long since abandoned." }
        };

        static Helmet()
        {
            //Define new required and optional ValidProperties for this class
            DeclareNewPropertyTypes(requiredProperties);
            DeclareNewPropertyTypes(optionalProperties);
        }

        public Helmet(Dictionary<string, object>? derivedProperties = null)
            : base(SetLocalProperties(derivedProperties ??= new Dictionary<string, object>()))
        {
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
