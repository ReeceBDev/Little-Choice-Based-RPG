using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems.ContainerSystems
{
    public static class WeightbearingLogic
    {
        public static bool CheckIfCarriable(PropertyContainer carryingContainer, GameObject targetObject)
        {
            //Guard Clauses for properties used in this method.
            if (!carryingContainer.Properties.HasExistingPropertyName("Weightbearing.StrengthInKG"))
                throw new Exception($"{carryingContainer} of ID {carryingContainer.Properties.GetPropertyValue("ID")} has no property of Weightbearing.StrengthInKG!");

            //Create a new WeightHeldInKG property, if the target has never held weight before.
            if (!carryingContainer.Properties.HasExistingPropertyName("Weightbearing.StrengthInKG"))
                throw new Exception($"{carryingContainer} of ID {carryingContainer.Properties.GetPropertyValue("ID")} has no property of Weightbearing.StrengthInKG!");

            //Return whether the target can hold the new weight!
            uint totalStrength = (uint)carryingContainer.Properties.GetPropertyValue("Weightbearing.StrengthInKG");
            uint weightHeld = (uint)carryingContainer.Properties.GetPropertyValue("Weightbearing.WeightHeldInKG");

            uint targetWeight = (uint)targetObject.Properties.GetPropertyValue("WeightInKG");

            return (totalStrength < (weightHeld + targetWeight));
        }
    }
}
