using Little_Choice_Based_RPG.Managers.World;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Furniture;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Plants;
using Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Break;
using Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Flammability;
using Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Repair;

using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.RoomTypes;
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


            //Make a lightbulb
            Dictionary<string, object> zalolintLightbulb48w230vHP = new Dictionary<string, object>();
            zalolintLightbulb48w230vHP.Add("Name", "ZaloLint 48w 230v Lightstrip");
            zalolintLightbulb48w230vHP.Add("Type", "Little_Choice_Based_RPG.Resources.Entities.Physical.Furniture.Lightbulb");
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
            GameObject testLightBulb = PropertyContainerFactory.NewGameObject(zalolintLightbulb48w230vHP);
            roomEntities.Add(testLightBulb);
        }
    }
}
