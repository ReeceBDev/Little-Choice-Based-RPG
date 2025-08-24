using Little_Choice_Based_RPG.Resources.Entities.Rooms;

namespace Little_Choice_Based_RPG.Types.TypedEventArgs
{
    internal class PlayerChangedRoomEventArgs : EventArgs
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
