using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Little_Choice_Based_RPG.Objects.Base;

namespace Little_Choice_Based_RPG.Entities.Derived.Equippables.Armour
{
    internal class Helmet : EquippableObject
    {
        private protected Helmet(Vector2 setPosition) : base(defaultName, defaultGenericDescriptor, setPosition, setWeightInKG: defaultWeightInKG)
        {
            base.Name.Value = defaultName;
            base.descriptor.generic.Value = "Folorn, a mudcaked helmet rots here in its own scrap, long since abandoned.";
            base.WeightInKG = 1.8m;
        }
    }
}
