using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface;
using Little_Choice_Based_RPG.Managers.World;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Resources.Systems.DamageSystems;
using Little_Choice_Based_RPG.Resources.Systems.SystemEventBus;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.Extensions;

namespace Little_Choice_Based_RPG.Resources.Systems.ContainerSystems
{
    /// <summary> Allows PropertyContainers to contain other GameObject. This takes them out of the Rooms' own contents. Requires WeightbearingLogic. </GameObject> </summary>
    public class InventorySystem : PropertyLogic
    {
        //This class requires WeightbearingLogic.
        WeightbearingLogic weightBearingLogicInstantiation = WeightbearingLogic.Instance;

        protected override void InitialiseNewSubscriber(PropertyContainer sourceContainer, PropertyHandler sourceProperties)
        {
            //The subscriber requires Component.Weightbearing. This then checks for the required weightbearing values itself :)
            if (!sourceProperties.HasExistingPropertyName("Component.WeightbearingLogic"))
            {
                sourceProperties.CreateProperty("Component.WeightbearingLogic", true); //Creates the weightbearing component.
                SystemSubscriptionEventBus.Subscribe(sourceContainer, "WeightbearingLogic"); //Subscribes the source to the component.
            }

            //Give the subscriber an ItemContainer
            if (!sourceContainer.Extensions.ContainsExtension("Inventory")) //If an ItemContainer isnt already assigned
            {
                //Generate new Inventory
                ItemContainer newInventory = new ItemContainer();

                //Then add the inventory to the source's extensions.
                sourceContainer.Extensions.AddExtension(newInventory);
            }
            else //If an ItemContainer is already assigned
            {
                //Find the correct one (by unique ID on each inventory?)
                //Load it/assign it or whatever else is necessary
                //Update inventory weight limit from source object

                throw new NotImplementedException("Not yet implemented. This is waiting for the loading of saved states!");
            }
        }

        public static void MoveBetweenInventories(IUserInterface mutexHolder, GameObject target,
                PropertyContainer source, PropertyContainer destination)
        {
            if (!source.Extensions.ContainsExtension("Inventory"))
                throw new Exception("");

            if (!destination.Extensions.ContainsExtension("Inventory"))
                throw new Exception("");

            ItemContainer sourceInventory = (ItemContainer)source.Extensions.GetExtension("Inventory");
            ItemContainer destinationInventory = (ItemContainer)destination.Extensions.GetExtension("Inventory");

            sourceInventory.Remove(target);
            destinationInventory.Add(target);
        }


        /// <summary> Generates interactions for a player upon their rooms' contents changing. </summary>
        public void OnRoomUpdate(Player targetPlayer, PropertyChangedEventArgs roomChangedData)
        {
            if (roomChangedData.newProperty.PropertyName == "ItemContainer.Added")
            {

            }

            if (roomChangedData.newProperty.PropertyName == "ItemContainer.Removed")
            {

            }
        }
    }
}
