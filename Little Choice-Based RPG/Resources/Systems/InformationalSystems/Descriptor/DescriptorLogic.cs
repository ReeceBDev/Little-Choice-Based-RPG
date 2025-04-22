using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory.InventoryExtensions;
using Little_Choice_Based_RPG.Resources.Systems.InformationalSystems.Descriptor.DescriptorExtensions;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PublicInteractionsSystems;
using Little_Choice_Based_RPG.Types;
using Little_Choice_Based_RPG.Types.DescriptorConditions;
using Little_Choice_Based_RPG.Types.EntityProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems.InformationalSystems.Descriptor
{
    public record struct CreateDescriptorArgs(uint EntityID, string PropertyName, object PropertyValue);
    public static class DescriptorLogic
    {
        /*
        /// <summary> Create a new EntityStateDescriptor on a target object. The dictionary must define Item ID, PropertyName and PropertyValue.</summary>
        public static void CreateDescriptor(PropertyContainer target, List<CreateDescriptorArgs> conditionArgs)
        {
            if (!target.Extensions.ContainsExtension("ItemContainer"))
                throw new ArgumentException($"The target {target} didn't contain an extension of the identifier \"ItemContainer\"! " +
                    $"The extensions list was {target.Extensions}.");

            if (!target.Extensions.ContainsExtension("ConditionalDescriptors"))
                throw new ArgumentException($"The target {target} didn't contain an extension of the identifier \"ConditionalDescriptors\"! " +
                    $"The extensions list was {target.Extensions}.");

            conditionArgs.OrderByDescending()

            while (conditionArgs.Count > 0)
            {
                uint currentID;
                foreach (var arg in conditionArgs)
                {
                    arg.EntityID;
                }
            }
        }
        */


        /// <summary> Takes a target and the name of a descriptor to retrieve it. Considers conditional descriptors of the same identity before returning. 
        /// "Descriptor." may be omitted for the descriptorIdentity.
        public static string GetDescriptor(PropertyContainer targetContainer, string descriptorIdentity)
        {
            string currentDescriptor;
            string descriptorName = descriptorIdentity.StartsWith("Descriptor.") ? descriptorIdentity : $"Descriptor.{descriptorIdentity}";

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

            List<uint>? remainingIDs = null;
            List<uint>? totalIDs = null;
            string concatenatedValidDescriptors = "";
            List<IDescriptorCondition> relevantDescriptors = new List<IDescriptorCondition>();
            List<IDescriptorCondition> prioritisedDescriptors = new List<IDescriptorCondition>();

            ConditionalDescriptors storedDescriptors = (ConditionalDescriptors) targetContainer.Extensions.GetExtension("ConditionalDescriptors");
            ItemContainer targetContents = (ItemContainer)targetContainer.Extensions.GetExtension("ItemContainer");

            //Populate the list of remainingIDs
            foreach (var entity in targetContents.Inventory)
                totalIDs.Add((uint)entity.Properties.GetPropertyValue("ID"));

            remainingIDs = totalIDs;

            //Grab relevant descriptors
            foreach (var descriptor in storedDescriptors.Descriptors)
            {
                if (descriptor.DescriptorIdentity == descriptorName) //Descriptor describes the required descriptorName
                    if (descriptor.CheckConditionIsValid()) //Descriptor currently matches its condition
                        relevantDescriptors.Add(descriptor);
            }

            //Prioritise the relevant descriptors
            prioritisedDescriptors = EntityStateLogic.PrioritiseDescriptors(relevantDescriptors, remainingIDs, out remainingIDs);

            //Check if the target requests an additive or ordinary description.
            if (targetContainer.Properties.HasPropertyAndValue($"{descriptorName}.Additive", true))
            {
                //Generate an additive description
                concatenatedValidDescriptors += GenerateAdditiveDescription(targetContainer, totalIDs, targetContents, prioritisedDescriptors);
            }
            else //Generate an ordinary description
            { 
                concatenatedValidDescriptors += ConcatenateDescriptors(prioritisedDescriptors);
                concatenatedValidDescriptors += ConcatenateDescriptors(remainingIDs, targetContents);
            }

            return concatenatedValidDescriptors;
        }

        private static string ConcatenateDescriptors(List<IDescriptorCondition> prioritisedDescriptors)
        {
            string concatenatedValidDescriptors = "";

            //Concatenate the prioritised descriptors.
            foreach (var descriptor in prioritisedDescriptors)
                concatenatedValidDescriptors += $"{descriptor.GetDescriptor} ";

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
            if (!targetContainer.Extensions.ContainsExtension("ItemContainer"))
                throw new Exception($"This descriptors parent {targetContainer} didn't contain a reference for \"ItemContainer\" in its extensions list {targetContainer.Extensions}!");

            if (!targetContainer.Properties.HasPropertyAndValue($"{descriptorName}.Additive", true))
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
            ItemContainer parentItemContainer = (ItemContainer)targetContainer.Extensions.GetExtension("ItemContainer");
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
    }
}
