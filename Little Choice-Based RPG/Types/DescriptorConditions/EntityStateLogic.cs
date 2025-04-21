using Little_Choice_Based_RPG.Resources;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory.InventoryExtensions;
using Little_Choice_Based_RPG.Types.EntityProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.DescriptorConditions
{
    public static class EntityStateLogic
    {
        /// <summary> Returns whether the existing entity states match the conditions' required entity states </summary>
        public static bool CheckConditionIsValid(PropertyContainer targetEntity, EntityStateDescriptor targetStateDescriptor)
        {
            if (!targetEntity.Extensions.ContainsExtension("ItemContainer"))
                throw new Exception($"This descriptors parent {targetEntity} didn't contain a reference for \"ItemContainer\" in its extensions list {targetEntity.Extensions}!");

            List<uint> validEntityStates = new(); //List of EntityID
            ItemContainer parentItemContainer = (ItemContainer)targetEntity.Extensions.GetExtension("ItemContainer");

            foreach (EntityState requiredEntityState in targetStateDescriptor.RequiredEntityStates)
            {
                //test the ID for all the objects in the room
                foreach (GameObject entity in parentItemContainer.Inventory)
                {
                    uint entityID = (uint)entity.Properties.GetPropertyValue("ID");

                    //when an object exists, test if it matches state
                    if (entityID == requiredEntityState.EntityReferenceID)
                    {
                        if (CheckEntityStateIsValid(requiredEntityState, entity))
                            validEntityStates.Add(entityID);
                    }
                }
            }

            if (validEntityStates.Count > targetStateDescriptor.RequiredEntityStates.Count)
                throw new Exception("Critical error during Room Descriptor assessment: There were more valid entity states than the maximum possible number of required states. Something went seriously wrong! The objects might have misconfigured IDs, check if the IDs are unique!");

            //if all the objects in a condition are valid, return true
            return validEntityStates.Count == targetStateDescriptor.RequiredEntityStates.Count;
        }

        /// <summary> 
        /// Returns the highest priority descriptors that remain valid. 
        /// 
        /// This is done per entity. For example, this method checks if an entity has been described as part of a room description.
        /// If it hasn't been described yet, the priorty list gets checked for further descriptions. 
        /// </summary>
        public static List<IDescriptorCondition> PrioritiseDescriptors(List<IDescriptorCondition> rawDescriptors, List<uint> targetIDsRemaining, out List<uint> remainingIDs)
        {
            uint previousPriority = 0;
            uint currentPriority;
            List<uint> higherPriorityObjectIDs = new();
            List<uint> previousSamePriorityObjectIDs = new();
            List<uint> currentObjectIDs = new();
            List<IDescriptorCondition> prioritisedDescriptors = new();

            List<IDescriptorCondition> sortedDescriptors = rawDescriptors.OrderBy(i => i.Priority).ToList();

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
                        descriptorRemainsValid = false;
                }

                //Assign a prioritised Descriptor
                if (descriptorRemainsValid)
                {
                    prioritisedDescriptors.Add(descriptor);

                    //Remove the IDs remaining.
                    foreach (uint testEntityID in currentObjectIDs)
                    {
                        if (!targetIDsRemaining.Contains(testEntityID))
                            throw new Exception($"Tried to remove testEntityID '{testEntityID}' from targetIDsRemaining {targetIDsRemaining}, but it wasn't in there to begin with!");
                        targetIDsRemaining.Remove(testEntityID);
                    }
                }

                //Always set previousPriority = currentPriority; then continue the loop.
                previousPriority = currentPriority;
            }

            //Return IDs that the priority system overlooked
            remainingIDs = targetIDsRemaining;

            return prioritisedDescriptors;
        }

        private static bool CheckEntityStateIsValid(EntityState testState, GameObject currentState)
        {

            if (testState.EntityReferenceID == (uint)currentState.Properties.GetPropertyValue("ID")) //check if the entityIDs match
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
                        string propertyName = requiredProperty.PropertyName;
                        object propertyValue = requiredProperty.PropertyValue;

                        if (currentState.Properties.HasPropertyAndValue(propertyName, propertyValue))
                            validProperties.Add(requiredProperty);
                    }

                    //if all properties match, then the state is valid
                    return validProperties.Count == testState.RequiredProperties.Count;
                }
            }
            else return false; //entity IDs didnt match.
        }
    }
}
