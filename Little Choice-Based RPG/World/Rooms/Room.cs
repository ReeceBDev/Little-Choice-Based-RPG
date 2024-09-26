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
using Little_Choice_Based_RPG.Objects.Base;
using Little_Choice_Based_RPG.Choices;
using Little_Choice_Based_RPG.World.Managers;

namespace Little_Choice_Based_RPG.World.Rooms
{
    public enum Direction
    {
        North,
        East,
        South,
        West,
    }
    public struct RoomDirection
    {
        public Direction ChosenDirection { get; private set; }
        public uint DestinationRoomID { get; private set; }
        public uint ObjectID { get; private set; }
        public RoomDirection (Direction setDirection, uint setDestinationRoomID, uint setObjectID = 0)
        {
            ChosenDirection = setDirection;
            DestinationRoomID = setDestinationRoomID;
            ObjectID = setObjectID;
        }
    }

    public struct RoomDescriptor
    {
        public string generic;
        public string initial;
        public string distant;
        public string flair;
    }

    public enum RoomType
    {
        Desert,
        Town
    }

    // handles rooms, each room has a scenic description, a description of what it looks like from other rooms, a list of objects that are sitting inside it, currently accessible directions to other rooms, visibility (i.e. fogginess) 0-4 clear (can view infinitely far), light fog (can view 3 far), medium fog (can view 2 far), heavy fog (can view 1 far), dark (cant view)
    public class Room
    {
        private protected static uint currentID = 0;
        private protected uint uniqueID = 0;

        private protected RoomDescriptor descriptors;

        private protected RoomType roomType = RoomType.Desert;

        private protected bool hasPlayerVisited;

        private protected List<List<GameObject>> internalEntityGrid = new List<List<GameObject>>();
        private protected List<RoomDirection> directions = new List<RoomDirection>();

        public Room(string setName, RoomType setRoomType)
        {
            this.uniqueID = ++currentID;
            this.roomType = setRoomType;
            this.Name = setName;
        }

        public Room(string setName, RoomType setRoomType, string newGenericDescriptor, string newInitialDescriptor = "", string newDistantDescriptor = "",
            int visibility = 3) : this(setName, setRoomType)
        {
            this.PhysicalVisibility = visibility;

            descriptors.generic = newGenericDescriptor;

            if (newInitialDescriptor == "")
                descriptors.initial = newGenericDescriptor; //if Initial is empty, copy Generic
            else
                descriptors.initial = newInitialDescriptor;

            if (newDistantDescriptor == "")
                descriptors.distant = newGenericDescriptor; //if Distant is empty, copy Generic
            else
                descriptors.distant = newDistantDescriptor;
        }

        public RoomType GetRoomType() => roomType;

        public void AddDirection(RoomDirection direction) => directions.Add(direction);
        public void RemoveDirection(RoomDirection direction) => directions.Remove(direction);

        public uint ID => uniqueID;
        public RoomDescriptor Descriptors => descriptors;
        public string Name { get; private protected set; }
        public List<RoomDirection> Directions { get; private set; } = directions;
        public int PhysicalVisibility { get; private protected init; }

    }
}
