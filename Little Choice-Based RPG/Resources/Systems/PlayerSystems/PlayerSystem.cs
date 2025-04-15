using Little_Choice_Based_RPG.Resources.Extensions;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems;
using Little_Choice_Based_RPG.Resources.Systems.SystemEventBus;
using Little_Choice_Based_RPG.Types.EntityProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems.PlayerSystems
{
    /// <summary> Implements player-specific logic. Requires InventoryLogic.</summary>
    public class PlayerSystem : PropertyLogic
    {
        //This class requires InventorySystem.
        InventorySystem inventorySystem = InventorySystem.Instance;

        static PlayerSystem()
        {
            PropertyValidation.CreateValidProperty("Player.CanHear", PropertyType.Boolean);
            PropertyValidation.CreateValidProperty("Player.CanSee", PropertyType.Boolean);
        }

        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedData)
        {
            if (propertyChangedData.newProperty.PropertyName == "Position")// player room changed
                GenerateInventoryChoices(propertyChangedData.sourceContainer, propertyChangedData.sourceContainer.Properties); 
        }

        protected override void InitialiseNewSubscriber(PropertyContainer sourceContainer, PropertyHandler sourceProperties)
        {
            InitialiseInventoryComponent(sourceContainer, sourceProperties);
            GenerateInventoryChoices(sourceContainer, sourceProperties);
        }

        private void InitialiseInventoryComponent(PropertyContainer sourceContainer, PropertyHandler sourceProperties)
        {
            //The subscriber requires Component.InventorySystem. This then checks for the required inventory properties itself :)
            if (!sourceProperties.HasExistingPropertyName("Component.InventorySystem"))
            {
                sourceProperties.CreateProperty("Component.InventorySystem", true); //Creates the inventory component.
                SystemSubscriptionEventBus.Subscribe(sourceContainer, "InventorySystem"); //Subscribes the source to the component.
            }
        }

        private void GenerateInventoryChoices(PropertyContainer sourceContainer, PropertyHandler sourceProperties)
        {
            //Get room
            //Iterate through each item in the room
            //Give the player pick=-ups for them based on their capability
            //Generate moves

            //Iterate through player stored inventory items
            //Generate put-downs
            //Generate moves
        }
    }
}
