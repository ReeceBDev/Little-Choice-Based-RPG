using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Xml;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using System.Security.Principal;
using System.Runtime.InteropServices.Marshalling;
using System.Collections.Immutable;
using System.Collections;
using System.Runtime.ConstrainedExecution;
using Little_Choice_Based_RPG.Resources.Entities.Immaterial.Transition;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.RoomSpecificTypes;
using Little_Choice_Based_RPG.Types;

namespace Little_Choice_Based_RPG.Resources.Rooms
{
    public class Room : PropertyContainer
    {
        private protected List<GameObject> roomEntities = new List<GameObject>();
        private protected List<ConditionalDescriptor> localConditionalDescriptors = new List<ConditionalDescriptor>();

        private readonly static Dictionary<string, PropertyType> requiredProperties = new Dictionary<string, PropertyType>()
        {
            {"Descriptor.Default", PropertyType.String},
        };

        private readonly static Dictionary<string, PropertyType> optionalProperties = new Dictionary<string, PropertyType>()
        {

        };

        private readonly static Dictionary<string, object> defaultProperties = new Dictionary<string, object>()
        {
            {"Name", "Default Room Test"},
            {"Component.InventorySystem", true }
        };

        static Room()
        {
            //Define new required and optional ValidProperties for this class
            DeclareNewPropertyTypes(requiredProperties);
            DeclareNewPropertyTypes(optionalProperties);
        }

        private protected Room(Dictionary<string, object>? derivedProperties = null)
            : base(SetLocalProperties(derivedProperties ??= new Dictionary<string, object>()))
        {
            //Validate required properties have been set on entityProperties
            ValidateRequiredProperties(requiredProperties);
        }

        private static Dictionary<string, object> SetLocalProperties(Dictionary<string, object> derivedProperties)
        {
            //Apply default properties for this class to the current list of derivedProperties
            ApplyDefaultProperties(derivedProperties, defaultProperties);

            return derivedProperties; //Return is required to give (base) the derived list.
        }
    }
}
