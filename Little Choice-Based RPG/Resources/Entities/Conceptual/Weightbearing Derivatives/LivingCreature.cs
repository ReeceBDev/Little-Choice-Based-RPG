using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Entities.Conceptual.Weightbearing_Derivatives
{
    public class LivingCreature : MovingObject
    {
        private protected bool isAlive;
        private protected int health;
        private protected LivingCreature(string setName, decimal setWeightInKG = 0m, decimal setStrengthInKG = 0m)
            : base(setName, setWeightInKG, setStrengthInKG)
        {
            isAlive = true;
            health = 100;
        }
    }
}
