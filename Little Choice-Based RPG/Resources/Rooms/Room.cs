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
using System.Runtime.InteropServices.Marshalling;
using System.Collections.Immutable;
using System.Collections;
using System.Runtime.ConstrainedExecution;
using Little_Choice_Based_RPG.Resources.Entities.Immaterial.Transition;

namespace Little_Choice_Based_RPG.Resources.Rooms
{
    public enum RoomType
    {
        Desert,
        Town
    }
    public record struct EntityState(uint EntityReferenceID, List<EntityProperty>? RequiredProperties); //An ID without an EntityProperty should just be checked for being present
    public record struct ConditionalDescriptor(string Descriptor, List<EntityState> RequiredEntityStates, uint Priority = 6);

    public class Room
    {
        private protected static uint currentID = 0U; // 0 is an null, Invalid ID
        private protected uint uniqueID;
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

        public List<uint> GetRoomEntityIDs()
        {
            List<uint> entityIDs = new List<uint>();
            foreach (GameObject entity in roomEntities)
            {
                entityIDs.Add(entity.ID);
            }
            return entityIDs;
        }

        public List<GameObject> GetRoomObjects()
        {
            List<GameObject> validObjects = new List<GameObject>();
            EntityProperty immaterialProperty = new EntityProperty("isImmaterial", true);

            foreach (GameObject entity in roomEntities)
            {
                if (!entity.entityProperties.Contains(immaterialProperty))
                    validObjects.Add(entity);
            }

            return validObjects;
        }

        public List<GameObject> GetRoomObjects(EntityProperty requiredEntityProperty)
        {
            List<GameObject> validObjects = new List<GameObject>();
            EntityProperty immaterialProperty = new EntityProperty("isImmaterial", true);

            foreach (GameObject entity in roomEntities)
            {
                if (!entity.entityProperties.Contains(immaterialProperty))
                {
                    if (entity.entityProperties.Contains(requiredEntityProperty))
                        validObjects.Add(entity);
                }
            }
            return validObjects;
        }

        public List<GameObject> GetRoomObjects(List<EntityProperty> requiredProperties)
        {
            List<GameObject> validObjects = new List<GameObject>();
            EntityProperty immaterialProperty = new EntityProperty("isImmaterial", true);

            foreach (GameObject entity in roomEntities)
            {
                if (!entity.entityProperties.Contains(immaterialProperty))
                {
                    //Check the required properties are all contained within the entity
                     List<EntityProperty> validProperties = new();

                    //foreach (EntityProperty currentProperty in requiredProperties)
                    foreach (EntityProperty requiredProperty in requiredProperties)
                    {
                        if (entity.entityProperties.Contains(requiredProperty))
                            validProperties.Add(requiredProperty);
                    }

                    //if there are all of the validProperties, then add the entity to validObjects
                    if (validProperties.Count == requiredProperties.Count)
                        validObjects.Add(entity);
                }
            }
            return validObjects;
        }

        public List<string> GetRoomDescriptors()
        {
            List<string> currentRoomDescriptors = ReturnValidConditionalDescriptors();

            if (currentRoomDescriptors.Count() == 0)
                currentRoomDescriptors.Add(defaultDescriptor);
            return currentRoomDescriptors;
        }

        private protected List<string> ReturnValidConditionalDescriptors()
        {
            List<ConditionalDescriptor> validRoomDescriptors = new(); //Descriptor, Priority

            foreach (ConditionalDescriptor currentCondition in localConditionalDescriptors)
            {
                if (CheckConditionIsValid(currentCondition, roomEntities))
                    validRoomDescriptors.Add(currentCondition);
            }

            List<string> prioritisedDescriptors = PrioritiseDescriptorsAsStrings(validRoomDescriptors);
            return prioritisedDescriptors;
        }
        
        private protected bool CheckConditionIsValid(ConditionalDescriptor condition, List<GameObject> roomEntities)
        {
            //records if existing entity states match the conditions' required entity states
            List<uint> validEntityStates = new(); //EntityID

            foreach (EntityState requiredEntityState in condition.RequiredEntityStates)
            {
                //test the ID for all the objects in the room
                foreach (GameObject entity in roomEntities)
                {
                    //when an object exists, test if it matches state
                    if (entity.ID == requiredEntityState.EntityReferenceID)
                    {
                        if (CheckStateIsValid(requiredEntityState, entity))
                            validEntityStates.Add(entity.ID);
                    }
                }
            }

            //if all the objects in a condition are valid, return true
            return validEntityStates.Count == condition.RequiredEntityStates.Count;
        }

        private protected bool CheckStateIsValid(EntityState testState, GameObject currentState)
        {

            if (testState.EntityReferenceID == currentState.ID) //check if the entityIDs match
            {
                //If no properties exist, then the object will be considered a match by default, since there are no properties being checked, just presence.
                if (testState.RequiredProperties == null)
                    return true;
                else //check for properties to match before adding as a valid state
                {
                    //Check if each property matches the required properties
                    List<EntityProperty> validProperties = new();

                    //foreach (EntityProperty currentProperty in testState.RequiredProperties)
                    foreach (EntityProperty requiredProperty in testState.RequiredProperties)
                    {
                        if (currentState.entityProperties.Contains(requiredProperty))
                            validProperties.Add(requiredProperty);
                    }

                    //if all properties match, then the state is valid
                    return validProperties.Count == testState.RequiredProperties.Count;
                }
            }
            else return false; //entity IDs didnt match.
        }

        public List<string> PrioritiseDescriptorsAsStrings(List<ConditionalDescriptor> rawDescriptors)
        {
            uint previousPriority = 0;
            uint currentPriority;
            List<uint> higherPriorityObjectIDs = new();
            List<uint> previousSamePriorityObjectIDs = new();
            List<uint> currentObjectIDs = new();
            List<string> prioritisedStrings = new();

            List<ConditionalDescriptor> sortedDescriptors = rawDescriptors.OrderBy(i => i.Priority).ToList();

            foreach (var descriptor in sortedDescriptors)
            {
                //Always set current priority
                currentPriority = descriptor.Priority;

                //If currentPriority is lower than previous priority:
                if (currentPriority > previousPriority)
                {
                    //Then also higherpriorityobjects += previousSamePriorityObjects;
                    foreach (uint newPreviousSamePriorityObjectID in previousSamePriorityObjectIDs)
                    {
                        if (!higherPriorityObjectIDs.Contains(newPreviousSamePriorityObjectID))
                            higherPriorityObjectIDs.Add(newPreviousSamePriorityObjectID);
                    }
                    //Then also clear previousSamePriorityObjects
                    previousSamePriorityObjectIDs.Clear();
                }

                //Always clear currentObjectIDs;
                currentObjectIDs.Clear();

                foreach (EntityState descriptorEntityState in descriptor.RequiredEntityStates)
                {
                    //Always set currentObjectIDs;
                    if (!currentObjectIDs.Contains(descriptorEntityState.EntityReferenceID))
                        currentObjectIDs.Add(descriptorEntityState.EntityReferenceID);

                    //previousSamePriorityObjects += currentObjectIDs;
                    if (!previousSamePriorityObjectIDs.Contains(descriptorEntityState.EntityReferenceID))
                        previousSamePriorityObjectIDs.Add(descriptorEntityState.EntityReferenceID);
                }

                //currentObjectIDs should always be checked to see if items already exist in higherpriorityobjects.
                //If they dont exist, this descriptor will be added valid.
                bool descriptorRemainsValid = true;
                foreach (uint testEntityID in currentObjectIDs)
                {
                    if (higherPriorityObjectIDs.Contains(testEntityID))
                    {
                        descriptorRemainsValid = false;
                    }
                }

                if (descriptorRemainsValid)
                    prioritisedStrings.Add(descriptor.Descriptor);

                //Always set previousPriority = currentPriority;
                previousPriority = currentPriority;
            }

            return prioritisedStrings;
        }

        public uint RoomID => uniqueID;
        public RoomType RoomType { get; init; }
        public List<RoomDirection> Directions { get; private protected set; } = new List<RoomDirection>();

    }
}
