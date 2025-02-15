using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Furniture;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Plants;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Rooms.Premade.Unique.Test
{
    internal class BurgundyTree : Room
    {
        private protected uint chairEntityID;
        private protected uint treeEntityID;
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
            uniqueID = ++currentID;

            RoomType = setRoomType;
            Name = setName;
            defaultDescriptor = setDefaultDescriptor;

            Chair burgundyWoodChair = new Chair("Burgundy Chair", RoomID, "You are looking at a cool chair", "A chair sits here.");
            Tree burgundyWoodTree = new Tree("Burgundy Tree", RoomID, "That's a burgundy-leaved tree.", "A tree is here. It's cool. And burgundy.");

            roomEntities.Add(burgundyWoodChair);
            roomEntities.Add(burgundyWoodTree);

            List<uint> conditionIDs = new();
            List<EntityConditions> conditionsList = new();

            conditionsList.Add(new EntityConditions(burgundyWoodTree.ID, null));
            ConditionalDescriptor descriptorCondition = new ConditionalDescriptor("Caramel and burgundy leaves rustle in the shallow breeze across the arid plains, the wind gently whipping around your ankles.", conditionsList);
            localConditionalDescriptors.Add(descriptorCondition);

            conditionsList.Clear();

            conditionsList.Add(new EntityConditions(burgundyWoodChair.ID, null));
            conditionsList.Add(new EntityConditions(burgundyWoodTree.ID, null));
            descriptorCondition = new ConditionalDescriptor("Gently falling from their branches, the occasional leaf drifts as a feather to rest upon a chair that sits beside the tree, carved from the same oakwood as the trunk it rests against.", conditionsList);
            localConditionalDescriptors.Add(descriptorCondition);

            conditionsList.Clear();

            var treeProperties = new List<EntityProperty>();
            treeProperties.Add(new EntityProperty("isBurnt", true));

            conditionsList.Add(new EntityConditions(burgundyWoodTree.ID, treeProperties));
            descriptorCondition = new ConditionalDescriptor("Burgundy leaves drift in the wind around a burnt husk, the trunk of a charred and forgotten tree holding up a singed chair.Perhaps this was a nice spot to sit and read, once.", conditionsList, 1);
            localConditionalDescriptors.Add(descriptorCondition);
        }
    }
}
