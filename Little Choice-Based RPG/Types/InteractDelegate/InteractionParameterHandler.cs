using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.InteractDelegate
{
    internal class InteractionParameterHandler ///I want this class to be responsible for finding and putting together information sources outlined in DelegateValidation.
    {

        private static Dictionary<InteractionParameter, Delegate> parameterHandlers = new Dictionary<InteractionParameter, Delegate>()
        {
            { InteractionParameter.Source_GameObject, handleSource_GameObjectDelegate }
        };

        static InteractionParameterHandler() //Validate that each DelegateParameter is handled by a method here, and create a switch statement in GetParameter with them all in.
        {
            Enum[] validDelegateParameters = GetValues(DelegateParameter));

            foreach (var enumName in Enum.GetValues(typeof(InteractionParameter)))
            {
                Delegate.CreateDelegate(typeof(object), this, "handleSource_GameObjectDelegate", handleSource_GameObject);
            }
        }
        
        public static T GetParameter<T>(InteractionParameter requiredParameter) // Entry point. Should basically be a dynamically written switch statement pointing to delegates of other methods.
        {
            parameterHandlers.TryGetValue(requiredParameter).Invoke();
        }

        private static object handleSource_GameObject()
        {   

        }


    }
}
