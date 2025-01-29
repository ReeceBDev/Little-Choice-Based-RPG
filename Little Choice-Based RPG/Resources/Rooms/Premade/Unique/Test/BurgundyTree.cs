﻿using Little_Choice_Based_RPG.Objects.Base;
using Little_Choice_Based_RPG.Resources.Entities.Base;
using Little_Choice_Based_RPG.Resources.Entities.Derived.Furniture;
using Little_Choice_Based_RPG.Resources.Entities.Derived.Plants;
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
        private protected uint chairEntityID;
        private protected uint treeEntityID;

        private protected override List<RoomDescriptor> CheckDescriptorConditions()
        {
           List<RoomDescriptor> activatedRoomDescriptors = new();
            GameObject? correctChair = null;
            GameObject? correctTree = null;

            foreach (GameObject entity in roomEntities)
            {
                if (entity.ID == chairEntityID)
                    correctChair = entity;
                if (entity.ID == treeEntityID)
                    correctTree = entity;
            }

            if (correctTree != null)
            {
                List<uint> condition1EIDs = new([chairEntityID]);
                RoomDescriptor condition1 = new RoomDescriptor("Gently falling from their branches, the occasional leaf drifts as a feather to rest upon a chair that sits beside the tree, carved from the same oakwood as the trunk it rests against.", condition1EIDs);
            }

            if (correctTree != null && correctChair != null)
            {
                    List<uint> condition2EIDs = new ([treeEntityID, chairEntityID]);
                    RoomDescriptor condition2 = new RoomDescriptor("Gently falling from their branches, the occasional leaf drifts as a feather to rest upon a chair that sits beside the tree, carved from the same oakwood as the trunk it rests against.", condition2EIDs);
            }

            return activatedRoomDescriptors;
        }

        /*
Chair Sits Alone Without A Tree:
Dependent on Tree = Not Present, Chair being Untouched
"Burgundy leaves drift in the wind around the base of a mottled old chair that sits alone half in the sand, tilted and angled like it had been hammered part-way into the ground by some kind of a titan."


Chair Sits with a Burnt Tree:
Dependent on Tree = Burnt, Chair being Untouched
"Burgundy leaves drift in the wind around a burnt husk, the trunk of a charred and forgotten tree holding up a singed chair. Perhaps this was a nice spot to sit and read, once."
*/

        public BurgundyTree(string setName, RoomType setRoomType, string setDefaultDescriptor) : base(setName, setRoomType, setDefaultDescriptor)
        {
            this.Name = "A Burgundy Tree";
            this.roomType = RoomType.Desert;
            genericDescriptor = new RoomDescriptor("Arid desert sand whips by your skin. A few burgundy leaves drift across the floor.");

            Chair burgundyWoodChair = new Chair("Burgundy Chair", RoomID, "You are looking at a cool chair", "A chair sits here.");
            Tree burgundyWoodTree = new Tree("Burgundy Tree", RoomID, "That's a burgundy-leaved tree.", "A tree is here. It's cool. And burgundy.");

            chairEntityID = burgundyWoodChair.ID;
            treeEntityID = burgundyWoodTree.ID;

            roomEntities.Add(burgundyWoodChair);
            roomEntities.Add(burgundyWoodTree);
        }
    }
}
