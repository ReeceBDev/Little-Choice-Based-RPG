using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Plants;
using Little_Choice_Based_RPG.Resources.Entities.Rooms;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory.InventoryExtensions;
using Little_Choice_Based_RPG.Resources.Systems.InformationalSystems;
using Little_Choice_Based_RPG.Resources.Systems.InformationalSystems.Descriptor;
using Little_Choice_Based_RPG.Types.Navigation;
using Little_Choice_Based_RPG.Types.PropertySystem.Containers;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using Little_Choice_Based_RPG.Types.PropertySystem.Properties;
using Little_Choice_Based_RPG.Resources.Components.ComponentAccessors;

namespace Little_Choice_Based_RPG.Managers.World
{
    internal class GameEnvironment
    {
        private readonly Coordinates minimumMapPoint = new Coordinates(-5, 0, -5);
        private readonly Coordinates maximumMapPoint = new Coordinates(5, 0, 5);

        private protected static uint currentID = 0;
        public uint UniqueID { get; init; }

        /// <summary> A registry of all rooms currently present and accessible in the game world. </summary>
        public RoomPositions RoomRegistry = new();

        public GameEnvironment()
        {
            UniqueID = ++currentID;

            RoomRegistry = GenerateEnvironment();

            //Subscribe to the PropertyContainer Factory, for recieving room additions.
            //PropertyContainerFactory.NewPropertyContainer += OnNewPropertyContainer;
        }

        public RoomPositions GenerateEnvironment()
        {
            RoomPositions predefinedRooms = new();
            RoomPositions completeEnvironment = new();

            predefinedRooms = GeneratePredefinedRooms();
            completeEnvironment = EnvironmentMapper.AutoGenerate(predefinedRooms, minimumMapPoint, maximumMapPoint);

            return completeEnvironment;
        }

        public RoomPositions GeneratePredefinedRooms()
        {
            RoomPositions predefinedRooms = new();
            //string setNorthOfAtriiKaalDefaultDescriptor = "You wake up with a start - you breath in sharply and sputter as heavy dust dries your mouth. \r\nIn front of you is the cracked and charred sandstone ground of the Potsun Burran. It glitters with the debris of a thousand shredded spaceships.\r\nThe high-pitched drone you hear subsides in to a roar as you realise you are laying on the ground, face-first.";
            //Room northOfAtriiKaal = new Room ("NorthOfAtriiKaal", RoomType.Desert, setNorthOfAtriiKaalDefaultDescriptor);
            //Rooms.Add((uint)northOfAtriiKaal.Properties.GetPropertyValue("ID"), northOfAtriiKaal);

            Room burgundyTree1 = GenerateBurgundyTree();
            predefinedRooms.Add(0, 0, 0, burgundyTree1);

            Room burgundyTree2 = GenerateBurgundyTree();
            predefinedRooms.Add(1, 0, 0, burgundyTree2);

            return predefinedRooms;
        }

        /// <summary> What is this??? This is DIRTY code. Yuck! But how else should I put a new room in the register? 
        /// I should probably make new rooms subscribe to an EnvironmentSystem of some kind, registered via properties as usual, which does the below properly. </summary>
        /*
        protected virtual void OnNewPropertyContainer(object? sender, IPropertyContainer newObject)
        {
            if (newObject is not Room newRoom)
                return;

            //If the room already knows where it wants to be exactly:
            if (newRoom.Properties.HasExistingPropertyName("Position"))
            {
                Coordinates coordinates = (Coordinates)newRoom.Properties.GetPropertyValue("Position");
                RoomRegistry.Add(coordinates.X, coordinates.Y, coordinates.Z, newRoom);
            }
            else
                return; //Otherwise, for now, do nothing
        }
        */

        private Room GenerateBurgundyTree()
        {
            //Create a chair
            Type temporaryExplicitTypeUntilJSON = typeof(Little_Choice_Based_RPG.Resources.Entities.Physical.Furniture.Chair);

            List<Action<PropertyContainer>> newBurgundyWoodChair = new();
            newBurgundyWoodChair.Add(i => i.Identity.Name = "Burgundy-Wood Chair");
            newBurgundyWoodChair.Add(i => i.Identity.EntityType = temporaryExplicitTypeUntilJSON);
            newBurgundyWoodChair.Add(i => i.Descriptor.Generic.Default = "A chair sits here.");
            newBurgundyWoodChair.Add(i => i.Descriptor.Inspect.Default = "You are looking at a cool chair.");
            newBurgundyWoodChair.Add(i => i.Inventory.Interactions.PickupTitle = "try to pick up the chair");
            newBurgundyWoodChair.Add(i => i.Inventory.Interactions.PickupInvoking = "you hoist up the chair. its heavy.");
            newBurgundyWoodChair.Add(i => i.Inventory.Interactions.DropTitle = "let go of the heavy chair.");
            newBurgundyWoodChair.Add(i => i.Inventory.Interactions.DropInvoking = "you drop the chair with a thud. what a relief.");
            GameObject burgundyWoodChair = (GameObject)PropertyContainerFactory.New(newBurgundyWoodChair, temporaryExplicitTypeUntilJSON);
            GameObject burgundyWoodChair2 = (GameObject)PropertyContainerFactory.New(newBurgundyWoodChair, temporaryExplicitTypeUntilJSON);


            //Create a tree
            List<Action<PropertyContainer>> newBurgundyTree = new();
            newBurgundyTree.Add(i => i.Identity.Name = "Burgundy Tree");
            newBurgundyTree.Add(i => i.Descriptor.Generic.Default = "A tree is here. It's cool. And burgundy.");
            newBurgundyTree.Add(i => i.Descriptor.Inspect.Default = "That's a burgundy-leaved tree.");
            Tree burgundyTree = new Tree(newBurgundyTree);

            //Create a lightbulb
            temporaryExplicitTypeUntilJSON = typeof(Little_Choice_Based_RPG.Resources.Entities.Physical.Furniture.Lightbulbs.TwentyFourInchLightstrip);

            List<Action<PropertyContainer>> zalolintLightbulb48w230vHP = new();
            zalolintLightbulb48w230vHP.Add(i => i.Identity.Name = "ZaloLint 48w 230v Lightstrip");
            zalolintLightbulb48w230vHP.Add(i => i.Identity.EntityType = temporaryExplicitTypeUntilJSON);
            zalolintLightbulb48w230vHP.Add(i => i.Descriptor.Generic.Default = "Faint buzzing stings off the walls as a zalolint lightstrip flickers a harshly cold glow, the chemical smell of residual burnt zalolintene wisps for an instant.");
            zalolintLightbulb48w230vHP.Add(i => i.Descriptor.Inspect.Default = "Aluminium scratched lightly at either end and boldy emitting its artificial gaze, the zalolint drones a thousand fizzes, emanating a contiguous noise. \nIts white light unrelenting and brazen in its boldness, the artificiality capable of maybe keeping awake even an insomniac.\nRolling it over in its socket with a squeak, your finger brushes over a bold font decree in small, white capitals, which reads \"48W 230V - HP 24INCH ZALOLINT HIGHSPEC PHOTON EMITTER PROPERTY - CONTAINS ZALOLINTENE - PROPERTY OF FITZLO CO.\"");

            zalolintLightbulb48w230vHP.Add(i => i.Repair.UsesSystem = true);
            zalolintLightbulb48w230vHP.Add(i => i.Repair.RepairableByChoice = true);
            zalolintLightbulb48w230vHP.Add(i => i.Repair.Interactions.Repairable.Title = "Repair - Scrap out the broken remains with your functioning 48w 230v lightstrip.");
            zalolintLightbulb48w230vHP.Add(i => i.Repair.Interactions.Repairable.Invoking = "With a scraping squeak you negotiate twisting out each side of the sharp shattered glass from its linear socket. \nPlacing your functional lightstrip into the quickrelease, a switch faintly clicks and you forcefully scrape in the opposite side. \nUpon both sides touching their contacts, the lightstrip naps and crackles to life immediately, a gentle 48 watts of power rippling dramatically through the filament, igniting the recombustible zalolintene.");
            zalolintLightbulb48w230vHP.Add(i => i.Damagable.IsBroken = true);

            zalolintLightbulb48w230vHP.Add(i => i.Break.UsesSystem = true);
            zalolintLightbulb48w230vHP.Add(i => i.Break.BreakableByChoice = true);
            zalolintLightbulb48w230vHP.Add(i => i.Break.Interactions.Breakable.Title = "Break - Shatter along the side of the zalolint lightstrip until it leaks or the filament bends.");
            zalolintLightbulb48w230vHP.Add(i => i.Break.Interactions.Breakable.Invoking = "Relentlessly smashing the centre of the zalolint's tubular trunk, a loud crack splits glass into energetic shards which skate the air with viciousness, whispering off the floor.");
            zalolintLightbulb48w230vHP.Add(i => i.Break.Descriptors.Broken.Generic = "Above hangs pieces of a shattered zalolint lightstrip. Underfoot crunches glass shards, the sharp citric of leaked zalolintene permeates your smell. Its souless husk clings by its decrepit filament to its socket, its home turned graveyard.");
            zalolintLightbulb48w230vHP.Add(i => i.Break.Descriptors.Broken.Inspect = "Charred and crisp, the gashed and razor edges of aluminium that once housed the zalolint lightstrip are now warped and twisted, bearing their metallic teeth to the world.\n Rubbing your thumb over one side, rubbing away some soot, you find grey capitals which announce meekly, \"48W 230V - HP 24INCH ZA...INT HIG...\" \nThe rest of the text is obscured by a long blackened scorch-mark that runs from its tip electric blue into a strand of bold copper, the burn widening rapidly into charcoal grey up until the very end of the casing where the burn looks to have originated.");

            zalolintLightbulb48w230vHP.Add(i => i.Inventory.Interactions.PickupTitle = "pick - the lightbulb, test");
            zalolintLightbulb48w230vHP.Add(i => i.Inventory.Interactions.PickupInvoking = "you are picking up the lightbulb test");
            zalolintLightbulb48w230vHP.Add(i => i.Inventory.Interactions.DropTitle = "drop - the lightbulb, test");
            zalolintLightbulb48w230vHP.Add(i => i.Inventory.Interactions.DropInvoking = "you are dropping the lightbulb test");
            GameObject testLightBulb = (GameObject)PropertyContainerFactory.New(zalolintLightbulb48w230vHP, temporaryExplicitTypeUntilJSON);
            GameObject testLightBulb2 = (GameObject)PropertyContainerFactory.New(zalolintLightbulb48w230vHP, temporaryExplicitTypeUntilJSON);


            //Create the first conditional descriptor
            CreateDescriptorStateList firstCondition = new("Descriptor.Generic.Current", 
                "Caramel and burgundy leaves rustle in the shallow breeze across the arid plains, the wind gently whipping around your ankles.");

            firstCondition.AddEntityCondition(null, null, burgundyTree.Identity.ID);


            //Create the second conditional descriptor
            CreateDescriptorStateList secondCondition = new("Descriptor.Generic.Current", 
                "Gently falling from their branches, the occasional leaf drifts as a feather to rest upon a chair that sits beside the tree, carved from the same oakwood as the trunk it rests against.");

            secondCondition.AddEntityCondition(null, null, burgundyWoodChair.Identity.ID);
            secondCondition.AddEntityCondition(null, null, burgundyTree.Identity.ID);


            //Create the third conditional descriptor 
            CreateDescriptorStateList thirdCondition = new("Descriptor.Generic.Current", 
                "Burgundy leaves drift in the wind around a burnt husk, the trunk of a charred and forgotten tree holding up a singed chair. " +
                "Perhaps this was a nice spot to sit and read, once.", 
                1);

            thirdCondition.AddEntityCondition("Flammability.IsBurnt", true, burgundyTree.Identity.ID);


            //Create the Room itself
            temporaryExplicitTypeUntilJSON = typeof(Room);

            List<Action<PropertyContainer>> newBurgundyTreeRoom = new();

            newBurgundyTreeRoom.Add(i => i.Identity.Name = "Underneath a Burgundy Tree");
            newBurgundyTreeRoom.Add(i => i.Identity.EntityType = temporaryExplicitTypeUntilJSON);
            newBurgundyTreeRoom.Add(i => i.Descriptor.Generic.Default = "Arid desert sand whips by your skin. A few burgundy leaves drift across the floor.");
            newBurgundyTreeRoom.Add(i => i.Descriptor.Inspect.Default = "Theres a tree here.");
            newBurgundyTreeRoom.Add(i => i.Directions.Travel.Description = "Feet scraping flecks of shrubbery drowned in dust scrambling from cracked sandstone, you forge your way further across the desolation.");
            newBurgundyTreeRoom.Add(i => i.Directions.Travel.Title = "Travel - ");


            //Establish new variables
            Room newRoom = (Room) PropertyContainerFactory.New(newBurgundyTreeRoom, temporaryExplicitTypeUntilJSON);
            List<GameObject> roomEntities = ((ItemContainer) newRoom.Extensions.Get("ItemContainer")).Inventory;


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