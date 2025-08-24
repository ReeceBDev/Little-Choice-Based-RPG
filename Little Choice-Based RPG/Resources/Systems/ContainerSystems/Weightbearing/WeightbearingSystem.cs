using Little_Choice_Based_RPG.Resources.Entities;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Rooms;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.TypedEventArgs.PropertyContainerEventArgs;

namespace Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Weightbearing
{
    /// <summary> Gives objects strength to hold weight. Decides if further objects may be carried. Requires something to use it, i.e. InventorySystem, GearSystem. All weight and strength is in KG (Kilogrammes). </summary>
    internal class WeightbearingSystem : PropertyLogic
    {
        static WeightbearingSystem()
        {
            PropertyValidation.CreateValidProperty("Weightbearing.WeightHeldInKG", PropertyType.Decimal); // Weight currently held
            PropertyValidation.CreateValidProperty("Weightbearing.StrengthInKG", PropertyType.Decimal); //Maximum weight carriable
        }
        protected override void InitialiseNewSubscriber(PropertyContainer sourceContainer, PropertyHandler sourceProperties)
        {
            //Require StrengthInKG, unless it's a Room.
            if (!sourceProperties.GetPropertyValue("Type").Equals("Little_Choice_Based_RPG.Resources.Rooms.Room"))
                if (!sourceProperties.HasExistingPropertyName("Weightbearing.StrengthInKG"))
                    throw new Exception($"{sourceContainer} of ID {sourceProperties.GetPropertyValue("ID")} has no property of Weightbearing.StrengthInKG!");
        }

        protected override void OnObjectChanged(object sender, ObjectChangedEventArgs propertyChangedData)
        {
            //Add or remove weight, unless it's a Room.
            if (propertyChangedData.Source is not Room)
            {
                if (propertyChangedData.Property.Equals("ItemContainer.Added"))
                    WeightbearingProcessor.AddWeight((GameObject)propertyChangedData.Source, (GameObject)propertyChangedData.Change);

                if (propertyChangedData.Property.Equals("ItemContainer.Removed"))
                    WeightbearingProcessor.DropWeight((GameObject)propertyChangedData.Source, (GameObject)propertyChangedData.Change);
            }
        }
    }
}
