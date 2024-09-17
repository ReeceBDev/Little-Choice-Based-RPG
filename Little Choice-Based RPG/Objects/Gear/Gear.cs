﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Objects.Gear
{
    internal struct GearDescription()
    {
        public string equip = "You equip this and feel much better prepared for the Potsun Burran and its challenges.";
        public string unequip = "Unequipping it with due care, you free yourself up for something else in its place.";
    }
    internal class Gear : GenericObject
    {
        private protected GearDescription gearDescriptor;
        private protected const bool isEquippable = true;
        private protected bool isWornByPlayer = false;

        public bool IsEquippable { get; }
        public bool IsWornByPlayer { get; set; }
        public string EquipDescriptor => gearDescriptor.equip;
        public string UnequipDescriptor => gearDescriptor.unequip;
    }
}
