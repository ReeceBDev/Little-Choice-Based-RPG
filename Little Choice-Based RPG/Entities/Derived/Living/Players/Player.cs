using Little_Choice_Based_RPG.Choices;
using Little_Choice_Based_RPG.Entities.Derived.Equippables.Armour;
using Little_Choice_Based_RPG.Frontend;
using Little_Choice_Based_RPG.Entities;
using Little_Choice_Based_RPG.Entities.Derived.Equippables.Armour.Helmets;
using Little_Choice_Based_RPG.Types;
using Little_Choice_Based_RPG.World.Rooms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Little_Choice_Based_RPG.Entities.Derived.Living.Players
{
    internal struct PlayerGear
    {
        internal Helmet equippedHelmet;
    }
    public class Player : LivingCreature
    {
        private protected PlayerGear carriedGear;
        private protected SanitizedString? playerDescriptor;

        private protected bool wearingHelmet = true;

        private protected bool playerCanHear = false;
        private protected bool playerCanSee = false;
        private protected bool playerCanMove = false;
        private protected bool isPlayerKnockedDown = true;

        private protected int xPosition;
        private protected int yPosition;

        public uint CurrentRoomID { get; set; } = 0U;
        public Player(Vector2 setPosition) : base(setPosition)
        {
            carriedGear.equippedHelmet = new Little_Choice_Based_RPG.Entities.Derived.Equippables.Armour.Helmets.DavodianMkIHelmet(this.Position);
        }

        public void Equip()
        {
            if (carriedGear.equippedHelmet == null)
            {
                playerDescriptor.Value = carriedGear.equippedHelmet.EquipDescriptor;
                wearingHelmet = true;
            }
            else
                playerDescriptor.Value = $"The {GetEquippedHelmet()} that you wear prevents you from doing this.";

        }

        public void Unequip()
        {
            if (carriedGear.equippedHelmet == null)
            {
                playerDescriptor.Value = "You reach your hands to your head and quickly realise you aren't wearing a helmet.";
            }
            else
            {
                playerDescriptor.Value = carriedGear.equippedHelmet.UnequipDescriptor;
                wearingHelmet = false;
            }

        }

        public SanitizedString GetEquippedHelmet() => carriedGear.equippedHelmet.Name;

        public SanitizedString PlayerDescriptor => playerDescriptor;
        public bool CanHear => playerCanHear;
        public UserInterface CurrentInterface { get; init; } = new UserInterface();
    }
}