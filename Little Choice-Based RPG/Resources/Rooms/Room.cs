using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Xml;
using Little_Choice_Based_RPG.Choices;
using Little_Choice_Based_RPG.World.Managers;
using Little_Choice_Based_RPG.Resources.Entities.Base;

namespace Little_Choice_Based_RPG.Resources.Rooms
{
    public struct RoomDirection
    {
        public Direction ChosenDirection { get; private set; }
        public uint DestinationRoomID { get; private set; }
        public uint ObjectID { get; private set; }
        public RoomDirection(Direction setDirection, uint setDestinationRoomID, uint setObjectID = 0)
        {
            ChosenDirection = setDirection;
            DestinationRoomID = setDestinationRoomID;
            ObjectID = setObjectID;
        }
    }

    public class Room
    {
        private protected static uint currentID = 0;
        private protected uint uniqueID = 0;

        private protected List<RoomDescriptor> currentRoomDescriptors;

        private protected RoomType roomType;

        private protected List<List<GameObject>> RoomEntities = new List<List<GameObject>>();

        public Room(string setName, RoomType setRoomType)
        {
            uniqueID = ++currentID;
            roomType = setRoomType;
            Name = setName;
        }

        public Room(string setName, RoomType setRoomType, string newGenericDescriptor) : this(setName, setRoomType)
        {
            descriptors.generic = newGenericDescriptor;
        }

        public RoomType GetRoomType() => roomType;

        public void AddDirection(RoomDirection direction) => Directions.Add(direction);
        public void RemoveDirection(RoomDirection direction) => Directions.Remove(direction);

        public uint ID => uniqueID;
        public string Name { get; private protected set; }
        public List<RoomDirection> Directions { get; private protected set; } = new List<RoomDirection>();
    }

    public enum RoomType
    {
        Desert,
        Town
    }
    public enum Direction
    {
        North,
        East,
        South,
        West,
    }

    public class RoomDescriptor
    {
        public string descriptor;
        public List<GameObject> entityReferences;
    }

    public abstract class ConditionalRoomDescriptors
    {
    }
}
