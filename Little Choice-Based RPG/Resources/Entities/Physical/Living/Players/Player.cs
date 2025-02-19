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
using Little_Choice_Based_RPG.Resources.Choices;
using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Equippables.Armour.Helmets;
using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual.Weightbearing_Derivatives;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual.Descriptive_Derivatives;

namespace Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players
{
    internal struct PlayerGear
    {
        internal Helmet equippedHelmet;
    }
    public class Player : LivingCreature
    {
        private protected PlayerGear carriedGear;

        private protected bool wearingHelmet = true;

        private protected bool playerCanHear = false;
        private protected bool playerCanSee = false;
        private protected bool playerCanMove = false;
        private protected bool isPlayerKnockedDown = true;

        public uint Position;

        public Player(string setName, uint setPosition, decimal setWeightInKG = 0m, decimal setStrengthInKG = 0m) : base(setName, setWeightInKG, setStrengthInKG = 0m)
        {
            Position = setPosition;
            //carriedGear.equippedHelmet = new DavodianMkIHelmet(Position);
        }

        public void Equip()
        {
            if (carriedGear.equippedHelmet == null)
            {
                LastActionDescriptor.Value = carriedGear.equippedHelmet.EquipDescriptor;
                wearingHelmet = true;
            }
            else
                LastActionDescriptor.Value = $"The {GetEquippedHelmet()} that you wear prevents you from doing this.";

        }

        public void Unequip()
        {
            if (carriedGear.equippedHelmet == null)
            {
                LastActionDescriptor.Value = "You reach your hands to your head and quickly realise you aren't wearing a helmet.";
            }
            else
            {
                LastActionDescriptor.Value = carriedGear.equippedHelmet.UnequipDescriptor;
                wearingHelmet = false;
            }

        }

        public virtual List<Choice> GenerateChoices()
        {
            List<Choice> choices = new List<Choice>();

            // choices.AddRange(HandleDirectionChoices());

            return choices;
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

        public SanitizedString GetEquippedHelmet() => carriedGear.equippedHelmet.Name;

        public SanitizedString PlayerDescriptor { get; private set; }
        public SanitizedString LastActionDescriptor { get; private set; }
        public bool CanHear => playerCanHear;
    }
}