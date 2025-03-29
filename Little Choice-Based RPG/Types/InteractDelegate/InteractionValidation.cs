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
    public static class InteractionValidation
    {
        private static Dictionary<string, List<InteractionParameter>> validDelegates = new Dictionary<string, List<InteractionParameter>>();

        public static void CreateValidDelegate(string setDelegateName, List<InteractionParameter> setDelegateParameters)
        {
            if (!IsValidDelegateName(setDelegateName))
                validDelegates.Add(setDelegateName, setDelegateParameters);
            else
                throw new ArgumentException("Duplicate ValidDelegate name. Tried to add a ValidDelegate which already exists!");
        }

        /// <summary> Tests if a delegate name exists. </summary>
        public static bool IsValidDelegateName(string delegateName) => validDelegates.ContainsKey(delegateName);

        public static List<InteractionParameter> GetDelegateParameters(string delegateName)
        {
            if (!validDelegates.ContainsKey(delegateName))
                throw new ArgumentException($"This validDelegate does not exists! There is no validDelegate named {delegateName}.");

            return validDelegates.GetValueOrDefault(delegateName);
        }

        /// <summary> Tests if a delegate exists - whether both its name and parameters are valid. </summary>
        public static bool IsValidDelegate(string testName, List<InteractionParameter> testParameters)
        {
            if (IsValidDelegateName(testName))
            {
                List<InteractionParameter> validParameters = validDelegates[testName];

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
