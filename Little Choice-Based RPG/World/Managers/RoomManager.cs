using Little_Choice_Based_RPG.World.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.World.Managers
{
    internal class RoomManager
    {
        public void GenerateAllRooms()
        {
            string setNorthOfAtriiKaalGenericDescriptor = "Cool Desert Vibes, you see some rocks here.";
            string setNorthOfAtriiKaalInitialDescriptor = "You wake up with a start - you breath in sharply and sputter as heavy dust dries your mouth. \r\nIn front of you is the cracked and charred sandstone ground of the Potsun Burran. It glitters with the debris of a thousand shredded spaceships.\r\nThe high-pitched drone you hear subsides in to a roar as you realise you are laying on the ground, face-first.";
            Room northOfAtriiKaal = new Room("NorthOfAtriiKaal", setNorthOfAtriiKaalGenericDescriptor, setNorthOfAtriiKaalInitialDescriptor);
        }
    }
}