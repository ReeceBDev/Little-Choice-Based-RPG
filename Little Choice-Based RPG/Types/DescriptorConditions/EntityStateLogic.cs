using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.Archive;

namespace Little_Choice_Based_RPG.Types.DescriptorConditions
{
    internal static class EntityStateLogic
    {
        public static bool CheckEntityStateIsValid(EntityState testState, GameObject currentState)
        {

            if (testState.EntityReferenceID == (uint)currentState.Properties.GetPropertyValue("ID")) //check if the entityIDs match
            {
                //If no properties exist, then the object will be considered a match by default, since there are no properties being checked, just presence.
                if (testState.RequiredProperties == null)
                    return true;
                else //check for properties to match before adding as a valid state
                {
                    //Check if each property matches the required properties
                    List<EntityProperty> validProperties = new();

                    //foreach (EntityProperty currentProperty in testState.RequiredProperties)
                    foreach (EntityProperty requiredProperty in testState.RequiredProperties)
                    {
                        string propertyName = requiredProperty.PropertyName;
                        object propertyValue = requiredProperty.PropertyValue;

                        if (currentState.Properties.HasPropertyAndValue(propertyName, propertyValue))
                            validProperties.Add(requiredProperty);
                    }

                    //if all properties match, then the state is valid
                    return validProperties.Count == testState.RequiredProperties.Count;
                }
            }
            else return false; //entity IDs didnt match.
        }
    }
}
