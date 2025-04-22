using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Resources.PropertyContainerEventArgs;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PrivateInteractionsSystems.PrivateInteractionsExtensions;
using Little_Choice_Based_RPG.Resources.Systems.SystemEventBus;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems
{
    /// <summary> Provide basic access to property logic contained within systems derived from this class. </summary>
    public abstract class PropertyLogic
    {
        public PropertyLogic()
        {
            //Create unique components for each derived system
            if (!(PropertyValidation.IsValidProperty($"Component.{this.GetType().Name.ToString()}", PropertyType.Boolean)))
                PropertyValidation.CreateValidProperty($"Component.{this.GetType().Name.ToString()}", PropertyType.Boolean);

            //Listen for new property containers which try to use systems
            SystemSubscriptionEventBus.SystemSubcriptionRequest += OnSystemSubscriptionRequest;
        }
        
        protected virtual void OnSystemSubscriptionRequest(object sender, SystemSubscriptionRequestEventArgs systemSubscriptionRequestData)
        {
            //If the current system type matches the reference name, subscribe to the requester's property list.
            if (systemSubscriptionRequestData.systemReferenceName == this.GetType().Name.ToString())
            {
                //Subscribe to the requesting object's change event
                systemSubscriptionRequestData.targetPropertyContainer.ObjectChanged += OnObjectChanged;

                //Initialise the subscriber
                InitialiseNewSubscriber(systemSubscriptionRequestData.targetPropertyContainer, systemSubscriptionRequestData.targetPropertyContainer.Properties);
            }
        }

        /// <summary> Provide logic for co-ordinating property changes with their relevant methods. </summary>
        protected abstract void OnObjectChanged(object sender, ObjectChangedEventArgs propertyChangedData);

        /// <summary> Apply starting implementation to a subscription. This may include handling the initial interactions based on its initial property state. </summary>
        protected abstract void InitialiseNewSubscriber(PropertyContainer sourceContainer, PropertyHandler sourceProperties);
    }
}
