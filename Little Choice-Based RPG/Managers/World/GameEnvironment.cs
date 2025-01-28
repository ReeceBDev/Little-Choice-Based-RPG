using Little_Choice_Based_RPG.Entities.Derived.Living.Players;
using Little_Choice_Based_RPG.Resources.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Managers.World
{
    public class GameEnvironment
    {
        private protected static uint currentID = 0;
        public uint UniqueID { get; init; }

        public List<Room> Rooms = new List<Room>();

        public GameEnvironment()
        {
            UniqueID = ++currentID;
        }

        public void GenerateAllRooms()
        {
            string setNorthOfAtriiKaalGenericDescriptor = "Cool Desert Vibes, you see some rocks here.";
            string setNorthOfAtriiKaalInitialDescriptor = "You wake up with a start - you breath in sharply and sputter as heavy dust dries your mouth. \r\nIn front of you is the cracked and charred sandstone ground of the Potsun Burran. It glitters with the debris of a thousand shredded spaceships.\r\nThe high-pitched drone you hear subsides in to a roar as you realise you are laying on the ground, face-first.";
            Room northOfAtriiKaal = new Room("NorthOfAtriiKaal", RoomType.Desert, setNorthOfAtriiKaalGenericDescriptor, setNorthOfAtriiKaalInitialDescriptor);

            Room AtriiKaal = new Room("AtriiKaal", RoomType.Town, "beans", "beans beans beans");

            uint uglyRoomID1 = northOfAtriiKaal.ID;
            uint tempRoomID2uglybastard = AtriiKaal.ID;

            RoomDirection oneToTwo = new RoomDirection(Direction.South, tempRoomID2uglybastard);
            northOfAtriiKaal.AddDirection(oneToTwo);
            RoomDirection twoToOne = new RoomDirection(Direction.North, uglyRoomID1);
            AtriiKaal.AddDirection(twoToOne);

            Rooms.Add(northOfAtriiKaal);
            Rooms.Add(AtriiKaal);

        }

        public Room? FindRoomByID(uint ID)
        {
            foreach (Room room in Rooms)
            {
                if (ID == room.ID)
                    return room;
            }

            return null;
        }
        /*
        public void LinkRooms()
        {
            Room.Add :<<<
        }
        */
    }
}