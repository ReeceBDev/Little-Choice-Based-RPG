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
using Little_Choice_Based_RPG.Resources.Entities.Base;
using System.Security.Principal;

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

    public record class RoomDescriptor(string Descriptor, List<uint>? EntityReferenceIDs = null);

    public class Room
    {
        private protected static uint currentID = 0;
        private protected uint uniqueID = 0;

        private protected RoomType roomType;
        private protected RoomDescriptor genericDescriptor;

        private protected List<GameObject> roomEntities = new List<GameObject>();

        public Room(string setName, RoomType setRoomType, string setDefaultDescriptor)
        {
            uniqueID = ++currentID;
            roomType = setRoomType;
            Name = setName;

            genericDescriptor = new RoomDescriptor(setDefaultDescriptor);
        }

        public RoomType GetRoomType() => roomType;

        public void AddDirection(RoomDirection direction) => Directions.Add(direction);
        public void RemoveDirection(RoomDirection direction) => Directions.Remove(direction);

        public uint RoomID => uniqueID;
        public string Name { get; private protected set; }
        public List<RoomDirection> Directions { get; private protected set; } = new List<RoomDirection>();

        public List<RoomDescriptor> GetRoomDescriptors()
        {
            List<RoomDescriptor>? currentRoomDescriptors = CheckDescriptorConditions();

            if (currentRoomDescriptors == null)
                currentRoomDescriptors.Add(genericDescriptor);
            return currentRoomDescriptors;
        }

        private protected virtual List<RoomDescriptor> CheckDescriptorConditions() => null;

        public List<uint> GetRoomEntityIDs()
        {
            List <uint> entityIDs = new List<uint>();
            foreach (GameObject entity in roomEntities)
            {
                entityIDs.Add(entity.ID);
            }
            return entityIDs;
        }
    }
}
