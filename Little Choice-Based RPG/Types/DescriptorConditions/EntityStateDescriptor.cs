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
    public class EntityStateDescriptor(string setDescriptorIdentity, string setDescriptor, PropertyContainer setParentReference, List < EntityState> setRequiredEntityStates, uint setPriority = 6)
        : IDescriptorCondition
    {
        public string GetDescriptor() => Descriptor;

        public ConditionType ConditionIdentity { get; init; } = ConditionType.RequireEntityStates;
        public string DescriptorIdentity { get; init; } = setDescriptorIdentity;
        public string Descriptor { get; init; } = setDescriptor;
        public uint Priority { get; init; } = setPriority;
        public List<EntityState> RequiredEntityStates { get; init; } = setRequiredEntityStates;
        public PropertyContainer ParentReference { get; init; } = setParentReference;
    }
}
