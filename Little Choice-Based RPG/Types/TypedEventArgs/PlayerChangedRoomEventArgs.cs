﻿using Little_Choice_Based_RPG.Resources.Rooms;

namespace Little_Choice_Based_RPG.Types.TypedEventArgs
{
    public class PlayerChangedRoomEventArgs : EventArgs
    {
        public PlayerChangedRoomEventArgs(Room oldRoom, Room newRoom)
        {
            OldRoom = oldRoom;
            NewRoom = newRoom;
        }

        public Room OldRoom { get; init; }
        public Room NewRoom { get; init; }
    }
}
