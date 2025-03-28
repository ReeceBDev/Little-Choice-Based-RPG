using Little_Choice_Based_RPG.Resources.Choices;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.EntityProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.InteractDelegate
{
    public static class InteractDelegates //Declares All Interact Delegates
    {
        public delegate void InteractOnSourceProperties(PropertyHandler Source_PropertyHandler);
        public delegate void InteractOnSourcePropertiesUsingTargetObject(PropertyHandler Source_PropertyHandler, GameObject Target_GameObject);

        static InteractDelegates()
        {
            // Use reflection to make these. I can then validate them, too, to see if they match perfectly the DelegateParameters.
            // Doing it this way around lets the delegates also be strongly typed and already seen by VS :)
            //
            // I CAN ALSO then use reflection to check if all the delegateValidations are handled by ChoiceHandler :)
            DelegateValidation.CreateValidDelegate("InteractOnSourceProperties", [DelegateParameter.Source_PropertyHandler]);
            DelegateValidation.CreateValidDelegate("InteractOnSourcePropertiesUsingTargetObject", [DelegateParameter.Source_PropertyHandler, DelegateParameter.Target_GameObject]);
        }
    }


    public class ChoiceAction
    {
        private readonly Delegate _action;

        // Constructor for a one-parameter action
        public ChoiceAction(Action<PropertyHandler> action)
        {
            _action = action;
        }

        // Constructor for a two-parameter action
        public ChoiceAction(Action<PropertyHandler, GameObject> action)
        {
            _action = action;
        }

        // Method to invoke the action
        public void Invoke(ChoiceHandler handler)
        {
            if (_action is Action<PropertyHandler> action1)
            {
                // Populate parameter for one-parameter action
                PropertyHandler sourceProperty = DelegateHandler.GetParameter<PropertyHandler>(DelegateParameter.Source_PropertyHandler);
                action1(sourceProperty);
            }
            else if (_action is Action<PropertyHandler, GameObject> action2)
            {
                // Populate parameters for two-parameter action
                var sourceProperty = DelegateHandler.GetParameter(DelegateParameter.Source_PropertyHandler);
                var targetObject = DelegateHandler.GetParameter(DelegateParameter.Target_GameObject);
                action2(sourceProperty, targetObject);
            }
            else
            {
                throw new InvalidOperationException("Unsupported action type.");
            }
        }
    }
}
