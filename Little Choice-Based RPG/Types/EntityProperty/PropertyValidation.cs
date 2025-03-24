using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
namespace Little_Choice_Based_RPG.Types.EntityProperty
{
    enum PropertyType //Defines all possible types that may be contained within a property. Properties only contain one PropertyType each.
    {
        Boolean,
        String,
        UInt32
    }

    internal static class PropertyValidation //Defines all valid properties for the program. These properties may be applied to GameObjects, Rooms, etc.
    {
        private static Dictionary<string, PropertyType> validProperties = new Dictionary<string, PropertyType>()
        {
            //Defines default Properties.
            {"Type", PropertyType.String},
            {"IsImmaterial", PropertyType.Boolean},

            //Strength System
            {"IsWeightBearing", PropertyType.Boolean},
            {"WeightInKG", PropertyType.UInt32},
            {"StrengthInKG", PropertyType.UInt32},
            {"TotalWeightHeldInKG", PropertyType.UInt32},

            //Living
            {"IsAlive", PropertyType.Boolean},
            {"Health", PropertyType.UInt32},

            //Player
            {"CanSee", PropertyType.Boolean},
            {"CanMove", PropertyType.Boolean},
            {"IsKnockedDown", PropertyType.Boolean},

            //Object Damage
            {"IsBroken", PropertyType.Boolean},
            {"IsBurnt", PropertyType.Boolean},

            //Descriptor System
            {"Descriptor.Generic", PropertyType.String},
            {"Descriptor.Inspect", PropertyType.String},
            {"Descriptor.Equip", PropertyType.String},
            {"Descriptor.Unequip", PropertyType.String},

            //DavodianMk1Helmet Specific
            {"IsAudioBroken", PropertyType.Boolean}
        };

        public static void CreateValidProperty(string setPropertyName, PropertyType setPropertyType) //Defines additional properties.
        {
            if (!IsValidPropertyName(setPropertyName))
                validProperties.Add(setPropertyName, setPropertyType);
            else
                throw new ArgumentException("Duplicate ValidProperty name. Tried to add a ValidProperty which already exists!");
        }

        public static bool IsValidPropertyType(object propertyValueType) => Enum.IsDefined(typeof(PropertyType), propertyValueType.GetType().Name.ToString()); //Tests if a property value's type exists.
        public static bool IsValidPropertyName(string propertyName) => validProperties.ContainsKey(propertyName); //Tests if a property name exists.
        public static bool IsValidPropertyValue(string propertyName, object propertyValueType)  //Tests if a property value aligns with its associated name's required type.
        {
            if (IsValidPropertyName(propertyName))
            {
                return validProperties.GetValueOrDefault(propertyName).ToString() == propertyValueType.GetType().Name.ToString();
            }
            else
                throw new ArgumentException("Name not found. Tried to search for a ValidProperty name which doesn't exist!");
        }

        public static bool IsValidProperty(string propertyName, object propertyValueType)  //Tests if a property exists - whether both its name and type are valid.
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
