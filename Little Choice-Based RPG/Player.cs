using Little_Choice_Based_RPG.Objects.Gear.Armour;
using Little_Choice_Based_RPG.Objects.Gear.Armour.Helmets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG
{
    internal struct PlayerGear
    {
        internal Helmet equippedHelmet;
    }
    internal class Player
    {
        private protected PlayerGear carriedGear;
        private protected Player
        private protected string playerDescriptor;

        private protected bool wearingHelmet = true;

        private protected bool PlayerCanHear = false;
        private protected bool PlayerCanSee = false;
        private protected bool PlayerCanMove = false;
        private protected bool IsPlayerKnockedDown = true;

        public Player()
        {
            this.carriedGear.equippedHelmet = new DavodianMkI();
        }

        public void Equip()
        {
            if (this.carriedGear.equippedHelmet == null)
            {
                playerDescriptor = this.carriedGear.equippedHelmet.EquipDescriptor;
                wearingHelmet = true;
            }
            else
                playerDescriptor = $"The {GetEquippedHelmet()} that you wear prevents you from doing this.";

        }

        public void Unequip()
        {
            if (this.carriedGear.equippedHelmet == null)
            {
                playerDescriptor = "You reach your hands to your head and quickly realise you aren't wearing a helmet.";
            }
            else
            {
                playerDescriptor = this.carriedGear.equippedHelmet.UnequipDescriptor;
                wearingHelmet = false;
            }

        }

        public string GetEquippedHelmet() => this.carriedGear.equippedHelmet.Name;

        public string PlayerDescriptor => this.playerDescriptor;
    }
}