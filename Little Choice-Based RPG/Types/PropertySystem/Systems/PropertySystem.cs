using Little_Choice_Based_RPG.Types.PropertySystem;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using Little_Choice_Based_RPG.Types.PropertySystem.Properties;
using Little_Choice_Based_RPG.Types.PropertySystem.Systems.SystemEventBus;
using Little_Choice_Based_RPG.Types.TypedEventArgs;
using Little_Choice_Based_RPG.Types.TypedEventArgs.PropertyContainerEventArgs;

namespace Little_Choice_Based_RPG.Types.PropertySystem.Systems
{
    /// <summary> Provides PropertyContainers with access to property logic contained within systems derived from this class. 
    /// PropertyContainers may subscribe themselves to systems derived from this class. 
    /// Watches subscribers for changes to their properties, so that property changes can trigger system events. </summary>
    internal abstract class PropertySystem : IPropertySystem
    {
        /* Old property system (V1)
        public PropertySystem()
        {
            //Listen for new property containers which try to use systems
            SystemSubscriptionEventBus.SystemSubcriptionRequest += OnSystemSubscriptionRequest; //Subscribe
            SystemSubscriptionEventBus.SystemUnsubscriptionRequest += OnSystemUnsubscriptionRequest; //Optional unsubscribe
        }

        public void SubscribeToPropertyInstance(IProperty eventPublisher, EventHandler<IPropertyChangedEventArgs> targetMethod) 
            => eventPublisher.PropertyChanged += targetMethod;

        public void UnsubscribeFromPropertyInstance(IProperty eventPublisher, EventHandler<IPropertyChangedEventArgs> targetMethod) 
            => eventPublisher.PropertyChanged -= targetMethod;

        protected void OnSystemSubscriptionRequest(object sender, SystemSubscriptionRequestEventArgs systemSubscriptionRequestData)
        {
            //If the current system type matches the reference name, subscribe to the requester's property list.
            if (systemSubscriptionRequestData.systemReferenceName == GetType().Name.ToString())
            {
                //Subscribe to the requesting object's change event
                systemSubscriptionRequestData.targetPropertyContainer.ContainerChanged += OnObjectChanged;

                //Initialise the subscriber
                InitialiseNewSubscriber(systemSubscriptionRequestData.targetPropertyContainer, systemSubscriptionRequestData.targetPropertyContainer.Properties);
            }
        }

        protected void OnSystemUnsubscriptionRequest(object sender, SystemSubscriptionRequestEventArgs systemSubscriptionRequestData)
        {
            //If the current system type matches the reference name, unsubscribe from the requester's property list.
            if (systemSubscriptionRequestData.systemReferenceName == GetType().Name.ToString())
            {
                //Unsubscribe from the requesting object's change event
                systemSubscriptionRequestData.targetPropertyContainer.ContainerChanged -= OnObjectChanged;
            }
        }

        /// <summary> Provide logic for co-ordinating property changes with their relevant methods. </summary>
        protected abstract void OnObjectChanged(object sender, ObjectChangedEventArgs propertyChangedData);
        */

        //New Property System (V2)
        /// <summary> Apply starting implementation to a subscription. This may include handling the initial interactions based on its initial property state. </summary>
        public abstract void InitialiseNewSubscriber(IPropertyContainer sourceContainer, PropertyStore sourceProperties);
    }
}
