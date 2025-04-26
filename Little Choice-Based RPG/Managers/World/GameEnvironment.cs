using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Furniture;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Plants;
using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory.InventoryExtensions;
using Little_Choice_Based_RPG.Resources.Systems.InformationalSystems;
using Little_Choice_Based_RPG.Resources.Systems.InformationalSystems.Descriptor;
using Little_Choice_Based_RPG.Resources.Systems.InformationalSystems.Descriptor.DescriptorExtensions;
using Little_Choice_Based_RPG.Types;
using Little_Choice_Based_RPG.Types.DescriptorConditions;
using Little_Choice_Based_RPG.Types.EntityProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Managers.World
{
    public class GameEnvironment
    {
        private protected static uint currentID = 0;
        public uint UniqueID { get; init; }

        public Dictionary<uint, Room> Rooms = new Dictionary<uint, Room>();

        public GameEnvironment()
        {
            UniqueID = ++currentID;
        }

        public void GenerateAllRooms()
        {
            //string setNorthOfAtriiKaalDefaultDescriptor = "You wake up with a start - you breath in sharply and sputter as heavy dust dries your mouth. \r\nIn front of you is the cracked and charred sandstone ground of the Potsun Burran. It glitters with the debris of a thousand shredded spaceships.\r\nThe high-pitched drone you hear subsides in to a roar as you realise you are laying on the ground, face-first.";
            //Room northOfAtriiKaal = new Room ("NorthOfAtriiKaal", RoomType.Desert, setNorthOfAtriiKaalDefaultDescriptor);
            //Rooms.Add((uint)northOfAtriiKaal.Properties.GetPropertyValue("ID"), northOfAtriiKaal);

            Room burgundyTree1 = GenerateBurgundyTree();
            Rooms.Add((uint)burgundyTree1.Properties.GetPropertyValue("ID"), burgundyTree1);

            Room burgundyTree2 = GenerateBurgundyTree();
            Rooms.Add((uint) burgundyTree2.Properties.GetPropertyValue("ID"), burgundyTree2);
        }

        public Room? GetRoomByID(uint ID)
        {
            foreach (KeyValuePair<uint, Room> roomList in Rooms)
            {
                if (roomList.Key.Equals(ID))
                    return roomList.Value;
            }

            return null;
        }


        private Room GenerateBurgundyTree()
        {
            //Create a chair
            Dictionary<string, object> newBurgundyWoodChair = new Dictionary<string, object>();
            newBurgundyWoodChair.Add("Name", "Burgundy-Wood Chair");
            newBurgundyWoodChair.Add("Type", "Little_Choice_Based_RPG.Resources.Entities.Physical.Furniture.Chair");
            newBurgundyWoodChair.Add("Descriptor.Generic.Default", "A chair sits here.");
            newBurgundyWoodChair.Add("Descriptor.Inspect.Default", "You are looking at a cool chair.");
            newBurgundyWoodChair.Add("Descriptor.InventorySystem.Interaction.Pickup.Title", "try to pick up the chair");
            newBurgundyWoodChair.Add("Descriptor.InventorySystem.Interaction.Pickup.Invoking", "you hoist up the chair. its heavy.");
            newBurgundyWoodChair.Add("Descriptor.InventorySystem.Interaction.Drop.Title", "let go of the heavy chair.");
            newBurgundyWoodChair.Add("Descriptor.InventorySystem.Interaction.Drop.Invoking", "you drop the chair with a thud. what a relief.");
            GameObject burgundyWoodChair = (GameObject)PropertyContainerFactory.NewGameObject(newBurgundyWoodChair);
            GameObject burgundyWoodChair2 = (GameObject)PropertyContainerFactory.NewGameObject(newBurgundyWoodChair);


            //Create a tree
            Dictionary<string, object> newBurgundyTree = new Dictionary<string, object>();
            newBurgundyTree.Add("Name", "Burgundy Tree");
            newBurgundyTree.Add("Descriptor.Generic.Default", "A tree is here. It's cool. And burgundy.");
            newBurgundyTree.Add("Descriptor.Inspect.Default", "That's a burgundy-leaved tree.");
            Tree burgundyTree = new Tree(newBurgundyTree);

            //Create a lightbulb
            Dictionary<string, object> zalolintLightbulb48w230vHP = new Dictionary<string, object>();
            zalolintLightbulb48w230vHP.Add("Name", "ZaloLint 48w 230v Lightstrip");
            zalolintLightbulb48w230vHP.Add("Type", "Little_Choice_Based_RPG.Resources.Entities.Physical.Furniture.Lightbulbs.TwentyFourInchLightstrip");
            zalolintLightbulb48w230vHP.Add("Descriptor.Generic.Default", "Faint buzzing stings off the walls as a zalolint lightstrip flickers a harshly cold glow, the chemical smell of residual burnt zalolintene wisps for an instant.");
            zalolintLightbulb48w230vHP.Add("Descriptor.Inspect.Default", "Aluminium scratched lightly at either end and boldy emitting its artificial gaze, the zalolint drones a thousand fizzes, emanating a contiguous noise. \nIts white light unrelenting and brazen in its boldness, the artificiality capable of maybe keeping awake even an insomniac.\nRolling it over in its socket with a squeak, your finger brushes over a bold font decree in small, white capitals, which reads \"48W 230V - HP 24INCH ZALOLINT HIGHSPEC PHOTON EMITTER PROPERTY - CONTAINS ZALOLINTENE - PROPERTY OF FITZLO CO.\"");

            zalolintLightbulb48w230vHP.Add("Component.RepairSystem", true);
            zalolintLightbulb48w230vHP.Add("Repairable.ByChoice", true);
            zalolintLightbulb48w230vHP.Add("Descriptor.Repair.Interaction.Title", "Repair - Scrap out the broken remains with your functioning 48w 230v lightstrip.");
            zalolintLightbulb48w230vHP.Add("Descriptor.Repair.Interaction.Invoking", "With a scraping squeak you negotiate twisting out each side of the sharp shattered glass from its linear socket. \nPlacing your functional lightstrip into the quickrelease, a switch faintly clicks and you forcefully scrape in the opposite side. \nUpon both sides touching their contacts, the lightstrip naps and crackles to life immediately, a gentle 48 watts of power rippling dramatically through the filament, igniting the recombustible zalolintene.");
            zalolintLightbulb48w230vHP.Add("Damage.Broken", true);

            zalolintLightbulb48w230vHP.Add("Component.BreakSystem", true);
            zalolintLightbulb48w230vHP.Add("Breakable.ByChoice", true);
            zalolintLightbulb48w230vHP.Add("Descriptor.Breakable.Interaction.Title", "Break - Shatter along the side of the zalolint lightstrip until it leaks or the filament bends.");
            zalolintLightbulb48w230vHP.Add("Descriptor.Breakable.Interaction.Invoking", "Relentlessly smashing the centre of the zalolint's tubular trunk, a loud crack splits glass into energetic shards which skate the air with viciousness, whispering off the floor.");
            zalolintLightbulb48w230vHP.Add("Descriptor.Generic.Broken", "Above hangs pieces of a shattered zalolint lightstrip. Underfoot crunches glass shards, the sharp citric of leaked zalolintene permeates your smell. Its souless husk clings by its decrepit filament to its socket, its home turned graveyard.");
            zalolintLightbulb48w230vHP.Add("Descriptor.Inspect.Broken", "Charred and crisp, the gashed and razor edges of aluminium that once housed the zalolint lightstrip are now warped and twisted, bearing their metallic teeth to the world.\n Rubbing your thumb over one side, rubbing away some soot, you find grey capitals which announce meekly, \"48W 230V - HP 24INCH ZA...INT HIG...\" \nThe rest of the text is obscured by a long blackened scorch-mark that runs from its tip electric blue into a strand of bold copper, the burn widening rapidly into charcoal grey up until the very end of the casing where the burn looks to have originated.");

            zalolintLightbulb48w230vHP.Add("Descriptor.InventorySystem.Interaction.Pickup.Title", "pick - the lightbulb, test");
            zalolintLightbulb48w230vHP.Add("Descriptor.InventorySystem.Interaction.Pickup.Invoking", "you are picking up the lightbulb test");
            zalolintLightbulb48w230vHP.Add("Descriptor.InventorySystem.Interaction.Drop.Title", "drop - the lightbulb, test");
            zalolintLightbulb48w230vHP.Add("Descriptor.InventorySystem.Interaction.Drop.Invoking", "you are dropping the lightbulb test");
            GameObject testLightBulb = (GameObject)PropertyContainerFactory.NewGameObject(zalolintLightbulb48w230vHP);
            GameObject testLightBulb2 = (GameObject)PropertyContainerFactory.NewGameObject(zalolintLightbulb48w230vHP);


            //Create the first conditional descriptor
            CreateDescriptorStateList firstCondition = new("Descriptor.Generic.Current", 
                "Caramel and burgundy leaves rustle in the shallow breeze across the arid plains, the wind gently whipping around your ankles.");

            firstCondition.AddEntityCondition(null, null, (uint)burgundyTree.Properties.GetPropertyValue("ID"));


            //Create the second conditional descriptor
            CreateDescriptorStateList secondCondition = new("Descriptor.Generic.Current", 
                "Gently falling from their branches, the occasional leaf drifts as a feather to rest upon a chair that sits beside the tree, carved from the same oakwood as the trunk it rests against.");

            secondCondition.AddEntityCondition(null, null, (uint)burgundyWoodChair.Properties.GetPropertyValue("ID"));
            secondCondition.AddEntityCondition(null, null, (uint)burgundyTree.Properties.GetPropertyValue("ID"));


            //Create the third conditional descriptor 
            CreateDescriptorStateList thirdCondition = new("Descriptor.Generic.Current", 
                "Burgundy leaves drift in the wind around a burnt husk, the trunk of a charred and forgotten tree holding up a singed chair. " +
                "Perhaps this was a nice spot to sit and read, once.", 
                1);

            thirdCondition.AddEntityCondition("Flammability.IsBurnt", true, (uint)burgundyTree.Properties.GetPropertyValue("ID"));


            //Create the Room itself
            Dictionary<string, object> newBurgundyTreeRoom = new Dictionary<string, object>();

            newBurgundyTreeRoom.Add("Name", "Underneath a Burgundy Tree");
            newBurgundyTreeRoom.Add("Type", "Little_Choice_Based_RPG.Resources.Rooms.Room");
            newBurgundyTreeRoom.Add("Descriptor.Generic.Default", "Arid desert sand whips by your skin. A few burgundy leaves drift across the floor.");
            newBurgundyTreeRoom.Add("Descriptor.Inspect.Default", "Theres a tree here.");


            //Establish new variables
            Room newRoom = (Room) PropertyContainerFactory.NewGameObject(newBurgundyTreeRoom);
            ItemContainer roomEntities = (ItemContainer) newRoom.Extensions.Get("ItemContainer");


            //Add the new entites to the Room.
            roomEntities.Add(testLightBulb);
            roomEntities.Add(burgundyWoodChair);
            roomEntities.Add(burgundyWoodChair2);
            roomEntities.Add(testLightBulb2);
            roomEntities.Add(burgundyTree);

            //Add the new conditional descriptors to the Room.
            DescriptorProcessor.CreateDescriptor(newRoom, firstCondition);
            DescriptorProcessor.CreateDescriptor(newRoom, secondCondition);
            DescriptorProcessor.CreateDescriptor(newRoom, thirdCondition);


            return newRoom;
        }
    }
}