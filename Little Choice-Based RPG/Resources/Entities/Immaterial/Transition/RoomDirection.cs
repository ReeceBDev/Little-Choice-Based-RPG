using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.EntityProperty;
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
        private string defaultDescriptor = $"Feet scraping flecks of shrubbery drowned in dust scrambling from cracked sandstone, you forge your way further across the desolation.";
        public RoomDirection(string setDirectionName, uint setDestinationRoomID, string setTravelDescriptor = "", uint setDependentObjectID = 0) : base(setDirectionName)
        {
            DestinationRoomID = setDestinationRoomID;
            TravelDescriptor = (setTravelDescriptor == "") ? defaultDescriptor : setTravelDescriptor;

            if (setDependentObjectID > 0)
            {
                DirectionIsVisible = false;
                DependentObjectID = setDependentObjectID;
            }

            entityProperties.CreateProperty("isImmaterial", true);
        }

        public uint GetDestinationID(GameObject? usingKey = null) // Returns 0 if the key does not match DependentObjectID. usingKey should be the calling object.
        {
            uint keyID = (usingKey != null) ? usingKey.ID : 0;

            return (keyID == DependentObjectID) ? DestinationRoomID : 0;
        }

        
        public string TravelDescriptor { get; private set; }
        public bool DirectionIsVisible { get; private set; } = true;
        public uint DependentObjectID { get; private set; } = 0; //0 must be Invalid because there is no objectID that can be 0.
        private uint DestinationRoomID { get; set; }
    }
}
