using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Resources.Rooms.Premade.Unique.Test;
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

        public Dictionary<uint, Room> Rooms = new Dictionary<uint, Room>();

        public GameEnvironment()
        {
            UniqueID = ++currentID;
        }

        public void GenerateAllRooms()
        {
            string setNorthOfAtriiKaalDefaultDescriptor = "You wake up with a start - you breath in sharply and sputter as heavy dust dries your mouth. \r\nIn front of you is the cracked and charred sandstone ground of the Potsun Burran. It glitters with the debris of a thousand shredded spaceships.\r\nThe high-pitched drone you hear subsides in to a roar as you realise you are laying on the ground, face-first.";

            Room northOfAtriiKaal = new Room ("NorthOfAtriiKaal", RoomType.Desert, setNorthOfAtriiKaalDefaultDescriptor);
            Rooms.Add(northOfAtriiKaal.RoomID, northOfAtriiKaal);

            BurgundyTree burgundyTree = new BurgundyTree("A Burgundy Tree", RoomType.Desert, "Arid desert sand whips by your skin. A few burgundy leaves drift across the floor.");
            Rooms.Add(burgundyTree.RoomID, burgundyTree);
        }

        public Room? GetRoomByID(uint ID)
        {
            foreach (KeyValuePair<uint, Room> roomList in Rooms)
            {
                if (roomList.Key.Equals(ID))
                    return roomList.Value;
            }

            return null;
        }

        /*
        private void InstantiateRoom()
        {
        //Add code to do all the room and key adding stuff here.
        }
        */
        /*
        public void LinkRooms()
        {
            Room.Add :<<<
        }
        */
    }
}