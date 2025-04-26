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
using Little_Choice_Based_RPG.Types.EntityProperties;

namespace Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players
{
    public class Player : LivingCreature
    {
        private readonly static Dictionary<string, PropertyType> requiredProperties = new Dictionary<string, PropertyType>()
        {
            { "Descriptor.Player.Custom", PropertyType.String }
        };

        private readonly static Dictionary<string, PropertyType> optionalProperties = new Dictionary<string, PropertyType>()
        {

        };

        private readonly static Dictionary<string, object> defaultProperties = new Dictionary<string, object>()
        {
            //Components
            {"Component.PlayerSystem", true },
            {"Component.InventorySystem", true },
            {"Component.GearSystem", true },

            //Capabilities
            {"Player.CanHear", true},
            {"Player.CanSee", true },

            //Stats
            {"Weightbearing.StrengthInKG",  32.0m},
            {"WeightInKG", 78.4m},

            //Custom descriptor
            {"Descriptor.Player.Custom", "A man falls from a tear in reality, thrown forwards without footing and clipped at by cold, sharp airs and a stomach-pit of sudden peril. Darkly dressed without a sliver of skin, behind his jacket a nictation of the lapel reveals a dark silver-grey gunmetal contraption branded with one of the traditional marks of the Potsun Burran, meaning scavenger. He is you.\n\nOut of the void of glass, ensnared in whispers of iridescent whisps of near translucent smoke, white whispers of reality fluttering away having been dragged out of the mirror-esque and clinging with futility to the edges of your self. You stagger clear of the rupture and catch yourself, your boots hitting the ground with a thud, grains of sand thrown from the edges of your boots."}
        };

        static Player()
        {
            //Define new required and optional ValidProperties for this class
            DeclareNewPropertyTypes(requiredProperties);
            DeclareNewPropertyTypes(optionalProperties);
        }

        public Player(Dictionary<string, object>? derivedProperties = null)
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