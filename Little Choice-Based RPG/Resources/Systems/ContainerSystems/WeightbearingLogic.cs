using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.EntityProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems.ContainerSystems
{
    /// <summary> Gives objects strength to hold weight. Decides if further objects may be carried. Requires something to use it, i.e. InventorySystem, GearSystem. All weight and strenght is in KG (Kilogrammes). </summary>
    public class WeightbearingLogic : PropertyLogic
    {
        static WeightbearingLogic()
        {
            PropertyValidation.CreateValidProperty("Weightbearing.WeightHeldInKG", PropertyType.Decimal); // Weight currently held
            PropertyValidation.CreateValidProperty("Weightbearing.StrengthInKG", PropertyType.Decimal); //Maximum weight carriable
        }
        protected override void InitialiseNewSubscriber(PropertyContainer sourceContainer, PropertyHandler sourceProperties)
        {
            //Require StrengthInKG
            if (!sourceProperties.HasExistingPropertyName("Weightbearing.StrengthInKG"))
                throw new Exception($"{sourceContainer} of ID {sourceProperties.GetPropertyValue("ID")} has no property of Weightbearing.StrengthInKG!");
        }



        public bool CheckIfCarriable(GameObject source, GameObject target)
        {
            //Guard Clauses for properties used in this method.
            if (!sourceProperties.HasExistingPropertyName("Weightbearing.StrengthInKG"))
                throw new Exception($"{sourceContainer} of ID {sourceProperties.GetPropertyValue("ID")} has no property of Weightbearing.StrengthInKG!");

            //Create a new WeightHeldInKG property, if the target has never held weight before.
            if (!sourceProperties.HasExistingPropertyName("Weightbearing.StrengthInKG"))
                throw new Exception($"{sourceContainer} of ID {sourceProperties.GetPropertyValue("ID")} has no property of Weightbearing.StrengthInKG!");

            //Return whether the target can hold the new weight!
            uint totalStrength = (uint)Properties.GetPropertyValue("Weightbearing.StrengthInKG");
            uint weightHeld = (uint)Properties.GetPropertyValue("Weightbearing.WeightHeldInKG");

            uint targetWeight = (uint)target.Properties.GetPropertyValue("WeightInKG");

            return (totalStrength < (weightHeld + targetWeight));                
        }

        public void Carry(GameObject source, GameObject target)
        {
            //Guard Clauses for properties used in this method.
            if (!sourceProperties.HasExistingPropertyName("Weightbearing.StrengthInKG"))
                throw new Exception($"{sourceContainer} of ID {sourceProperties.GetPropertyValue("ID")} has no property of Weightbearing.StrengthInKG!");

            //Create a new WeightHeldInKG property, if the target has never held weight before.
            if (!Properties.HasExistingPropertyName("Weightbearing.WeightHeldInKG"))
                Properties.CreateProperty("Weightbearing.WeightHeldInKG", 0);

            v//Check that the new weight held isn't illegal
            if (!CheckIfCarriable(source, target))
                throw new Exception("(TURN THIS into a normal, user-friendly action-not-completed in-game error! :) ) the object is too heavy to be picked up by this"); // This should just be an error, really.

            //Update with the new weight
            Properties.ReplaceProperty("Weightbearing.WeightHeldInKG", weightHeld + targetWeight);
        }

        public void Drop(GameObject target)
        {
            //Guard Clauses for properties used in this method.
            if (!sourceProperties.HasExistingPropertyName("Weightbearing.StrengthInKG"))
                throw new Exception($"{sourceContainer} of ID {sourceProperties.GetPropertyValue("ID")} has no property of Weightbearing.StrengthInKG!");

            if (!sourceProperties.HasExistingPropertyName("Weightbearing.WeightHeldInKG"))
                throw new Exception($"{sourceContainer} of ID {sourceProperties.GetPropertyValue("ID")} has no property of Weightbearing.WeightHeldInKG! But this is using the Drop() method, so it should have already been given that property when carrying! Something therefore went wrong.");

            //Check that the new weight held isn't illegal
            uint targetWeight = (uint)target.Properties.GetPropertyValue("WeightInKG");
            uint weightHeld = (uint)Properties.GetPropertyValue("Weightbearing.WeightHeldInKG");

            if (0 > (weightHeld - targetWeight))
                throw new Exception($"{this} went below 0kg weight held when it tried to drop the {target}. That shouldn't happen!");

            //Update with the new weight
            Properties.ReplaceProperty("Weightbearing.WeightHeldInKG", weightHeld - targetWeight);
        }

    }
}
