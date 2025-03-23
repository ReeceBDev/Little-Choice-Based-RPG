using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.EntityProperty
{
    public class PropertyHandler
    {
        public void CreateProperty(string setPropertyName, object setPropertyValue) // Creates a new property
        {
            EntityProperties.Add(new EntityProperty(setPropertyName, setPropertyValue));
        }

        public void CreateProperty(string setPropertyName, object setPropertyValue, bool isReadOnly) // Creates a new property with a ReadOnly value
        {
            EntityProperties.Add(new EntityProperty(setPropertyName, setPropertyValue, isReadOnly));
        }

        public void UpdateProperty(string propertyName, object setPropertyValue) // Updates a property which is not frozen.
        {
            EntityProperty targetProperty = FetchPropertyByName(propertyName);
            if (!IsPropertyReadOnly(propertyName))
                targetProperty.SetPropertyValue(setPropertyValue);
            else
                throw new ArgumentException("Can't update this property. This EntityProperty is set to ReadOnly. (ReadOnly = True)");
        }

        public void UpdateProperty(string propertyName, object setPropertyValue, bool isReadOnly) // Updates a property which is not frozen, including its new frozen value.
        {
            if (!IsPropertyReadOnly(propertyName))
            {
                UpdateProperty(propertyName, setPropertyValue);

                if (isReadOnly)
                    FreezeProperty(propertyName);
                // Thaw is not required, as it will do nothing and throw an exception anyway.
            }
            else 
                throw new ArgumentException("Can't update this property. This EntityProperty is set to ReadOnly. (ReadOnly = True)");
        }

        public void UpsertProperty(string setPropertyName, object setPropertyValue) // Place this property on an object. Over-rides an existing property, unless frozen.
        {
            if (IsExistingProperty(setPropertyName))
                UpdateProperty(setPropertyName, setPropertyValue);
            else
                CreateProperty(setPropertyName, setPropertyValue);
        }

        public void UpsertProperty(string setPropertyName, object setPropertyValue, bool isReadOnly) // Place this property on an object. Over-rides an existing property, unless frozen.
        {
            if (IsExistingProperty(setPropertyName))
                UpdateProperty(setPropertyName, setPropertyValue, isReadOnly);
            else
                CreateProperty(setPropertyName, setPropertyValue, isReadOnly);
        }

        public object GetPropertyValue(string propertyName) => FetchPropertyByName(propertyName).PropertyValue; //Retrives the property value.

        public bool HasProperty(string propertyName) //Check if a property exists in EntityProperties.
        {
            foreach (EntityProperty testProperty in EntityProperties)
            {
                if (testProperty.PropertyName == propertyName)
                    return true;
            }

            return false;
        }

        public void DeleteProperty(string propertyName) //Removes a property, unless frozen.
        {
            EntityProperty targetProperty = FetchPropertyByName(propertyName);
            
            if (IsPropertyReadOnly(propertyName) == false)
                EntityProperties.Remove(targetProperty);
            else
                throw new ArgumentException("Can't delete this property. This EntityProperty is set to ReadOnly. (ReadOnly = True)");
            
        }

        public bool IsPropertyReadOnly(string propertyName) => FetchPropertyByName(propertyName).ReadOnly; // Tests if property is ReadOnly

        public void FreezeProperty(string propertyName) // Sets a property to ReadOnly = True
        {
            if (!IsPropertyReadOnly(propertyName))
                FetchPropertyByName(propertyName).FreezeProperty();
            else
                throw new ArgumentException("Can't freeze this property. This EntityProperty is already Frozen. (ReadOnly = True)");
        }

        public void ThawProperty(string propertyName) // Sets a property to ReadOnly = False
        {
            if (!IsPropertyReadOnly(propertyName))
                FetchPropertyByName(propertyName).ThawProperty();
            else
                throw new ArgumentException("Can't thaw this property. This EntityProperty is already Thawed. (ReadOnly = False)");
        }

        public bool IsExistingProperty(string propertyName)
        {
            foreach (EntityProperty testProperty in EntityProperties)
            {
                if (testProperty.PropertyName == propertyName)
                    return true;
            }

            return false;
        }

        private EntityProperty FetchPropertyByName(string propertyName) //Retrives the property itself
        {
            foreach (EntityProperty testProperty in EntityProperties)
            {
                if (testProperty.PropertyName == propertyName)
                    return testProperty;
            }

            throw new ArgumentException("No such EntityProperty exists inside this objects list of EntityProperties.");
        }

        public List<EntityProperty> EntityProperties = new List<EntityProperty>();
    }
}
