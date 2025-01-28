using Little_Choice_Based_RPG.Objects.Base;
using Little_Choice_Based_RPG.Resources.Rooms;
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
        public uint CurrentRoomID { get; set; } = 0U; // this is for navigating to other rooms on its own
        public uint CurrentGameEnvironmentID { get; set; } = 0U; // this is just to let the room be found from the currentRoomID - for getting navigable directions out of the room from its ID
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

        public void MoveToRoom(RoomDirection direction) => this.CurrentRoomID = direction.DestinationRoomID;
    }
}
