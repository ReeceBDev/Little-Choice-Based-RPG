using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.EntityProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Little_Choice_Based_RPG.Resources.Entities.Immaterial.Transition
{
    public class RoomDirection : Immaterial
    {

        private readonly static Dictionary<string, PropertyType> requiredProperties = new Dictionary<string, PropertyType>()
        {
            {"Direction.IsVisible", PropertyType.Boolean},
            {"Direction.DestinationRoomID", PropertyType.UInt32 },

            {"Descriptor.Direction.Travelling", PropertyType.String}
        };

        private readonly static Dictionary<string, PropertyType> optionalProperties = new Dictionary<string, PropertyType>()
        {
            {"Direction.AssociatedObjectID", PropertyType.UInt32},
        };

        private readonly static Dictionary<string, object> defaultProperties = new Dictionary<string, object>()
        {
            {"Direction.IsVisible", true},
            {"Descriptor.Direction.Travelling", "Feet scraping flecks of shrubbery drowned in dust scrambling from cracked sandstone, you forge your way further across the desolation."}
        };

        static RoomDirection()
        {
            //Define new required and optional ValidProperties for this class
            DeclareNewPropertyTypes(requiredProperties);
            DeclareNewPropertyTypes(optionalProperties);
        }

        private protected RoomDirection(PropertyHandler? derivedProperties = null)
            : base(SetLocalProperties(derivedProperties ??= new PropertyHandler()))
        {
            // When the direction is associated with an object, make it invisible by default.
            // This forces the implementing object to reveal the direction when accessible, i.e. a Door being open.
            if (entityProperties.HasExistingPropertyName("Direction.AssociatedObjectID"))
                entityProperties.UpsertProperty("Direction.IsVisible", false);

            //Validate required properties have been set on entityProperties
            ValidateRequiredProperties(requiredProperties);
        }

        private static PropertyHandler SetLocalProperties(PropertyHandler derivedProperties)
        {
            //Apply default properties for this class to the current list of derivedProperties
            ApplyDefaultProperties(derivedProperties, defaultProperties);

            return derivedProperties; //Return is required to give (base) the derived list.
        }


        public uint GetDestinationID(uint? associatedObjectID = null) // associatedObjectID should be the calling object. Throws an exception if the associatedObjectID does not match the one stored on this direction in entityProperties.
        {
            //If no associated object is required, return 
            if (!entityProperties.HasExistingPropertyName("Direction.AssociatedObjectID"))
                return (uint) entityProperties.GetPropertyValue("Direction.DestinationRoomID");

            //Check that the associated object matches the one stored in entityProperties
            if (associatedObjectID != (uint)entityProperties.GetPropertyValue("Direction.AssociatedObjectID"))
                throw new Exception("The associated object ID did not match the one stored in entityProperties!");

            return (uint)entityProperties.GetPropertyValue("Direction.DestinationRoomID");
        }
    }
}
