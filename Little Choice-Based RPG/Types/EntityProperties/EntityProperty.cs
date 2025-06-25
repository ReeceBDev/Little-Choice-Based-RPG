namespace Little_Choice_Based_RPG.Types.EntityProperties 
{
    /// <summary> Represents a string key and its associated 
    /// . The keys and values must match their schema in PropertyValidation. </summary>s
    public struct EntityProperty
    {
        /// <summary> Represents a string key and its associated value. The keys and values must match their schema in PropertyValidation. </summary>
        public EntityProperty(string setPropertyName, Object setPropertyValue, bool setIsFrozen = false)
        {
            SetPropertyName(setPropertyName); // Name must be set before Value, or else the value will not know what name to validate against.
                    SetPropertyValue(setPropertyValue);
            ReadOnly = setIsFrozen; // ReadOnly must be set at the end, or else the property might be ReadOnly before the value can be set.
        }

        /// <summary> Sets the property value. The type must match the type allocated to this properties property name. The property name and their type are defined in PropertyValidation. </summary>
        public void SetPropertyValue(object setPropertyValue)
        {
            if (!PropertyValidation.IsValidPropertyType(setPropertyValue))
                throw new ArgumentException($"PropertyType of {setPropertyValue.GetType()} on {setPropertyValue} not valid. Tried to set a value for an EntityProperty that doesn't match a valid PropertyType!");

            if (!PropertyValidation.IsValidPropertyValue(this.PropertyName, setPropertyValue))
                throw new ArgumentException($"PropertyType {setPropertyValue.GetType()} on {setPropertyValue}, doesn't match the ValidProperty type assigned to {PropertyName}. Tried to set a value for an EntityProperty that doesn't match its required PropertyType!");

            if (ReadOnly)
                throw new ArgumentException("This property is set to ReadOnly. Unable to set the property value!");
                    
            PropertyValue = setPropertyValue;
        }

        /// <summary> Stops this property from being modified. Sets this property as ReadOnly = true;. </summary>
        public void SetPropertyAsReadOnly() => ReadOnly = true;

        /// <summary> Make this property modifiable. Sets this property as ReadOnly = false; </summary>
        public void UnsetPropertyAsReadOnly() => ReadOnly = false;

        /// <summary> Changes the property name to that of another ValidProperty. </summary>
        private void SetPropertyName(string setPropertyName)
        {
            if (PropertyValidation.IsValidPropertyName(setPropertyName))
                PropertyName = setPropertyName;
            else
                throw new ArgumentException($"Name {setPropertyName} not valid. Tried to name an EntityProperty without a matching ValidProperty name! \n\nNote: If this was a Component, make sure: \n1. The name matched a real component class type. \n2. The class inherits from PropertyLogic, where the Component.Type ValidProperties are created. \n3. The required component system is running its static constructor before being called!");
        }

        /// <summary> The name of the property. This must match an valid property name in PropertyValidation. </summary>
        public string PropertyName { get; private set; }

        /// <summary> The value of the property. This must match the PropertyName's correlated value in PropertyValidation. </summary>
        public Object PropertyValue { get; private set; }

        /// <summary> Declares if a property is modifiable and deletable. </summary>
        public bool ReadOnly { get; private set; }
    }
}