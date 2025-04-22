using Little_Choice_Based_RPG.Resources.PropertyContainerEventArgs;
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
    /// <summary> Implements player-specific logic.</summary>
    public class PlayerSystem : PropertyLogic
    {
        static PlayerSystem()
        {
            PropertyValidation.CreateValidProperty("Player.CanHear", PropertyType.Boolean);
            PropertyValidation.CreateValidProperty("Player.CanSee", PropertyType.Boolean);
        }

        protected override void InitialiseNewSubscriber(PropertyContainer sourceContainer, PropertyHandler sourceProperties)
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
