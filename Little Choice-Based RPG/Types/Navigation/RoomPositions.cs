using Little_Choice_Based_RPG.Resources.Entities.Rooms;
using System.Collections;

namespace Little_Choice_Based_RPG.Types.Navigation
{
    /// <summary> Represents a list of Rooms which may be accessed by ID or Coordinates. </summary>
    internal class RoomPositions : IEnumerable
    {
        /// <summary> Parameterless constructor as this class needs nothing for instantisation. </summary>
        public RoomPositions() 
        {
        }

        /// <summary> Copy constructor for replicating values to a new instantiation of the same type. </summary>
        public RoomPositions(RoomPositions master)
        {
            RoomList = master.RoomList;
            CoordinateRoomIDs = master.CoordinateRoomIDs;
        }

        /// <summary> Adds a new Room to the RoomList. 
        /// Add an off-grid room by setting it's co-ordinates as null. Off-grids rooms exist outside of the co-ordinate map system. </summary>
        public void Add(Coordinates? location, Room roomRef) => Add(location?.X, location?.Y, location?.Z, roomRef);

        /// <summary> Adds a new Room to the RoomList. 
        /// Add an off-grid room by setting it's co-ordinates as null. Off-grids rooms exist outside of the co-ordinate map system. </summary>
        public void Add(int? x, int? y, int? z, Room roomRef)
        {
            if (!roomRef.Extensions.Contains("ItemContainer"))
                throw new Exception($"The room {roomRef} has not been fully initialised yet! It didn't have an ItemContainer to store anything, and was therefore inaccessible!");

            uint roomID = (uint)roomRef.Properties.GetPropertyValue("ID");

            //Check either none or all of the coordinates are null at once
            if ((x == null) || (y == null) || (z == null))
            {
                if (!((x == null) && (y == null) && (z == null)))
                    throw new Exception($"Uneven null values! Only some of the values of this coordinate were set, but the others were null. Either all of the should be null, or none of them! X:{x}, Y:{y}, Z:{z}.");

                //If all of the coordinates are null, set as off-grid.
                OffGridRooms.Add(roomID, roomRef);
                return;
            }

            Coordinates newCoordinates = new Coordinates((int)x, (int)y, (int)z);

            RoomList.Add(newCoordinates, (roomRef, roomID));
            CoordinateRoomIDs.Add(roomID, newCoordinates);
        }

        /// <summary> Removes a room from the list by Coordinates. Can only remove rooms which have coordinates. </summary>
        public void Remove(Coordinates location) => Remove(location.X, location.Y, location.Z);

        /// <summary> Removes a room from the list by Room reference. Can remove rooms with no coordinates (Off-Grid). </summary>
        public void Remove(Room roomRef) => Remove((uint)roomRef.Properties.GetPropertyValue("ID"));

        /// <summary> Removes a room from the list by coordinates. Can only remove rooms which have coordinates. </summary>
        public void Remove(int x, int y, int z)
        {
            Coordinates targetCoordinates = new Coordinates(x, y, z);
            uint targetID = RoomList[targetCoordinates].RoomID;

            RoomList.Remove(targetCoordinates);
            CoordinateRoomIDs.Remove(targetID);
        }

        /// <summary> Removes a room from the list by ID. Can remove rooms with no coordinates (Off-Grid). </summary>
        public void Remove(uint roomID)
        {
            //Off-Grid
            if (OffGridRooms.ContainsKey(roomID))
            {
                OffGridRooms.Remove(roomID);
                return;
            }

            //On-Grid
            Coordinates targetCoordinates = CoordinateRoomIDs[roomID];

            RoomList.Remove(targetCoordinates);
            CoordinateRoomIDs.Remove(roomID);
        }


        /// <summary> Returns a room by Room ID. May return off-room grids. </summary>
        public Room GetRoom(uint roomID)
        {
            //Off-grid
            if (OffGridRooms.ContainsKey(roomID))
                return OffGridRooms[roomID];

            //On-grid
            return GetRoom(CoordinateRoomIDs[roomID]);
        }

        /// <summary> Returns a room by coordinates as X, Y, Z. Only returns rooms with co-ordinates assigned. </summary>
        public Room GetRoom(int x, int y, int z) => GetRoom(new Coordinates(x, y, z));

        /// <summary> Returns a room by coordinates as type Coordinates. Only returns rooms with co-ordinates assigned. </summary>
        public Room GetRoom(Coordinates roomCoordinates) => RoomList[roomCoordinates].RoomRef;


        /// <summary> Check if a set of coordinates exist in this list, by X, Y, Z. </summary>
        public bool HasCoordinates(int x, int y, int z) => HasCoordinates(new Coordinates(x, y, z));

        /// <summary> Check if a set of coordinates exist in this list, by type Coordinates. </summary>
        public bool HasCoordinates(Coordinates roomCoordinates) => RoomList.ContainsKey(roomCoordinates);

        /// <summary> Check if a room exists in this list by Room ID. </summary>
        public bool HasRoom(uint roomID) 
            => CoordinateRoomIDs.ContainsKey(roomID) || OffGridRooms.ContainsKey(roomID); //On-grid || Off-Grid.

        /// <summary> Check if a room exists in this list by Room Reference. </summary>
        public bool HasRoom(Room roomRef) => HasRoom((uint)roomRef.Properties.GetPropertyValue("ID"));


        /// <summary> Return coordinates, by Room ID. May return null, if the target is off-grid. </summary>
        public Coordinates? GetCoordinates(uint roomID)
            => OffGridRooms.ContainsKey(roomID) ? null : CoordinateRoomIDs[roomID]; //Off-grid : On-Grid.

        /// <summary> Return coordinates, by Room reference. May return null, if the target is off-grid. </summary>
        public Coordinates? GetCoordinates(Room roomRef) => GetCoordinates((uint) roomRef.Properties.GetPropertyValue("ID"));


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public RoomPositionsEnumerator GetEnumerator()
        {
            return new RoomPositionsEnumerator(RoomList, CoordinateRoomIDs, OffGridRooms);
        }

        /// <summary> Accessed with Coordinates, provides the RoomID and RoomReference. </summary>
        public Dictionary<Coordinates, (Room RoomRef, uint RoomID)> RoomList { get; private set; } = new(); //Coordinates, RoomRef, RoomID

        /// <summary> Accessed with RoomID, provides Coordinates, which can then be used again to retrieve the RoomReference itself. </summary>
        public Dictionary<uint, Coordinates> CoordinateRoomIDs { get; private set; } = new(); //RoomID, Coordinates

        /// <summary> Off-grid rooms have null co-ordinates. Therefore, their list should just be RoomRef to RoomID, and the null coordinate assumed upon getting. </summary>
        public Dictionary<uint, Room> OffGridRooms { get; private set; } = new(); //RoomID, RoomRef
    }
}
