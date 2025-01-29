using Little_Choice_Based_RPG.Objects.Base;
using Little_Choice_Based_RPG.Resources.Entities.Base;
using Little_Choice_Based_RPG.Resources.Entities.Derived.Furniture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Rooms.Premade.Unique.Test
{
    internal class BurgundyTree : Room
    {
        private protected bool isChairUntouched;
        private protected uint chairEntityID;
        public void SetConditionChairID(uint chairID)
        {
            chairEntityID = chairID;
        }
        public string CheckCondition1()
        {
           if (isChairUntouched == true)
            {
                currentRoomDescriptors.Add("Gently falling from their branches, the occasional leaf drifts as a feather to rest upon a chair that sits beside the tree, carved from the same oakwood as the trunk it rests against.");
            }
        }

        public BurgundyTree()
        {
            Chair burgundyWoodChair = new Chair("burgundyWoodChair", RoomID, "Wow what a cool chair", "A chair sits here.");
            chairEntityID = burgundyWoodChair.ID;
        }
        Chair Sits Alone Without A Tree:
Dependent on Tree = Not Present, Chair being Untouched
"Burgundy leaves drift in the wind around the base of a mottled old chair that sits alone half in the sand, tilted and angled like it had been hammered part-way into the ground by some kind of a titan."


Chair Sits with a Burnt Tree:
Dependent on Tree = Burnt, Chair being Untouched
"Burgundy leaves drift in the wind around a burnt husk, the trunk of a charred and forgotten tree holding up a singed chair. Perhaps this was a nice spot to sit and read, once."
    }
}
