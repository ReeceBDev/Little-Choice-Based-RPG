using Little_Choice_Based_RPG.Resources.Systems.SystemEventBus;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.Interactions.InteractDelegate;
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
                //Subscribe to the requester's property list
                systemSubscriptionRequestData.targetPropertyContainer.Properties.PropertyChanged += OnPropertyChanged;

                //Initialise currently valid system choices
                GiveInitialInteractions(systemSubscriptionRequestData.targetPropertyContainer, systemSubscriptionRequestData.targetPropertyContainer.Properties);
            }
        }

        public void GiveInteraction(PropertyContainer target, IInvokableInteraction interaction)
        {
            target.Interactions.Add(interaction);
        }

        /// <summary> Provide logic for co-ordinating property changes with their relevant methods. </summary>
        protected abstract void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedData);

        /// <summary> Provide the interactions which currently apply upon loading. </summary>
        protected abstract void GiveInitialInteractions(PropertyContainer sourceContainer, PropertyHandler sourceProperties);
    }
}
