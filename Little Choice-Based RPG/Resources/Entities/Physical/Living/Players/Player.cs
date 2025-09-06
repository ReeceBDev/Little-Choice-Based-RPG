using Little_Choice_Based_RPG.Resources.Components.ComponentAccessors;
using Little_Choice_Based_RPG.Types.PropertySystem.Containers;
using Little_Choice_Based_RPG.Types.PropertySystem.Properties;

namespace Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players
{
    internal class Player : LivingCreature
    {
        private readonly static List<Func<PropertyContainer, IProperty?>> requiredProperties = new()
        {
            { i => i.Player.Descriptor.Custom }
        };

        private readonly static List<Action<PropertyContainer>> defaultProperties = new()
        {
            //Components
            { i => i.Player.UsesSystem = true },
            { i => i.Inventory.UsesSystem = true },
            { i => i.Gear.UsesSystem = true },

            //Capabilities
            { i => i.Player.CanHear = true},
            { i => i.Player.CanSee = true },

            //Stats
            { i => i.Weightbearing.StrengthInKG = 32.0m},
            { i => i.Weight.WeightInKG = 78.4m},

            //Custom descriptor
            { i => i.Player.Descriptor.Custom = "A man falls from a tear in reality, thrown forwards without footing and clipped at by cold, sharp airs and a stomach-pit of sudden peril. Darkly dressed without a sliver of skin, behind his jacket a nictation of the lapel reveals a dark silver-grey gunmetal contraption branded with one of the traditional marks of the Potsun Burran, meaning scavenger. He is you.\n\nOut of the void of glass, ensnared in whispers of iridescent whisps of near translucent smoke, white whispers of reality fluttering away having been dragged out of the mirror-esque and clinging with futility to the edges of your self. You stagger clear of the rupture and catch yourself, your boots hitting the ground with a thud, grains of sand thrown from the edges of your boots."}
        };

        public Player(List<Action<PropertyContainer>>? derivedProperties = null)
            : base(ConcatenateProperties(derivedProperties, defaultProperties))
        {
            //Validate required properties have been set on entityProperties
            ValidateRequiredProperties(requiredProperties);
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