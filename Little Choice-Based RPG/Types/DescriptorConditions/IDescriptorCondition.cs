using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.DescriptorConditions
{
    public interface IDescriptorCondition
    {
        public string GetDescriptor();
        public ConditionType ConditionIdentity { get; init; }
        public string DescriptorIdentity { get; init; }      
        public uint Priority { get; init; }
        public List<EntityState> RequiredEntityStates { get; init; }
    }
}
