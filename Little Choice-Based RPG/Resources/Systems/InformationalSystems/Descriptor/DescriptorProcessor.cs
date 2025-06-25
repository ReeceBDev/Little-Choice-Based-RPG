using Little_Choice_Based_RPG.Resources.Entities;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory.InventoryExtensions;
using Little_Choice_Based_RPG.Resources.Systems.InformationalSystems.Descriptor.DescriptorExtensions;
using Little_Choice_Based_RPG.Types;
using Little_Choice_Based_RPG.Types.DescriptorConditions;

namespace Little_Choice_Based_RPG.Resources.Systems.InformationalSystems.Descriptor
{
    public static class DescriptorProcessor
    {
        /// <summary> Create a new EntityStateDescriptor on the target object. Uses a CreateDescriptorStateList to define the conditions. </summary>
        public static void CreateDescriptor(PropertyContainer target, CreateDescriptorStateList conditions)
        {
            if (!target.Extensions.Contains("ItemContainer"))
                throw new ArgumentException($"The target {target} didn't contain an extension of the identifier \"ItemContainer\"! " +
                    $"The extensions list was {target.Extensions}.");

            //Give the target the ConditionalDescriptors extension if they don't already have it
            if (!target.Extensions.Contains("ConditionalDescriptors"))
                target.Extensions.AddExtension(new ConditionalDescriptors());

            ConditionalDescriptors currentDescriptors = (ConditionalDescriptors)target.Extensions.Get("ConditionalDescriptors");

            //Add the new condition to the target object.
            currentDescriptors.Add(new EntityStateDescriptor(conditions.Descriptor.PropertyName, (string) conditions.Descriptor.PropertyValue, 
                conditions.EntityStates, conditions.Priority));
        }


        /// <summary> Takes a target and the name of a descriptor to retrieve it. Considers conditional descriptors of the same identity before returning. 
        /// "Descriptor." may be omitted for the descriptorIdentity.
        public static string GetDescriptor(PropertyContainer targetContainer, string descriptorPartialIdentity)
        {
            string? currentDescriptor = null;
            string descriptorName = descriptorPartialIdentity.StartsWith("Descriptor.") ? descriptorPartialIdentity : $"Descriptor.{descriptorPartialIdentity}";

            //Try different descriptors and fallback to more generic descriptors until one is found. 
            while (currentDescriptor is null)
            {
                if (TryGetConditionalDescriptor(targetContainer, descriptorName, out currentDescriptor))
                    break;

                if (TryGetRegularDescriptor(targetContainer, descriptorName, out currentDescriptor))
                    break;

                //Check if the descriptor can fallback to a more generic one and try again if so.
                if (!TryDescriptorFallback(descriptorName, out descriptorName))
                    throw new Exception($"Tried to get a {descriptorName} descriptor for {targetContainer}, but couldn't find any valid ones! Even tried fallbacks!");
            }
            
            return currentDescriptor;
        }

        private static bool TryDescriptorFallback(string descriptorName, out string fallbackName)
        {
            //Fallback to default from current
            if (descriptorName.EndsWith(".Current"))
            {
                fallbackName = 
                    descriptorName.Remove(descriptorName.Length - ".Current".Length) + ".Default";

                return true;
            }

            //Fallback to generic.current from inspect
            if (descriptorName.Contains(".Inspect."))
            {
                //Turn into generic.current
                fallbackName = descriptorName.Remove(descriptorName.LastIndexOf(".Inspect.")) + ".Generic.Current";
                return true;
            }

            //No fallbacks were found.
            fallbackName = descriptorName;
            return false;
        }

        private static bool TryGetRegularDescriptor(PropertyContainer targetContainer, string descriptorName, out string? descriptor)
        {
            descriptor = null;

            if (targetContainer.Properties.HasExistingPropertyName(descriptorName))
                descriptor = (string)targetContainer.Properties.GetPropertyValue(descriptorName);

            return (descriptor is not null);
        }

        private static bool TryGetConditionalDescriptor(PropertyContainer targetContainer, string descriptorName, out string? concatenatedDescriptor)
        {
            concatenatedDescriptor = null;

            if (!targetContainer.Extensions.Contains("ConditionalDescriptors"))
                return false;
            /*throw new ArgumentException($"The target {targetContainer} didn't contain an extension of the identifier \"ConditionalDescriptors\"! " +
                $"The extensions list was {targetContainer.Extensions}.");*/

            if (!targetContainer.Extensions.Contains("ItemContainer"))
                return false;
            /*throw new ArgumentException($"The target {targetContainer} didn't contain an extension of the identifier \"ItemContainer\"! " +
                $"The extensions list was {targetContainer.Extensions}.");*/

            List<uint>? remainingIDs = null;
            List<uint>? totalIDs = new();
            string concatenatedValidDescriptors = "";
            List<IDescriptorCondition> relevantDescriptors = new List<IDescriptorCondition>();
            List<IDescriptorCondition> prioritisedDescriptors = new List<IDescriptorCondition>();

            ConditionalDescriptors storedDescriptors = (ConditionalDescriptors)targetContainer.Extensions.Get("ConditionalDescriptors");
            ItemContainer targetContents = (ItemContainer)targetContainer.Extensions.Get("ItemContainer");

            //Populate the list of remainingIDs
            foreach (var entity in targetContents.Inventory)
                totalIDs.Add((uint)entity.Properties.GetPropertyValue("ID"));

            remainingIDs = totalIDs;

            //Grab relevant descriptors
            foreach (var descriptor in storedDescriptors.Descriptors)
            {
                if (descriptor.DescriptorIdentity == descriptorName) //Descriptor describes the required descriptorName
                    if (descriptor.CheckConditionIsValid(targetContainer, descriptor)) //Descriptor currently matches its condition
                        relevantDescriptors.Add(descriptor);
            }
        
            //Prioritise the relevant descriptors
            prioritisedDescriptors = PrioritiseDescriptors(relevantDescriptors, remainingIDs, out remainingIDs);
        
            //Check if the target requests an additive or ordinary description.
            if (targetContainer.Properties.HasPropertyAndValue($"{descriptorName}.IsAdditive", true))
            {
                //Generate an additive description
                concatenatedValidDescriptors += GenerateAdditiveDescription(descriptorName, targetContainer, totalIDs, prioritisedDescriptors);
            }
            else //Generate an ordinary description
            {
                concatenatedValidDescriptors += ConcatenateDescriptors(prioritisedDescriptors) + "\n\n";
                concatenatedValidDescriptors += ConcatenateDescriptors(remainingIDs, targetContents);
            }
            
            concatenatedDescriptor = concatenatedValidDescriptors;
            
            return true;
        }

        private static string ConcatenateDescriptors(List<IDescriptorCondition> prioritisedDescriptors)
        {
            string concatenatedValidDescriptors = "";

            //Concatenate the prioritised descriptors.
            foreach (var descriptor in prioritisedDescriptors)
                concatenatedValidDescriptors += $"{descriptor.GetDescriptor()} ";

            return concatenatedValidDescriptors;
        }

        private static string ConcatenateDescriptors(List<uint> remainingIDs, ItemContainer targetContents)
        {
            string concatenatedValidDescriptors = "";

            //Add in the remaining descriptors.
            foreach (var entity in targetContents.Inventory)
                if (remainingIDs.Contains((uint)entity.Properties.GetPropertyValue("ID")))
                {
                    concatenatedValidDescriptors += $"{GetDescriptor(entity, "Generic.Current")} ";
                }

            return concatenatedValidDescriptors;
        }
        
        public static string GenerateAdditiveDescription(string descriptorName, PropertyContainer targetContainer, List<uint> remainingIDs,
            List<IDescriptorCondition>? prioritisedDescriptors = null)
        {
            if (!targetContainer.Extensions.Contains("ItemContainer"))
                throw new Exception($"This descriptors parent {targetContainer} didn't contain a reference for \"ItemContainer\" in its extensions list {targetContainer.Extensions}!");

            if (!targetContainer.Properties.HasPropertyAndValue($"{descriptorName}.IsAdditive", true))
                throw new Exception($"Was this descriptor additive? This descriptors parent {targetContainer} didn't contain a reference for \"{descriptorName}.Additive, true\" in its properties list {targetContainer.Properties}!");

            if (!targetContainer.Properties.HasExistingPropertyName($"{descriptorName}.Additive.Prefix"))
                throw new Exception($"This descriptors parent {targetContainer} didn't contain a reference for \"{descriptorName}.Prefix\" in its properties list {targetContainer.Properties}!");

            if (!targetContainer.Properties.HasExistingPropertyName($"{descriptorName}.Additive.Infix"))
                throw new Exception($"This descriptors parent {targetContainer} didn't contain a reference for \"{descriptorName}.Infix\" in its properties list {targetContainer.Properties}!");

            if (!targetContainer.Properties.HasExistingPropertyName($"{descriptorName}.Additive.Suffix"))
                throw new Exception($"This descriptors parent {targetContainer} didn't contain a reference for \"{descriptorName}.Suffix\" in its properties list {targetContainer.Properties}!");

            if (!targetContainer.Properties.HasExistingPropertyName($"{descriptorName}.Additive.Empty"))
                throw new Exception($"This descriptors parent {targetContainer} didn't contain a reference for \"{descriptorName}.Suffix\" in its properties list {targetContainer.Properties}!");


            string descriptorSuffix = (string) targetContainer.Properties.GetPropertyValue($"{descriptorName}.Additive.Suffix");
            string descriptorInfix = (string) targetContainer.Properties.GetPropertyValue($"{descriptorName}.Additive.Infix");
            string descriptorPrefix = (string)targetContainer.Properties.GetPropertyValue($"{descriptorName}.Additive.Prefix");
            string emptyDescriptor = (string)targetContainer.Properties.GetPropertyValue($"{descriptorName}.Additive.Empty");

            string concatenatedDescriptor = descriptorPrefix; //Starts with the prefix.
            ItemContainer parentItemContainer = (ItemContainer)targetContainer.Extensions.Get("ItemContainer");
            bool previousEntryWritten = false;

            //Set the empty descriptor, if the inventory is empty
            if (prioritisedDescriptors.Count == 0 && remainingIDs.Count == 0)
                concatenatedDescriptor += emptyDescriptor;

            //Add each prioritsied descriptor and remove its relevant remaining IDs.
            foreach (var descriptor in prioritisedDescriptors)
            {
                //Preceed by the infix unless it's the first entry.
                if (!previousEntryWritten)
                    concatenatedDescriptor += descriptorInfix;

                //Add the entry
                concatenatedDescriptor += descriptor.GetDescriptor();

                //Remove related IDs from remainingIDs
                foreach(var entityState in descriptor.RequiredEntityStates)
                    remainingIDs.Remove(entityState.EntityReferenceID);                

                previousEntryWritten = true;
            }

            //Add each objectname until the remaining IDs are emtpy. 
            foreach (var entity in parentItemContainer.Inventory)
                foreach (var id in remainingIDs)
                    if (id.Equals(entity.Properties.GetPropertyValue("ID")))
                    {
                        //Preceed by the infix unless it's the first entry.
                        if (!previousEntryWritten)
                            concatenatedDescriptor += descriptorInfix;

                        //Add the name and remove the ID from remainingIDs
                        concatenatedDescriptor += entity.Properties.GetPropertyValue("Name");
                        remainingIDs.Remove(id);

                        previousEntryWritten = true;
                    }

            //Add the final suffix
            concatenatedDescriptor += descriptorSuffix;

            return concatenatedDescriptor;
        }

        private static List<IDescriptorCondition> PrioritiseDescriptors(List<IDescriptorCondition> rawDescriptors, List<uint> targetIDsRemaining, out List<uint> remainingIDs)
        {
            uint previousPriority = 0;
            uint currentPriority;
            List<uint> higherPriorityObjectIDs = new();
            List<uint> previousSamePriorityObjectIDs = new();
            List<uint> currentObjectIDs = new();
            List<IDescriptorCondition> prioritisedDescriptors = new();

            List<IDescriptorCondition> sortedDescriptors = rawDescriptors.OrderBy(i => i.Priority).ToList();

            List<uint> allSuccessfulObjectIDs = new();

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

                    //Mark its IDs as described
                    foreach (uint entityID in currentObjectIDs)
                    {
                        if (!allSuccessfulObjectIDs.Contains(entityID)) //Grabs each succesful ID only once
                            allSuccessfulObjectIDs.Add(entityID);
                    }
                }

                //Always set previousPriority = currentPriority; then continue the loop.
                previousPriority = currentPriority;
            }


            //Figure out which IDs were overlooked.
            foreach (uint entityID in allSuccessfulObjectIDs)
                if (targetIDsRemaining.Contains(entityID)) //Ensure that nothing is removed unnecessarily!
                    targetIDsRemaining.Remove(entityID);

            //Return IDs that the priority system overlooked
            remainingIDs = targetIDsRemaining;

            return prioritisedDescriptors;
        }

    }
}
