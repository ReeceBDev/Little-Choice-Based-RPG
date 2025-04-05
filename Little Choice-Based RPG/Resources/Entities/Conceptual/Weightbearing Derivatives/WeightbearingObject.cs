using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.EntityProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Entities.Conceptual.Weightbearing_Derivatives
{
    public class WeightbearingObject : GameObject
    {
        private readonly static Dictionary<string, PropertyType> requiredProperties = new Dictionary<string, PropertyType>()
        {
            {"IsWeightBearing", PropertyType.Boolean},
            {"StrengthInKG", PropertyType.Decimal}, // maximum weightbearing capacity
        };

        private readonly static Dictionary<string, PropertyType> optionalProperties = new Dictionary<string, PropertyType>()
        {
            {"TotalWeightHeldInKG", PropertyType.Decimal}, //current weight held
        };

        private readonly static Dictionary<string, object> defaultProperties = new Dictionary<string, object>()
        {
            {"IsWeightBearing", true }
        };

        static WeightbearingObject()
        {
            //Define new required and optional ValidProperties for this class
            DeclareNewPropertyTypes(requiredProperties);
            DeclareNewPropertyTypes(optionalProperties);
        }

        private protected WeightbearingObject(Dictionary<string, object>? derivedProperties = null)
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
        public void Carry(GameObject target)
        {
            if (!entityProperties.HasProperty("IsWeightBearing", true))
                throw new Exception($"{this} is unable to hold any weight itself!");

            if (!entityProperties.HasExistingPropertyName("TotalWeightHeldInKG"))
                entityProperties.CreateProperty("TotalWeightHeldInKG", 0);

            uint totalStrength = (uint)entityProperties.GetPropertyValue("StrengthInKG");
            uint weightHeld = (uint)entityProperties.GetPropertyValue("TotalWeightHeldInKG");

            uint targetWeight = (uint)target.entityProperties.GetPropertyValue("WeightInKG");

            if (totalStrength < (weightHeld + targetWeight))
                throw new Exception("the object is too heavy to be picked up by this"); // This should just be an error, really.

            Attach(target);
            entityProperties.UpdateProperty("TotalWeightHeldInKG", weightHeld + targetWeight);
        }

        public void Drop(GameObject target)
        {
            uint targetWeight = (uint)target.entityProperties.GetPropertyValue("WeightInKG");
            uint weightHeld = (uint)entityProperties.GetPropertyValue("TotalWeightHeldInKG");

            if (0 > (weightHeld - targetWeight))
                throw new Exception($"{this} went below 0kg weight held when it tried to drop the {target}. That shouldn't happen!");

            Unattach(target);
            entityProperties.UpdateProperty("TotalWeightHeldInKG", weightHeld - targetWeight);
        }

    }
}
