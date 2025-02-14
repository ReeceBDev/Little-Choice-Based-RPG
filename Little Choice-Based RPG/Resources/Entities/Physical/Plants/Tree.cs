using Little_Choice_Based_RPG.Resources.Choices;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Entities.Physical.Plants
{
    public class Tree : InteractableObject
    {
        public Tree(string setName, uint setPosition, string newGenericDescriptor, string newInspectDescriptor, decimal setWeightInKG = 0m)
    : base(setName, setPosition, newGenericDescriptor, newInspectDescriptor, setWeightInKG)
        {
            entityProperties.Add(new EntityProperty("isBurnt", true)); // temporarily burnt the trees
        }

        public override List<Choice> GenerateChoices()
        {
            return new List<Choice>();
        }
    }
}
