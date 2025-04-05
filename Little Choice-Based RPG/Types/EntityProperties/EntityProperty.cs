using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.EntityProperties 
{ 
    public struct EntityProperty
    {
        public EntityProperty(string setPropertyName, Object setPropertyValue, bool setIsFrozen = false)
        {
            SetPropertyName(setPropertyName); // Name must be set before Value, or else the value will not know what name to validate against.
            SetPropertyValue(setPropertyValue);
            ReadOnly = setIsFrozen; // ReadOnly must be set at the end, or else the property might be ReadOnly before the value can be set.
        }

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

        public void SetPropertyAsReadOnly() => ReadOnly = true;

        public void UnsetPropertyAsReadOnly() => ReadOnly = false;

        private void SetPropertyName(string setPropertyName)
        {
            if (PropertyValidation.IsValidPropertyName(setPropertyName))
                PropertyName = setPropertyName;
            else
                throw new ArgumentException($"Name {setPropertyName} not valid. Tried to name an EntityProperty without a matching ValidProperty name!");
        }

        public string PropertyName { get; private set; }

        public Object PropertyValue { get; private set; }
        public bool ReadOnly { get; private set; }
    }
}