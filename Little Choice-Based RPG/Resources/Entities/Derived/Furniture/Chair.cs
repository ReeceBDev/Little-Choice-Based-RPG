using Little_Choice_Based_RPG.Objects.Base;
using Little_Choice_Based_RPG.Resources.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Entities.Derived.Furniture
{
    public class Chair : InteractableObject
    {
        public Chair(string setName, uint setPosition, string newGenericDescriptor, string newInspectDescriptor, decimal setWeightInKG = 0m)
    : base(setName, setPosition, newGenericDescriptor, newInspectDescriptor, setWeightInKG)
        {

        }

        public override List<Choice> GenerateChoices()
        {
           return new List<Choice>();
        }

        public bool isUntouched { get; set; } = true;
        public bool isBurnt { get; set; } = false;
    }
}
