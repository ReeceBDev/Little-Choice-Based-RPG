using Little_Choice_Based_RPG.Resources.Rooms;
using System.Collections;

namespace Little_Choice_Based_RPG.Types.Navigation
{
    public class RoomPositionsEnumerator : IEnumerator
    {
        private int enmerationPosition = -1; //This must be -1 as enumeration begins when running MoveNext() once.

        public RoomPositionsEnumerator(Dictionary<Coordinates, (Room RoomRef, uint RoomID)> setRoomList, Dictionary<uint, Coordinates> setCoordinateRoomIDs, 
            Dictionary<uint, Room> setOffGridRooms)
        {
            RoomList = setRoomList;
            CoordinateRoomIDs = setCoordinateRoomIDs;
            OffGridRooms = setOffGridRooms;
        }

        public bool MoveNext()
        {
            enmerationPosition++;
            return enmerationPosition < (RoomList.Count + OffGridRooms.Count);
        }

        public void Reset() => enmerationPosition = -1; //Resets to default value

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public (Room RoomRef, uint RoomID, Coordinates? RoomCoordinates) Current
        {
            get
            {
                try
                {
                    Coordinates? currentCoordinates;
                    Room currentRoomRef;
                    uint currentRoomID;

                    //Get On-Grids first
                    if (enmerationPosition < RoomList.Count)
                    {
                        KeyValuePair<Coordinates, (Room RoomRef, uint RoomID)> selectedEntry = RoomList.ElementAt(enmerationPosition);

                        currentCoordinates = selectedEntry.Key;
                        currentRoomRef = selectedEntry.Value.RoomRef;
                        currentRoomID = selectedEntry.Value.RoomID;
                    }
                    else //Off-Grids
                    {
                        int offGridEnumeration = enmerationPosition - RoomList.Count;

                        KeyValuePair<uint, Room> selectedEntry = OffGridRooms.ElementAt(offGridEnumeration);

                        currentCoordinates = null;
                        currentRoomRef = selectedEntry.Value;
                        currentRoomID = selectedEntry.Key;
                    }

                    (Room RoomRef, uint RoomID, Coordinates? RoomCoordinates) currentRoomData
                        = new(currentRoomRef, currentRoomID, currentCoordinates);

                    return currentRoomData;
                }
                catch (IndexOutOfRangeException)
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public Dictionary<Coordinates, (Room RoomRef, uint RoomID)> RoomList { get; private set; } = new(); //Coordinates, Room, RoomID
        public Dictionary<uint, Coordinates> CoordinateRoomIDs { get; private set; } = new(); //RoomID, Coordinates
        public Dictionary<uint, Room> OffGridRooms { get; private set; } = new(); //RoomID, RoomRef
    }
}
