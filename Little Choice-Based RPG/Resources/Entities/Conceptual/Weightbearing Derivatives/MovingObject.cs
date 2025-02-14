using Little_Choice_Based_RPG.Resources.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Entities.Conceptual.Weightbearing_Derivatives
{
    public class MovingObject : WeightbearingObject
    {
        private protected MovingObject(string setName, decimal setWeightInKG, uint setPosition, decimal setStrengthInKG)
            : base(setName, setPosition, setWeightInKG, setStrengthInKG)
        {
            this.Position = setPosition;
        }

        public void Move(uint newPosition)
        {
            this.Position = newPosition;
        }

        public void MoveToRoom(RoomDirection direction) => this.Position = direction.DestinationRoomID;
    }
}
