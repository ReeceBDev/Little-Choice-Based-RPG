/*namespace Little_Choice_Based_RPG.Types.PropertySystem.EntityProperties
{
    /// <summary> Defines all valid properties for the program. These properties may be applied to GameObjects, Rooms, etc. </summary>
    internal static class PropertyValidation 
    {
        /// <summary> Central repository for schemas of every valid properties that may be used. 
        /// If a property does not match a schema on this list, it cannot be used. </summary> 
        private static Dictionary<string, PropertyType> validProperties = new Dictionary<string, PropertyType>();

        /// <summary> Defines additional properties. </summary>
        public static void CreateValidProperty(string setPropertyName, PropertyType setPropertyType)
        {
            if (!IsValidPropertyName(setPropertyName))
                validProperties.Add(setPropertyName, setPropertyType);
            else
                throw new ArgumentException($"Duplicate ValidProperty name. Tried to add a ValidProperty which already exists! Offender details: Name: {setPropertyName}, PropertyType: {setPropertyType}");
        }

        /// <summary> Tests if a property value's type exists. </summary>
        public static bool IsValidPropertyType(object propertyValueType) 
            => Enum.IsDefined(typeof(PropertyType), propertyValueType.GetType().Name.ToString());

        /// <summary> Tests if a property name exists. </summary>
        public static bool IsValidPropertyName(string propertyName) => validProperties.ContainsKey(propertyName);

        /// <summary> Tests if a property value aligns with its associated name's required type. </summary>
        public static bool IsValidPropertyValue(string propertyName, object propertyValueType) 
        {
            if (IsValidPropertyName(propertyName))
            {
                return validProperties.GetValueOrDefault(propertyName).ToString() == propertyValueType.GetType().Name.ToString();
            }
            else
                throw new ArgumentException("Name not found. Tried to search for a ValidProperty name which doesn't exist!");
        }

        /// <summary> Tests if a property exists - whether both its name and type are valid. </summary>
        public static bool IsValidProperty(string propertyName, object propertyValueType) 
        {
            if (IsValidPropertyName(propertyName))
            {
                return validProperties.GetValueOrDefault(propertyName).ToString() == propertyValueType.GetType().Name.ToString();
            }
            else
                return false;
        }
    }
}
*/