using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.EntityProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Little_Choice_Based_RPG.Types.InteractDelegate
{
    /// <summary> Describes parameters and what they should be sourced from. </summary>
    public enum DelegateParameter
    {
        //Syntax: Target_Type -- The type will be used by the delegate/method. The target tells the ChoiceHandler where to retrieve that type. 
        // i.e. AllNearby_MetalCabinets_List_GameObject should return a list of gameobject that match the target description (List<GameObject>).

        //General GameObject
        Source_GameObject,
        Target_GameObject, //Target means asking the player to decide what to put in. // Issue: how to differentiate roomtarget or inventorytargets

        //General PropertyHandler
        Source_PropertyHandler,
        Target_PropertyHandler,

        //Flitered PropertyHandler
        // -- Filters apply to the target. 
        // -- i.e. Target_PropertyHandler_Filtered would mean a propertyhandler from a target gameobject in the local room filtered by having a list of properties.
        Target_PropertyHandler_Filtered
    }

    public static class DelegateValidation
    {
        private static Dictionary<string, List<DelegateParameter>> validDelegates = new Dictionary<string, List<DelegateParameter>>();

        public static void CreateValidDelegate(string setDelegateName, List<DelegateParameter> setDelegateParameters)
        {
            if (!IsValidDelegateName(setDelegateName))
                validDelegates.Add(setDelegateName, setDelegateParameters);
            else
                throw new ArgumentException("Duplicate ValidDelegate name. Tried to add a ValidDelegate which already exists!");
        }

        /// <summary> Tests if a delegate name exists. </summary>
        public static bool IsValidDelegateName(string delegateName) => validDelegates.ContainsKey(delegateName);

        public static List<DelegateParameter> GetDelegateParameters(string delegateName)
        {
            if (!validDelegates.ContainsKey(delegateName))
                throw new ArgumentException($"This validDelegate does not exists! There is no validDelegate named {delegateName}.");

            return validDelegates.GetValueOrDefault(delegateName);
        }

        /// <summary> Tests if a delegate exists - whether both its name and parameters are valid. </summary>
        public static bool IsValidDelegate(string testName, List<DelegateParameter> testParameters)
        {
            if (IsValidDelegateName(testName))
            {
                List<DelegateParameter> validParameters = validDelegates[testName];

                if (validParameters == testParameters)
                   return true;
                else
                    throw new Exception("This test returned false! The given list of validParameters do not match! This test may not be allowed to return false.");
            }
            else
                throw new ArgumentException($"This validDelegate does not exists! There is no validDelegate named {testName}.");
        }
    }
}
