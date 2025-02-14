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
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
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
    public record struct EntityConditions(uint EntityReferenceID, List<EntityProperty>? RequiredProperties); //An ID without an EntityProperty should just be checked for being present
    public record struct ConditionalDescriptor(string Descriptor, List<EntityConditions> RequiredEntityConditions, uint Priority = 6);

    public class Room
    {
        private protected static uint currentID = 0;
        private protected uint uniqueID = 0;
        private protected string defaultDescriptor;
        private protected List<GameObject> roomEntities = new List<GameObject>();
        private protected List<ConditionalDescriptor> localConditionalDescriptors = new List<ConditionalDescriptor>();

        public string Name;

        public Room(string setName, RoomType setRoomType, string setDefaultDescriptor) : base()
        {
            uniqueID = ++currentID;
            RoomType = setRoomType;
            Name = setName;
            defaultDescriptor = setDefaultDescriptor;
        }

        public void AddDirection(RoomDirection direction) => Directions.Add(direction);
        public void RemoveDirection(RoomDirection direction) => Directions.Remove(direction);

        public List<uint> GetRoomEntityIDs()
        {
            List<uint> entityIDs = new List<uint>();
            foreach (GameObject entity in roomEntities)
            {
                entityIDs.Add(entity.ID);
            }
            return entityIDs;
        }

        public List<string> GetRoomDescriptors()
        {
            List<string>? currentRoomDescriptors = ReturnValidConditionalDescriptors();

            if (currentRoomDescriptors == null)
                currentRoomDescriptors.Add(defaultDescriptor);
            return currentRoomDescriptors;
        }

        private protected List<string> ReturnValidConditionalDescriptors()
        {
            List<string> validRoomDescriptors = new();

            foreach (ConditionalDescriptor currentCondition in localConditionalDescriptors)
            {
                //records if entity states match the condition
                Dictionary<uint, bool> validEntityStates = new();

                //get each conditional ID from the condition
                foreach (EntityConditions currentEntityConditions in currentCondition.RequiredEntityConditions)
                {
                    //test the ID for all the objects in the room
                    foreach (GameObject existingObject in roomEntities)
                    {
                        //check for presence of items if there is no state
                        //when an object exists, test if it matches state
                        if (existingObject.ID == currentEntityConditions.EntityReferenceID)
                        {
                            // check state matches:
                            // if no properties exist, then add the object being present as a valid state.
                            if (currentEntityConditions.RequiredProperties == null)
                            {
                                validEntityStates.Add(existingObject.ID, true);
                            }
                            else //check for properties to match before adding as a valid state
                            {
                                Dictionary<EntityProperty, bool> validEntityPropertyStates = new();

                                foreach (EntityProperty currentProperty in currentEntityConditions.RequiredProperties)
                                {
                                    if(existingObject.entityProperties.Contains(currentProperty))
                                    {
                                        validEntityPropertyStates.Add(currentProperty, true);
                                    }
                                }

                                //if all properties match, mark the object as valid
                                if (validEntityPropertyStates.Count == currentEntityConditions.RequiredProperties.Count)
                                {
                                    validEntityStates.Add(existingObject.ID, true);
                                }
                            }
                        }
                    }
                }
                
                //if all the objects in a condition are valid, return its descriptor
                if (validEntityStates.Count == currentCondition.RequiredEntityConditions.Count)
                {
                    validRoomDescriptors.Add(currentCondition.Descriptor);
                }
            }

            return validRoomDescriptors;
        }

        public uint RoomID => uniqueID;
        public RoomType RoomType { get; init; }
        public List<RoomDirection> Directions { get; private protected set; } = new List<RoomDirection>();

    }
}
