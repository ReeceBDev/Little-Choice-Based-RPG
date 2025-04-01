using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
namespace Little_Choice_Based_RPG.Types.EntityProperties
{
    /// <summary> Defines all valid properties for the program. These properties may be applied to GameObjects, Rooms, etc. </summary>
    internal static class PropertyValidation 
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
            {"IsBurnt", PropertyType.Boolean},

            //Descriptor System
            {"Descriptor.Generic.Current", PropertyType.String}, //The one the object is currently using
            {"Descriptor.Inspect.Current", PropertyType.String}, //The one the object is currently using

            {"Descriptor.Generic.Default", PropertyType.String},
            {"Descriptor.Inspect.Default", PropertyType.String},
            {"Descriptor.Equip", PropertyType.String},
            {"Descriptor.Unequip", PropertyType.String},

            //DavodianMk1Helmet Specific
            {"IsAudioBroken", PropertyType.Boolean}
        };

        /// <summary> Defines additional properties. </summary>
        public static void CreateValidProperty(string setPropertyName, PropertyType setPropertyType)
        {
            if (!IsValidPropertyName(setPropertyName))
                validProperties.Add(setPropertyName, setPropertyType);
            else
                throw new ArgumentException("Duplicate ValidProperty name. Tried to add a ValidProperty which already exists!");
        }

        /// <summary> Tests if a property value's type exists. </summary>
        public static bool IsValidPropertyType(object propertyValueType) => Enum.IsDefined(typeof(PropertyType), propertyValueType.GetType().Name.ToString());

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
