using Little_Choice_Based_RPG.Resources.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Entities.Base.Weightbearing_Derivatives
{
    public class MovingObject : WeightbearingObject
    {
        public uint CurrentRoomID { get; set; } = 0U; // this is for navigating to other rooms on its own
        public uint CurrentGameEnvironmentID { get; set; } = 0U; // this is just to let the room be found from the currentRoomID - for getting navigable directions out of the room from its ID
        private protected MovingObject(uint setPosition) : base(setPosition)
        {
            this.Position = setPosition;
        }
        private protected MovingObject(string setName, uint setPosition, decimal setWeightInKG, decimal setStrengthInKG)
            : base(setName, setPosition, setWeightInKG, setStrengthInKG)
        {
        }

        public void Move(uint newPosition)
        {
            this.Position = newPosition;
        }

        public void MoveToRoom(RoomDirection direction) => this.CurrentRoomID = direction.DestinationRoomID;
    }
}
