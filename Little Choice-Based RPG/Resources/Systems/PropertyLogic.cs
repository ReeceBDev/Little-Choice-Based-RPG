using Little_Choice_Based_RPG.Resources.Entities;
using Little_Choice_Based_RPG.Resources.Systems.SystemEventBus;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.TypedEventArgs.PropertyContainerEventArgs;

namespace Little_Choice_Based_RPG.Resources.Systems
{
    /// <summary> Provide basic access to property logic contained within systems derived from this class. </summary>
    internal abstract class PropertyLogic
    {
        public PropertyLogic()
        {
            //Create unique components for each derived system
            if (!(PropertyValidation.IsValidProperty($"Component.{this.GetType().Name.ToString()}", PropertyType.Boolean)))
                PropertyValidation.CreateValidProperty($"Component.{this.GetType().Name.ToString()}", PropertyType.Boolean);

            //Listen for new property containers which try to use systems
            SystemSubscriptionEventBus.SystemSubcriptionRequest += OnSystemSubscriptionRequest; //Subscribe
            SystemSubscriptionEventBus.SystemUnsubscriptionRequest += OnSystemUnsubscriptionRequest; //Optional unsubscribe
        }
        
        protected void OnSystemSubscriptionRequest(object sender, SystemSubscriptionRequestEventArgs systemSubscriptionRequestData)
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

        protected void OnSystemUnsubscriptionRequest(object sender, SystemSubscriptionRequestEventArgs systemSubscriptionRequestData)
        {
            //If the current system type matches the reference name, unsubscribe from the requester's property list.
            if (systemSubscriptionRequestData.systemReferenceName == this.GetType().Name.ToString())
            {
                //Unsubscribe from the requesting object's change event
                systemSubscriptionRequestData.targetPropertyContainer.ObjectChanged -= OnObjectChanged;
            }
        }

        /// <summary> Provide logic for co-ordinating property changes with their relevant methods. </summary>
        protected abstract void OnObjectChanged(object sender, ObjectChangedEventArgs propertyChangedData);

        /// <summary> Apply starting implementation to a subscription. This may include handling the initial interactions based on its initial property state. </summary>
        protected abstract void InitialiseNewSubscriber(PropertyContainer sourceContainer, PropertyHandler sourceProperties);
    }
}
