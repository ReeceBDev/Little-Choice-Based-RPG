using Little_Choice_Based_RPG.Managers.World;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Immaterial.Container;
using Little_Choice_Based_RPG.Types.EntityProperties;

namespace Little_Choice_Based_RPG.Resources.Systems.Container
{
    internal class InventorySystem : PropertyLogic
    {
        static InventorySystem()
        {
            PropertyValidation.CreateValidProperty("Inventory.ID", PropertyType.UInt32);
            PropertyValidation.CreateValidProperty("Inventory.Stored", PropertyType.UInt32);
        }
        protected override void InitialiseNewSubscriber(PropertyContainer sourceContainer, PropertyHandler sourceProperties)
        {
            //Give the subscriber an ItemContainer
            if (!sourceProperties.HasExistingPropertyName("Inventory.ID")) //If an ItemContainer isnt already assigned
            {
                //Outline new Inventory
                Dictionary<string, object> newInventoryProperties = new Dictionary<string, object>();

                //Set new Inventory properties
                newInventoryProperties.Add("Name", $"{sourceProperties.GetPropertyValue("Name")}.Inventory");
                newInventoryProperties.Add("Type", "ItemContainer");

                //Set inventory weight limit from source object
                newInventoryProperties.Add("");

                //Generate new Inventory
                GameObject newInventory = GameObjectFactory.NewGameObject(newInventoryProperties);
            }  
            else //If an ItemContainer is already assigned
            {
                uint itemContainerID = (uint) sourceProperties.GetPropertyValue("Inventory.ID");
                //Load it/assign it or whatever else is necessary

                //Update inventory weight limit from source object
            }




            // If the object is broken already
            if (sourceProperties.HasProperty("Damage.Broken", true))
            {
                if (sourceProperties.HasProperty("Repairable.ByChoice", true))
                    InitialiseRepairChoices(sourceContainer, sourceProperties);
            }
        }
    }
}
