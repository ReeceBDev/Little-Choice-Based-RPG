using Little_Choice_Based_RPG.Resources.Choices;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual.Descriptive_Derivatives;
using Little_Choice_Based_RPG.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Entities.Conceptual.Interactions
{
    public abstract class InteractableObject : DescriptiveObject
    {
        public InteractableObject(string setName, uint setPosition, string newGenericDescriptor, string newInspectDescriptor, decimal setWeightInKG = 0m)
    : base(setName, setPosition, newGenericDescriptor, newInspectDescriptor, setWeightInKG)
        {

        }
        public abstract List<Choice> GenerateChoices();
    }
}
