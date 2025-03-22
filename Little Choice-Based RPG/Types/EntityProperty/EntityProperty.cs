using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.EntityProperty 
{ 

public struct EntityProperty
    {
        private string propertyName;
        private Object propertyValue;
        private bool isReadOnly;

        public EntityProperty(string setPropertyName, Object setPropertyValue)
        {
            isReadOnly = false;

            propertyName = setPropertyName;

            propertyValue = setPropertyValue;
        }

        public EntityProperty(string setPropertyName, Object setPropertyValue, bool setIsFrozen)
        {
            isReadOnly = setIsFrozen;

            propertyName = setPropertyName;

            attemptedPropertyValueType = setPropertyValue.GetType()

            if ( == bool)
            {
                propertyValue = setPropertyValue;
            }

        }
        public string PropertyName { get; private set; }

        public Object PropertyValue { get; private set; }
        public bool ReadOnly { get; private set; }
    }
}