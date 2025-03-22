using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
namespace Little_Choice_Based_RPG.Types.EntityProperty
{
    public enum PropertyType //Defines all possible types that may be contained within a property. Properties only contain one PropertyType each.
    {
        Bool,
        String
    }

    internal class PropertyDeclarations //Defines all possible properties for the program. These properties may be applied to GameObjects, Rooms, etc.
    {
        public PropertyDeclarations()
        {
            MetaProperties();
            FlammableProperties();
        }

        private protected void MetaProperties()
        {
            CreateProperty("isImmutable", PropertyType.Bool);
        }

        private protected void FlammableProperties()
        {
            CreateProperty("isBurnt", PropertyType.Bool);
            CreateProperty("isBurnt", PropertyType.Bool);
        }

        private protected void CreateProperty(string setPropertyName, PropertyType setPropertyType)
        {
            if (PropertyNames.ContainsKey(setPropertyName))
                throw new InvalidOperationException("This property already exists!");
            else
                PropertyNames.Add(setPropertyName, setPropertyType);
        }

        public Dictionary<string, PropertyType> PropertyNames {get; private set;} = new Dictionary<string, PropertyType>();
    }
}
*/
namespace Little_Choice_Based_RPG.Types.EntityProperty
{
    enum PropertyType //Defines all possible types that may be contained within a property. Properties only contain one PropertyType each.
    {
        Bool,
        String
    }

    internal static class PropertyDeclarations //Defines all valid properties for the program. These properties may be applied to GameObjects, Rooms, etc.
    {
        public static void InitialiseProperties()
        {
            CreateProperty("isImmutable", PropertyType.Bool);
            CreateProperty("isBurnt", PropertyType.Bool);
            CreateProperty("isBurnt", PropertyType.Bool); // Intentionally should throw an error. As a test.
        }

        private static void CreateProperty(string setPropertyName, PropertyType setPropertyType)
        {
            if (ValidProperties.ContainsKey(setPropertyName))
                throw new InvalidOperationException("This property already exists!");
            else
                ValidProperties.Add(setPropertyName, setPropertyType);
        }

        public static Dictionary<string, PropertyType> ValidProperties { get; private set; } = new Dictionary<string, PropertyType>()
        {
            { "isImmutable", PropertyType.Bool },
            { "isBurnt", PropertyType.Bool }
        };
    }
}
