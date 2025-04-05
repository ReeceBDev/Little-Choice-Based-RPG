using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.EntityProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.Interactions.InteractDelegate
{

    /// <summary> If sent, represents a request for a GameObject with the given filters and description. </summary>
    public record struct GameObjectRequest(string description, EntityProperty[]? filters = null);

    /// <summary> Retrives information from sources outlined in InteractionValidation. </summary>
    internal class InteractionParameterHandler
    {
        private delegate GameObject GetGameObjectDelegate();

        private static Dictionary<InteractionParameter, Delegate> parameterHandlers = new Dictionary<InteractionParameter, Delegate>();

        static InteractionParameterHandler() //Validate that each InteractionParameter is handled by a method here, and create a switch statement in GetParameter with them all in.
        {
            var validDelegateParameters = Enum.GetValues(typeof(InteractionParameter));

            foreach (var enumName in validDelegateParameters)
            {
                string delegateName = string.Concat("get", enumName, "Delegate");
                string methodName = string.Concat("Get", enumName);

                Regex grabTypeName = new Regex("(?<=_)[A-Za-z]*"); //Remember to throw guard clauses before allowing this to execute, to check that the InteractionParameter is formatted in a way that this class understands. It doesn't need to be a comprehensive validation.
                string returnTypeName = grabTypeName.Match(enumName.ToString()).Value;
                //Type returnType = //reflection to turn the returnTypeName into the actual type.
                    
                    //Imitate the below with reflection. This should create delegates of different return types.
                //Delegate getTarget_GameObjectDelegate = Delegate.CreateDelegate(typeof(GameObject), GetTarget_GameObject, "getTarget_GameObjectDelegate");
            }

            // Do reflection to create these two lines automatically:
            //GetGameObjectDelegate getTarget_GameObjectDelegate = GetTarget_GameObject;
            //parameterHandlers.Add(InteractionParameter.Target_GameObject, getTarget_GameObjectDelegate);
        }
        
        /*
        public static T GetParameter<T>(InteractionParameter requiredParameter) // Entry point. Should basically be a dynamically written switch statement pointing to delegates of other methods.
        {
            if (!parameterHandlers.ContainsKey(requiredParameter))
                throw new ArgumentException($"The requiredParameter of {requiredParameter} doesn't match a handler of its type in this.paramaterHandlers.");

            KeyValuePair<InteractionParameter, Delegate> relevantEntry = parameterHandlers.First(requiredParameter);
            Delegate relevantDelegate = relevantEntry.Value;

            if (T != relevantDelegate.ReturnType())
                throw new ArgumentException($"The desired type of {T} declared at GetParameter<T> doesn't match the return type of the delegate {relevantDelegate} that we have stored for requiredParameter {requiredParameter}. The delegate wanted type {relevantDelegate.ReturnType()}");


            List<InteractionParameter> requiredParameters = InteractionValidation.GetDelegateParameters(selectedInteraction.GetType().Name.ToString());


            //Invoke the delegate that we have stored for this InteractionParameter.
            T retrievedParameter = relevantEntry.Value.Invoke();

            return retrievedParameter;
        }

        private static GameObject GetTarget_GameObject()
        {
            //Launchs an event requesting user input, maybe sending the struct GameObjectRequest at the top of this document?
        }
        */
    }
}
