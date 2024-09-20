using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Little_Choice_Based_RPG.Objects.Base;

namespace Little_Choice_Based_RPG.Objects.Gear.Armour
{
    internal class Helmet : EquippableObject
    {
        private protected const bool isHelmet = true;
        public bool IsHelmet { get; }
    }
}
