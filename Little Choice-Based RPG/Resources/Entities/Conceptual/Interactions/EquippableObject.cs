using Little_Choice_Based_RPG.Resources.Entities.Conceptual.Descriptive_Derivatives;
using Little_Choice_Based_RPG.Types;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Entities.Conceptual.Interactions
{
    public abstract class EquippableObject : InteractableObject
    {
        public EquippableObject(string setName, uint setPosition, string newGenericDescriptor, string newInspectDescriptor, decimal setWeightInKG = 0m)
: base(setName, setPosition, newGenericDescriptor, newInspectDescriptor, setWeightInKG)
        {

        }
        /*
        public virtual void HandleEquipChoices()
        {
            if (this.currentlyInUse)
                ChoiceHandler.Add(new Choice($"Equip the {this.Name}.", Equip));
            else
                ChoiceHandler.Add(new Choice($"Unequip the {this.Name}.", Unequip));
        }
        */
        public virtual void ApplyModifiers()
        {

        }
        public virtual void RemoveModifiers()
        {

        }
        public void Equip()
        {
            Console.WriteLine(EquipDescriptor);
            ApplyModifiers();
        }
        public void Unequip()
        {
            Console.WriteLine(UnequipDescriptor);
            RemoveModifiers();
        }
        public string EquipDescriptor { get; set; } = "You equip this and feel much better prepared for the Potsun Burran and its challenges.";
        public string UnequipDescriptor { get; set; } = "Unequipping it with due care, you free yourself up for something else in its place.";
        public bool IsEquipped { get; set; } = false;
    }
}