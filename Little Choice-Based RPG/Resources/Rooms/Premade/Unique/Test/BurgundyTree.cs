using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Furniture;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Plants;
using Little_Choice_Based_RPG.Resources.Systems.Damage.Break;
using Little_Choice_Based_RPG.Resources.Systems.Damage.Flammability;
using Little_Choice_Based_RPG.Resources.Systems.Damage.Repair;
using Little_Choice_Based_RPG.Resources.Systems.Gear;
using Little_Choice_Based_RPG.Types.EntityProperties;
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
            //Set new room info
            uniqueID = ++currentID;

            RoomType = setRoomType;
            Name = setName;
            defaultDescriptor = setDefaultDescriptor;


            //Create new chair
            Dictionary<string, object> newBurgundyWoodChair = new Dictionary<string, object>();
            newBurgundyWoodChair.Add("Name", "Burgundy-Wood Chair");
            newBurgundyWoodChair.Add("Descriptor.Generic.Default", "A chair sits here.");
            newBurgundyWoodChair.Add("Descriptor.Inspect.Default", "You are looking at a cool chair.");
            Chair burgundyWoodChair = new Chair(newBurgundyWoodChair);

            //Create new tree
            Dictionary<string, object> newBurgundyTree = new Dictionary<string, object>();
            newBurgundyTree.Add("Name", "Burgundy Tree");
            newBurgundyTree.Add("Descriptor.Generic.Default", "A tree is here. It's cool. And burgundy.");
            newBurgundyTree.Add("Descriptor.Inspect.Default", "That's a burgundy-leaved tree.");
            Tree burgundyTree = new Tree(newBurgundyTree);

            //Add the new entities to the Room.
            roomEntities.Add(burgundyWoodChair);
            roomEntities.Add(burgundyTree);


            //Create the conditions for each room-state
            List<uint> conditionIDs = new();

            //Condition 1
            List<EntityState> conditionsList = new();

            conditionsList.Add(new EntityState((uint) burgundyTree.Properties.GetPropertyValue("ID"), null));
            ConditionalDescriptor descriptorCondition = new ConditionalDescriptor("Caramel and burgundy leaves rustle in the shallow breeze across the arid plains, the wind gently whipping around your ankles.", conditionsList);
            localConditionalDescriptors.Add(descriptorCondition);

            //Condition 2
            List<EntityState> conditionsList2 = new();

            conditionsList2.Add(new EntityState((uint)burgundyWoodChair.Properties.GetPropertyValue("ID"), null));
            conditionsList2.Add(new EntityState((uint)burgundyTree.Properties.GetPropertyValue("ID"), null));
            descriptorCondition = new ConditionalDescriptor("Gently falling from their branches, the occasional leaf drifts as a feather to rest upon a chair that sits beside the tree, carved from the same oakwood as the trunk it rests against.", conditionsList2);
            localConditionalDescriptors.Add(descriptorCondition);

            //Condition 3
            List<EntityState> conditionsList3 = new();

            var requiredTreeProperties = new List<EntityProperty>();
            requiredTreeProperties.Add(new EntityProperty("Flammability.IsBurnt", true));

            conditionsList3.Add(new EntityState((uint)burgundyTree.Properties.GetPropertyValue("ID"), requiredTreeProperties));
            descriptorCondition = new ConditionalDescriptor("Burgundy leaves drift in the wind around a burnt husk, the trunk of a charred and forgotten tree holding up a singed chair. Perhaps this was a nice spot to sit and read, once.", conditionsList3, 1);
            localConditionalDescriptors.Add(descriptorCondition);



        }
    }
}
