using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory.InventoryExtensions;
using Little_Choice_Based_RPG.Resources.Systems.InformationalSystems.Descriptor.DescriptorExtensions;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PublicInteractionsSystems;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.RoomSpecificTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems.DescriptorSystems.Descriptor
{
    public static class DescriptorLogic
    {
        /// <summary> Takes a target and the name of a descriptor to retrieve it. Considers conditional descriptors of the same identity before returning. 
        /// "Descriptor." may be omitted for the descriptorIdentity.
        public static string GetDescriptor(PropertyContainer targetContainer, string descriptorIdentity)
        {
            string currentDescriptor;
            string descriptorName = (descriptorIdentity.StartsWith("Descriptor.")) ? descriptorIdentity : $"Descriptor.{descriptorIdentity}";

            currentDescriptor = targetContainer.Extensions.ContainsExtension("ConditionalDescriptors") ? 
                GetConditionalDescriptor(targetContainer, descriptorName) : GetRegularDescriptor(targetContainer, descriptorIdentity);

            return currentDescriptor;
        }

        private static string GetRegularDescriptor(PropertyContainer targetContainer, string descriptorName)
        {
            if (!targetContainer.Properties.HasExistingPropertyName(descriptorName))
                throw new ArgumentException($"The target {targetContainer} didn't contain a descriptor of the name {descriptorName}! " +
                    $"The properties list was {targetContainer.Properties}.");

            return (string)targetContainer.Properties.GetPropertyValue(descriptorName);
        }

        private static string GetConditionalDescriptor(PropertyContainer targetContainer, string descriptorName)
        {
            if (!targetContainer.Extensions.ContainsExtension("ConditionalDescriptors"))
                throw new ArgumentException($"The target {targetContainer} didn't contain an extension of the identifier \"ConditionalDescriptors\"! " +
                    $"The extensions list was {targetContainer.Extensions}.");

            if (!targetContainer.Extensions.ContainsExtension("ItemContainer"))
                throw new ArgumentException($"The target {targetContainer} didn't contain an extension of the identifier \"ItemContainer\"! " +
                    $"The extensions list was {targetContainer.Extensions}.");

            string concatenatedValidDescriptors = "";
            List<ConditionalDescriptor> relevantDescriptors = new List<ConditionalDescriptor>();
            List<ConditionalDescriptor> prioritisedDescriptors = new List<ConditionalDescriptor>();

            ConditionalDescriptors storedDescriptors = (ConditionalDescriptors) targetContainer.Extensions.GetExtension("ConditionalDescriptors");
            ItemContainer storedEntities = (ItemContainer)targetContainer.Extensions.GetExtension("ItemContainer");

            //Grab relevant descriptors
            foreach (var descriptor in storedDescriptors.Descriptors)
            {
                if (descriptor.Identity == descriptorName) //Descriptor matches the required descriptorName
                    if (CheckConditionIsValid(descriptor, storedEntities.Inventory)) //Descriptor is matches the
                        relevantDescriptors.Add(descriptor);
            }

            //Prioritise the relevant descriptors
            prioritisedDescriptors = PrioritiseDescriptors(relevantDescriptors);

            //Concatenate the prioritised descriptors.
            foreach (var descriptor in prioritisedDescriptors)
                concatenatedValidDescriptors += $"descriptor.Descriptor ";

            return concatenatedValidDescriptors;
        }

        /// <summary> Returns whether the existing entity states match the conditions' required entity states </summary>
        private static bool CheckConditionIsValid(ConditionalDescriptor condition, List<GameObject> roomEntities)
        {
            List<uint> validEntityStates = new(); //EntityID

            foreach (EntityState requiredEntityState in condition.RequiredEntityStates)
            {
                //test the ID for all the objects in the room
                foreach (GameObject entity in roomEntities)
                {
                    uint entityID = (uint)entity.Properties.GetPropertyValue("ID");

                    //when an object exists, test if it matches state
                    if (entityID == requiredEntityState.EntityReferenceID)
                    {
                        if (CheckStateIsValid(requiredEntityState, entity))
                            validEntityStates.Add(entityID);
                    }
                }
            }

            if (validEntityStates.Count > condition.RequiredEntityStates.Count)
                throw new Exception("Critical error during Room Descriptor assessment: There were more valid entity states than the maximum possible number of required states. Something went seriously wrong! The objects might have misconfigured IDs, check if the IDs are unique!");

            //if all the objects in a condition are valid, return true
            return validEntityStates.Count == condition.RequiredEntityStates.Count;
        }

        private static bool CheckStateIsValid(EntityState testState, GameObject currentState)
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

        private static List<ConditionalDescriptor> PrioritiseDescriptors(List<ConditionalDescriptor> rawDescriptors)
        {
            uint previousPriority = 0;
            uint currentPriority;
            List<uint> higherPriorityObjectIDs = new();
            List<uint> previousSamePriorityObjectIDs = new();
            List<uint> currentObjectIDs = new();
            List<ConditionalDescriptor> prioritisedDescriptors = new();

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
                        descriptorRemainsValid = false;
                }

                if (descriptorRemainsValid)
                    prioritisedDescriptors.Add(descriptor);

                //Always set previousPriority = currentPriority; then continue the loop.
                previousPriority = currentPriority;
            }

            return prioritisedDescriptors;
        }

    }
}
