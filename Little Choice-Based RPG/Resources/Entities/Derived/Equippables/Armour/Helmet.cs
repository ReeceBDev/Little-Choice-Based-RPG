using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Little_Choice_Based_RPG.Choices;
using Little_Choice_Based_RPG.Objects.Base;

namespace Little_Choice_Based_RPG.Resources.Entities.Derived.Equippables.Armour
{
    internal abstract class Helmet : EquippableObject
    {
        private protected Helmet(Vector2 setPosition) : base(setPosition)
        {
            Position = setPosition;
            WeightInKG = 1.8m;
        }
    }
}
