using Little_Choice_Based_RPG.Objects.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Entities.Derived
{
    public class MovingObject : WeightbearingObject
    {
        private protected MovingObject(Vector2 setPosition) : base(setPosition)
        {
            this.Position = setPosition;
        }
        private protected MovingObject(string setName, decimal setWeightInKG, Vector2 setPosition, decimal setStrengthInKG)
            : base(setName, setWeightInKG, setPosition, setStrengthInKG)
        {
        }

        public void Move(Vector2 amountMoved)
        {
            this.Position += amountMoved;
        }
    }
}
