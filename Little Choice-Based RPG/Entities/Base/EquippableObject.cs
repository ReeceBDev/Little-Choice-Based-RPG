using Little_Choice_Based_RPG.Choices;
using Little_Choice_Based_RPG.Entities.Derived.Living.Players;
using Little_Choice_Based_RPG.Types;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Objects.Base
{
    internal struct EquippableDescription()
    {
        public SanitizedString equip;
        public SanitizedString unequip;
    }
    internal class EquippableObject : InteractableObject
    {
        private protected EquippableDescription equippableDescriptor;
        private protected bool currentlyInUse = false;

        public EquippableObject(string setName, string newGenericDescriptor, Vector2 setPosition, string newInspectDescriptor = "",
            string newNativeDescriptor = "", decimal setWeightInKG = 0m, string setEquipDescriptor = "", string setUnequipDescriptor = "")
            : base(setName, newGenericDescriptor, setPosition, newInspectDescriptor, newNativeDescriptor, setWeightInKG)
        {
            if (setEquipDescriptor == "")
                equippableDescriptor.equip.Value = "You equip this and feel much better prepared for the Potsun Burran and its challenges.";
            else
                equippableDescriptor.equip.Value = setEquipDescriptor;

            if (setUnequipDescriptor == "")
                equippableDescriptor.unequip.Value = "Unequipping it with due care, you free yourself up for something else in its place.";
            else
                equippableDescriptor.unequip.Value = setUnequipDescriptor;
        }

        public override void Interact()
        {
            HandleEquipChoices();
        }
        public virtual void HandleEquipChoices()
        {
            if (this.currentlyInUse)
                ChoiceHandler.Add(new Choice($"Put down the {this.Name}.", Equip));
            else
                ChoiceHandler.Add(new Choice($"Take the {this.Name}.", Unequip));
        }
        public virtual void Equip()// Pass through Gear slot object?? player maybe??)
        {
            this.currentlyInUse = true;
            Console.WriteLine(equippableDescriptor.equip);
            //player.maxHealth + 1;         <-- How to get this working
        }

        public virtual void Unequip() // Ditto
        {
            this.currentlyInUse = false;
            Console.WriteLine(equippableDescriptor.unequip);
        }
        public string EquipDescriptor => equippableDescriptor.equip.Value;
        public string UnequipDescriptor => equippableDescriptor.unequip.Value;
    }
}
