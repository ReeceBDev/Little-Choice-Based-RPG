using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Rooms;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;

namespace Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Weightbearing
{
    internal static class WeightbearingProcessor
    {
        public static void AddWeight(IPropertyContainer target, GameObject subject)
        {
            //Guard Clauses for properties used in this method.
            if (target is not Room)
                if (!target.Properties.HasExistingPropertyName("Weightbearing.StrengthInKG"))
                    throw new Exception($"{target} of ID {target.Properties.GetPropertyValue("ID")} has no property of Weightbearing.StrengthInKG!");

            //Create a new WeightHeldInKG property, if the source has never held weight before.
            if (!target.Properties.HasExistingPropertyName("Weightbearing.WeightHeldInKG"))
                target.Properties.CreateProperty("Weightbearing.WeightHeldInKG", 0);

            //Check that the new weight held isn't illegal
            if (!TargetCanCarry(target, subject))
                throw new Exception("Tried to add weight but the object is too heavy to be picked up by this!");

            //Update with the new weight
            decimal weightHeld = (decimal)target.Properties.GetPropertyValue("Weightbearing.WeightHeldInKG");
            decimal targetWeight = (decimal)subject.Properties.GetPropertyValue("WeightInKG");

            target.Properties.ReplaceProperty("Weightbearing.WeightHeldInKG", weightHeld + targetWeight);
        }

        public static void DropWeight(IPropertyContainer target, GameObject subject)
        {
            //Guard Clauses for properties used in this method.
            if (target is not Room)
                if (!target.Properties.HasExistingPropertyName("Weightbearing.StrengthInKG"))
                    throw new Exception($"{target} of ID {target.Properties.GetPropertyValue("ID")} has no property of Weightbearing.StrengthInKG!");

            if (!target.Properties.HasExistingPropertyName("Weightbearing.WeightHeldInKG"))
                throw new Exception($"{target} of ID {target.Properties.GetPropertyValue("ID")} has no property of Weightbearing.WeightHeldInKG! But this is using the Drop() method, so it should have already been given that property when carrying! Something therefore went wrong.");

            //Check that the new weight held isn't illegal
            decimal subjectWeight = (decimal)subject.Properties.GetPropertyValue("WeightInKG");
            decimal weightHeld = (decimal)target.Properties.GetPropertyValue("Weightbearing.WeightHeldInKG");

            if (0 > weightHeld - subjectWeight)
                throw new Exception($"{target} went below 0kg weight held when it tried to drop the {subject}. That shouldn't happen!");

            //Update with the new weight
            target.Properties.ReplaceProperty("Weightbearing.WeightHeldInKG", weightHeld - subjectWeight);
        }

        public static bool TargetCanCarry(IPropertyContainer target, GameObject subject)
        {
            //Guard Clauses for properties used in this method.
            if (target is Room)
                return true; //Presently, rooms and their derivatives should hold any amount

            if (!target.Properties.HasExistingPropertyName("Weightbearing.StrengthInKG"))
                throw new Exception($"{target} of ID {target.Properties.GetPropertyValue("ID")} has no property of Weightbearing.StrengthInKG!");

            //Create a new WeightHeldInKG property, if the target has never held weight before.
            if (!target.Properties.HasExistingPropertyName("Weightbearing.WeightHeldInKG"))
                target.Properties.CreateProperty("Weightbearing.WeightHeldInKG", 0m);

            //Return whether the target can hold the new weight!
            decimal totalStrength = (decimal)target.Properties.GetPropertyValue("Weightbearing.StrengthInKG");
            decimal weightHeld = (decimal)target.Properties.GetPropertyValue("Weightbearing.WeightHeldInKG");

            decimal subjectWeight = (decimal)subject.Properties.GetPropertyValue("WeightInKG");

            return totalStrength > (weightHeld + subjectWeight);
        }
    }
}
