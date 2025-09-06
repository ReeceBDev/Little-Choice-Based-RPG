using Little_Choice_Based_RPG.Types.PropertySystem.Archive;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using Little_Choice_Based_RPG.Types.PropertySystem.Systems;
using Little_Choice_Based_RPG.Types.PropertySystem.Systems.SystemEventBus;
using Little_Choice_Based_RPG.Types.TypedEventArgs.PropertyContainerEventArgs;

namespace Little_Choice_Based_RPG.Resources.Systems.PlayerSystems
{
    /// <summary> Implements player-specific logic.</summary>
    internal class PlayerSystem : PropertySystem
    {
        public override void InitialiseNewSubscriber(IPropertyContainer sourceContainer, PropertyStore sourceProperties)
        {
            //The subscriber requires Component.PrivateInteractionsSystem.
            if (!sourceProperties.HasExistingPropertyName("Component.PrivateInteractionsSystem"))
            {
                sourceProperties.CreateProperty("Component.PrivateInteractionsSystem", true); //Creates the PrivateInteractions component.
                SystemSubscriptionEventBus.Subscribe(sourceContainer, "PrivateInteractionsSystem"); //Subscribes the source to the component.
            }
        }

        protected override void OnObjectChanged(object sender, ObjectChangedEventArgs objectChangedData)
        {
        }
    }
}
