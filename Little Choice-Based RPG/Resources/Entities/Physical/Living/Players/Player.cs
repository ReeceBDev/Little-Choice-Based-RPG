using Little_Choice_Based_RPG.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Equippables.Armour;
using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual.Weightbearing_Derivatives;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual.Descriptive_Derivatives;
using Little_Choice_Based_RPG.Types.EntityProperties;

namespace Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players
{
    public class Player : LivingCreature
    {
        private readonly static Dictionary<string, PropertyType> requiredProperties = new Dictionary<string, PropertyType>()
        {
            {"Player.CanHear", PropertyType.Boolean },
            {"Player.CanSee", PropertyType.Boolean }
        };

        private readonly static Dictionary<string, PropertyType> optionalProperties = new Dictionary<string, PropertyType>()
        {

        };

        private readonly static Dictionary<string, object> defaultProperties = new Dictionary<string, object>()
        {
            {"HasGearSlots", true },
            {"Player.CanHear", true},
            {"Player.CanSee", true },
            {"StrengthInKG",  32.0m}
        };

        static Player()
        {
            //Define new required and optional ValidProperties for this class
            DeclareNewPropertyTypes(requiredProperties);
            DeclareNewPropertyTypes(optionalProperties);
        }

        public Player(uint setSpawnRoomID, Dictionary<string, object>? derivedProperties = null)
            : base(SetLocalProperties(derivedProperties ??= new Dictionary<string, object>()))
        {
            //Set player spawn position
            entityProperties.UpsertProperty("Position", setSpawnRoomID);

            //Give the player a helmet
            Dictionary<string, object> davodianMk1Helmet = new Dictionary<string, object>();
            davodianMk1Helmet.Add("Name", "Davodian MkI Covered Faceplate");
            davodianMk1Helmet.Add("Descriptor.Generic.Default", "Discarded aside here is your original Davodian MkI, its dented gunmetal grey faceplate long lustreless.");
            davodianMk1Helmet.Add("Descriptor.Inspect.Default", "Disfigured and mottled from years of abuse, the gunmetal grey protective sensors relay a live feed, protecting your vision and perceptible hearing.\r\nThe units bodywork has been long since reworked in patchwork copper shielding - under the seams near-charred components made up of scrapped debris poke through residual sand.");
            davodianMk1Helmet.Add("Descriptor.Equip", "Dusty residue coats your hands as you heave the thick, heavy metal above you and press your forehead in.\r\nInterlocking clicks engage behind your neck, a gentle buzz as the metal comes online.");
            davodianMk1Helmet.Add("Descriptor.Unequip", "Engaging the clasp at the rear, the locks reluctantly scrape their disengaging clicks and the full weight of the visor bears down on your head.\r\nSandpaper lining scratches the sides of your face when you tilt your head forwards and force the faceplate off.");

            davodianMk1Helmet.Add("Repair.IsRepairable", true);
            davodianMk1Helmet.Add("Repair.IsRepairableByChoice", true);
            davodianMk1Helmet.Add("Descriptor.Repair.Choice.Interact", "Repair - Re - calibrate the helmets longitudinal wave sensor - array.");
            davodianMk1Helmet.Add("Descriptor.Repair.Choice.Repairing", "You fixed the helmet, yayy!");

            davodianMk1Helmet.Add("IsBreakable", true);
            davodianMk1Helmet.Add("IsBreakableByChoice", true);
            davodianMk1Helmet.Add("Descriptor.Choice.Break.Interact", "Damage - Intentionally misalign the helmets longitudinal wave sensor-array");
            davodianMk1Helmet.Add("Descriptor.Descriptor.Choice.Breaking", "You broke the helmet oh nooo!");
            Helmet testDavodian = new Helmet(davodianMk1Helmet);

            this.entityProperties.UpsertProperty("Gear.Slot.Helmet.ID", testDavodian.entityProperties.GetPropertyValue("ID"));
            testDavodian.Equip();

            //Validate required properties have been set on entityProperties
            ValidateRequiredProperties(requiredProperties);
        }

        private static Dictionary<string, object> SetLocalProperties(Dictionary<string, object> derivedProperties)
        {
            //Apply default properties for this class to the current list of derivedProperties
            ApplyDefaultProperties(derivedProperties, defaultProperties);

            return derivedProperties; //Return is required to give (base) the derived list.
        }

        /*
        public List<Choice> HandleDirectionChoices()
        {
            List<Choice> choices = new List<Choice>();

            GameEnvironment currentGameEnvironment = GameDomain.FindEnvironmentByID(CurrentGameEnvironmentID);
            Room currentRoom = currentGameEnvironment.FindRoomByID(CurrentRoomID);

            foreach (RoomDirection availableDirection in currentRoom.Directions)
            {
                Direction orientationName = availableDirection.ChosenDirection;
                Room destinationRoom = currentGameEnvironment.FindRoomByID(availableDirection.DestinationRoomID);
                string destinationName = destinationRoom.Name;

                string flavourText = $"Move {orientationName} towards {destinationName}";

                choices.Add(new Choice(flavourText, MoveToRoomFromChoice));
                choices.Add(new Choice(flavourText, MoveToRoomFromChoice(availableDirection)));

            }

            return choices;
        }
        */

        /*
        public string MoveToRoomFromChoice(RoomDirection direction)
        {
            string movementDescriptor = "You walked towards your destination.";
            MoveToRoom(direction);
            return movementDescriptor;
        }
        */
    }
}