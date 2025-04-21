using Little_Choice_Based_RPG.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.EntityProperties
{
    public class PropertyHandler
    {
        /// <summary> Creates a new property on this object. </summary>
        public void CreateProperty(string setPropertyName, object setPropertyValue, bool isReadOnly = false)
        {
            EntityProperty newProperty = new EntityProperty(setPropertyName, setPropertyValue, isReadOnly);
            EntityProperties.Add(newProperty);

            OnPropertyChanged(newProperty);
        }

        /// <summary> Updates a property on this object, which is not currently frozen. </summary>
        public void ReplaceProperty(string propertyName, object setPropertyValue, bool isReadOnly = false)
        {
            if (!IsPropertyReadOnly(propertyName))
            {
                int index = GetEntityPropertyIndex(propertyName);

                EntityProperties.RemoveAt(index);
                EntityProperties.Insert(index, new EntityProperty(propertyName, setPropertyValue, isReadOnly));

                OnPropertyChanged(CopyEntityProperty(propertyName)); //This creates a new copy because EntityProperty is a value type.
            }
            else 
                throw new ArgumentException("Can't update this property. This EntityProperty is set to ReadOnly. (ReadOnly = True)");
        }

        /// <summary> Places a property on this object. Over-rides an existing property, unless frozen. </summary>
        public void UpsertProperty(string setPropertyName, object setPropertyValue, bool isReadOnly = false)
        {
            if (HasExistingPropertyName(setPropertyName))
                ReplaceProperty(setPropertyName, setPropertyValue, isReadOnly);
            else
                CreateProperty(setPropertyName, setPropertyValue, isReadOnly);
        }

        /// <summary> Removes a property from this Object. </summary>
        public void DeleteProperty(string propertyName) //Removes a property, unless frozen.
        {          
            EntityProperty targetProperty = CopyEntityProperty(propertyName);
            
            if (IsPropertyReadOnly(propertyName) == false)
                EntityProperties.Remove(targetProperty);
            else
                throw new ArgumentException("Can't delete this property. This EntityProperty is set to ReadOnly. (ReadOnly = True)");

        }

        /// <summary> Sets a property as ReadOnly. This disables modifications to the property. </summary>
        public void FreezeProperty(string propertyName)
        {
            if (!IsPropertyReadOnly(propertyName))
                EntityProperties[GetEntityPropertyIndex(propertyName)].SetPropertyAsReadOnly();
            else
                throw new ArgumentException("Can't freeze this property. This EntityProperty is already Frozen. (ReadOnly = True)");
        }

        /// <summary> Lifts the ReadOnly restriction from a property. This allows modifications to the property. </summary>
        public void ThawProperty(string propertyName) // Sets a property to ReadOnly = False
        {
            if (!IsPropertyReadOnly(propertyName))
                EntityProperties[GetEntityPropertyIndex(propertyName)].UnsetPropertyAsReadOnly();
            else
                throw new ArgumentException("Can't thaw this property. This EntityProperty is already Thawed. (ReadOnly = False)");
        }

        /// <summary> Checks if a property matches by both name and value, on this Object. </summary>
        public bool HasPropertyAndValue(string propertyName, object propertyValue)
        {
            foreach (EntityProperty testProperty in EntityProperties)
            {
                if (testProperty.PropertyName == propertyName)
                    if (testProperty.PropertyValue.Equals(propertyValue))
                        return true;
            }

            return false;
        }

        /// <summary> Checks if a property exists on this object. Cannot be used to check for values. </summary>
        public bool HasExistingPropertyName(string propertyName)
        {
            foreach (EntityProperty testProperty in EntityProperties)
            {
                if (testProperty.PropertyName == propertyName)
                    return true;
            }

            return false;
        }

        /// <summary> Returns a value from a matching property name on this object. </summary>
        public object GetPropertyValue(string propertyName) => CopyEntityProperty(propertyName).PropertyValue;


        /// <summary> Checks if property is modifiable. </summary>
        public bool IsPropertyReadOnly(string propertyName) => CopyEntityProperty(propertyName).ReadOnly;


        /// <summary> Returns a reference to an EntityProperty from a matching property name on this object. </summary>
        private EntityProperty CopyEntityProperty(string propertyName)
        {
            foreach (EntityProperty testProperty in EntityProperties)
            {
                if (testProperty.PropertyName == propertyName)
                    return testProperty;
            }

            throw new ArgumentException("No such EntityProperty exists inside this objects list of EntityProperties.");
        }

        private int GetEntityPropertyIndex(string propertyName)
        {
            if (!HasExistingPropertyName(propertyName))
                throw new ArgumentException($"The name {propertyName} was not found on this object's EntityProperties {EntityProperties}!");

            for (int index = 0; index < EntityProperties.Count(); index++)
            {
                if (EntityProperties[index].PropertyName == propertyName)
                    return index;
            }

            throw new Exception("The index didn't match any names that were checked. This shouldn't have happened!");
        }

        public event EventHandler<EntityProperty> PropertyChanged;
        protected private void OnPropertyChanged(EntityProperty updatedProperty) => PropertyChanged?.Invoke(this, updatedProperty);
        public List<EntityProperty> EntityProperties { get; private set; } = new List<EntityProperty>();
    }
}
