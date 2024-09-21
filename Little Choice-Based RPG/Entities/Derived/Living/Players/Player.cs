using Little_Choice_Based_RPG.Entities.Derived.Equippables.Armour;
using Little_Choice_Based_RPG.Objects.Base;
using Little_Choice_Based_RPG.Objects.Gear.Armour.Helmets;
using Little_Choice_Based_RPG.World.Rooms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Entities.Derived.Living.Players
{
    internal struct PlayerGear
    {
        internal Helmet equippedHelmet;
    }
    internal class Player
    {
        private protected PlayerGear carriedGear;
        private protected uint currentRoomID;
        private protected string playerDescriptor;

        private protected bool wearingHelmet = true;

        private protected bool playerCanHear = false;
        private protected bool playerCanSee = false;
        private protected bool playerCanMove = false;
        private protected bool isPlayerKnockedDown = true;

        public Player(Room spawnInsideRoom)
        {
            currentRoomID = spawnInsideRoom.ID;
            carriedGear.equippedHelmet = new DavodianMkI();
        }

        public void Equip()
        {
            if (carriedGear.equippedHelmet == null)
            {
                playerDescriptor = carriedGear.equippedHelmet.EquipDescriptor;
                wearingHelmet = true;
            }
            else
                playerDescriptor = $"The {GetEquippedHelmet()} that you wear prevents you from doing this.";

        }

        public void Unequip()
        {
            if (carriedGear.equippedHelmet == null)
            {
                playerDescriptor = "You reach your hands to your head and quickly realise you aren't wearing a helmet.";
            }
            else
            {
                playerDescriptor = carriedGear.equippedHelmet.UnequipDescriptor;
                wearingHelmet = false;
            }

        }

        public string GetEquippedHelmet() => carriedGear.equippedHelmet.Name;

        public string PlayerDescriptor => playerDescriptor;
        public bool CanHear => playerCanHear;
        public uint CurrentRoomID => currentRoomID;
    }
}