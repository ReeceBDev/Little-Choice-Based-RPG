using Little_Choice_Based_RPG.Resources.Entities;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory.InventoryExtensions;

namespace Little_Choice_Based_RPG.Types.DescriptorConditions
{
    internal class EntityStateDescriptor(string setDescriptorIdentity, string setDescriptor, List < EntityState> setRequiredEntityStates, uint setPriority = 6)
        : IDescriptorCondition
    {
        public string GetDescriptor() => Descriptor;

        /// <summary> Returns whether the existing entity states match the conditions' required entity states </summary>
        public bool CheckConditionIsValid(PropertyContainer targetEntity, IDescriptorCondition targetStateDescriptor)
        {
            if (!targetEntity.Extensions.Contains("ItemContainer"))
                throw new Exception($"This descriptors parent {targetEntity} didn't contain a reference for \"ItemContainer\" in its extensions list {targetEntity.Extensions}!");

            List<uint> validEntityStates = new(); //List of EntityID
            ItemContainer parentItemContainer = (ItemContainer)targetEntity.Extensions.Get("ItemContainer");

            foreach (EntityState requiredEntityState in targetStateDescriptor.RequiredEntityStates)
            {
                //test the ID for all the objects in the room
                foreach (GameObject entity in parentItemContainer.Inventory)
                {
                    uint entityID = (uint)entity.Properties.GetPropertyValue("ID");

                    //when an object exists, test if it matches state
                    if (entityID == requiredEntityState.EntityReferenceID)
                    {
                        if (EntityStateLogic.CheckEntityStateIsValid(requiredEntityState, entity))
                            validEntityStates.Add(entityID);
                    }
                }
            }

            if (validEntityStates.Count > targetStateDescriptor.RequiredEntityStates.Count)
                throw new Exception("Critical error during Room Descriptor assessment: There were more valid entity states than the maximum possible number of required states. Something went seriously wrong! The objects might have misconfigured IDs, check if the IDs are unique! Make sure there are no duplicate item additions in the room, as they have to have been instantiated in the factory uniquely! Make sure all entities were instantiated with a factory, not directly onto their type.");

            //if all the objects in a condition are valid, return true
            return validEntityStates.Count == targetStateDescriptor.RequiredEntityStates.Count;
        }

        public ConditionType ConditionIdentity { get; init; } = ConditionType.RequireEntityStates;
        public string DescriptorIdentity { get; init; } = setDescriptorIdentity;
        public string Descriptor { get; init; } = setDescriptor;
        public uint Priority { get; init; } = setPriority;
        public List<EntityState> RequiredEntityStates { get; init; } = setRequiredEntityStates;
    }
}
