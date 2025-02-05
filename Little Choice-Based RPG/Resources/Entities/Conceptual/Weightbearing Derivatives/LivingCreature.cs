using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Entities.Base.Weightbearing_Derivatives
{
    public class LivingCreature : MovingObject
    {
        private protected bool isAlive;
        private protected int health;
        private protected LivingCreature(uint setPosition) : base(setPosition)
        {
            this.Position = setPosition;
        }
        private protected LivingCreature(string setName, decimal setWeightInKG, uint setPosition, decimal setStrengthInKG)
            : base(setName, setPosition, setWeightInKG, setStrengthInKG)
        {
            isAlive = true;
            health = 100;
            this.Position = setPosition;
        }
    }
}
