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