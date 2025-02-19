using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual.Interactions;

namespace Little_Choice_Based_RPG.Resources.Entities.Physical.Equippables.Armour
{
    internal abstract class Helmet : EquippableObject
    {
        private protected Helmet(string setName, string newGenericDescriptor, string newInspectDescriptor, decimal setWeightInKG = 0m)
: base(setName, newGenericDescriptor, newInspectDescriptor, setWeightInKG)
        {
            WeightInKG = 1.8m;
        }
    }
}
