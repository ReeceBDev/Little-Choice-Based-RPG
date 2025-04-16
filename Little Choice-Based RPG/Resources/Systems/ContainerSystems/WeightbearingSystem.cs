using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.EventArgs;
using Little_Choice_Based_RPG.Types.EntityProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems.ContainerSystems
{
    /// <summary> Gives objects strength to hold weight. Decides if further objects may be carried. Requires something to use it, i.e. InventorySystem, GearSystem. All weight and strength is in KG (Kilogrammes). </summary>
    public class WeightbearingSystem : PropertyLogic
    {
        static WeightbearingSystem()
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

        protected override void OnObjectChanged(object sender, ObjectChangedEventArgs propertyChangedData)
        {
            if (propertyChangedData.IdentifierChanged.Equals("ItemContainer.Added"))
                AddWeight((GameObject)propertyChangedData.SourceContainer, (GameObject)propertyChangedData.ValueChanged);

            if (propertyChangedData.IdentifierChanged.Equals("ItemContainer.Removed"))
                DropWeight((GameObject)propertyChangedData.SourceContainer, (GameObject)propertyChangedData.ValueChanged);
        }

        private static void AddWeight(PropertyContainer sourceContainer, GameObject target)
        {
            //Guard Clauses for properties used in this method.
            if (!sourceContainer.Properties.HasExistingPropertyName("Weightbearing.StrengthInKG"))
                throw new Exception($"{sourceContainer} of ID {sourceContainer.Properties.GetPropertyValue("ID")} has no property of Weightbearing.StrengthInKG!");

            //Create a new WeightHeldInKG property, if the source has never held weight before.
            if (!sourceContainer.Properties.HasExistingPropertyName("Weightbearing.WeightHeldInKG"))
                sourceContainer.Properties.CreateProperty("Weightbearing.WeightHeldInKG", 0);

            //Check that the new weight held isn't illegal
            if (!WeightbearingLogic.CheckIfCarriable(sourceContainer, target))
                throw new Exception("Tried to add weight but the object is too heavy to be picked up by this!"); 

            //Update with the new weight
            uint weightHeld = (uint)sourceContainer.Properties.GetPropertyValue("Weightbearing.WeightHeldInKG");
            uint targetWeight = (uint)target.Properties.GetPropertyValue("WeightInKG");
            
            sourceContainer.Properties.ReplaceProperty("Weightbearing.WeightHeldInKG", weightHeld + targetWeight);
        }

        private static void DropWeight(PropertyContainer sourceContainer, GameObject target)
        {
            //Guard Clauses for properties used in this method.
            if (!sourceContainer.Properties.HasExistingPropertyName("Weightbearing.StrengthInKG"))
                throw new Exception($"{sourceContainer} of ID {sourceContainer.Properties.GetPropertyValue("ID")} has no property of Weightbearing.StrengthInKG!");

            if (!sourceContainer.Properties.HasExistingPropertyName("Weightbearing.WeightHeldInKG"))
                throw new Exception($"{sourceContainer} of ID {sourceContainer.Properties.GetPropertyValue("ID")} has no property of Weightbearing.WeightHeldInKG! But this is using the Drop() method, so it should have already been given that property when carrying! Something therefore went wrong.");

            //Check that the new weight held isn't illegal
            uint targetWeight = (uint)target.Properties.GetPropertyValue("WeightInKG");
            uint weightHeld = (uint)sourceContainer.Properties.GetPropertyValue("Weightbearing.WeightHeldInKG");

            if (0 > (weightHeld - targetWeight))
                throw new Exception($"{sourceContainer} went below 0kg weight held when it tried to drop the {target}. That shouldn't happen!");

            //Update with the new weight
            sourceContainer.Properties.ReplaceProperty("Weightbearing.WeightHeldInKG", weightHeld - targetWeight);
        }

    }
}
