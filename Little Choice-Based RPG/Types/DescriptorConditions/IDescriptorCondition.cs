using Little_Choice_Based_RPG.Types.PropertySystem.Entities;

namespace Little_Choice_Based_RPG.Types.DescriptorConditions
{
    internal interface IDescriptorCondition
    {
        public string GetDescriptor();
        public bool CheckConditionIsValid(IPropertyContainer targetEntity, IDescriptorCondition targetStateDescriptor);
        public ConditionType ConditionIdentity { get; init; }
        public string DescriptorIdentity { get; init; }      
        public uint Priority { get; init; }
        public List<EntityState> RequiredEntityStates { get; init; }
    }
}
