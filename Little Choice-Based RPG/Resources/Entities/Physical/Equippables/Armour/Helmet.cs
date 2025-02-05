using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Little_Choice_Based_RPG.Resources.Entities.Base.Descriptive_Derivatives;

namespace Little_Choice_Based_RPG.Resources.Entities.Physical.Equippables.Armour
{
    internal abstract class Helmet : EquippableObject
    {
        private protected Helmet(uint setPosition) : base(setPosition)
        {
            Position = setPosition;
            WeightInKG = 1.8m;
        }
    }
}
