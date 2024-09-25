using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Entities.Derived
{
    public class LivingCreature : MovingObject
    {
        private protected bool isAlive;
        private protected int health;
        private protected LivingCreature(Vector2 setPosition) : base(setPosition)
        {
            this.Position = setPosition;
        }
        private protected LivingCreature(string setName, decimal setWeightInKG, Vector2 setPosition, decimal setStrengthInKG)
            : base(setName, setWeightInKG, setPosition, setStrengthInKG)
        {
            isAlive = true;
            health = 100;
            this.Position = setPosition;
        }
    }
}
