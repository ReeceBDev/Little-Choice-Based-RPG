using Little_Choice_Based_RPG.Resources.Systems.SystemEventBus;
using Little_Choice_Based_RPG.Types.EntityProperties;
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
        public PropertyLogic(SystemSubscriptionEventBus systemSubscriptionEventBusReference)
        {
            //Create unique components for each derived system
            if (!(PropertyValidation.IsValidProperty($"Component.{this.GetType().Name.ToString()}", PropertyType.Boolean)))
                PropertyValidation.CreateValidProperty($"Component.{this.GetType().Name.ToString()}", PropertyType.Boolean);

            //Listen for new property containers which try to use systems
            systemSubscriptionEventBusReference.SystemSubcriptionRequest += OnSystemSubscriptionRequest;
        }

        protected virtual void OnSystemSubscriptionRequest(object sender, SystemSubscriptionRequestEventArgs systemSubscriptionRequestData)
        {
            //If the current system type matches the reference name, subscribe to the requester's property list.
            if (systemSubscriptionRequestData.systemReferenceName == this.GetType().Name.ToString())
                systemSubscriptionRequestData.targetPropertyHandler.Properties.PropertyChanged += OnPropertyChanged;
        }

        protected virtual void OnPropertyChanged(object sender, EntityProperty updatedProperty)
        {

        }
    }
}
