using Little_Choice_Based_RPG.Types.PropertyExtensions.PropertyExtensionEventArgs;
using Little_Choice_Based_RPG.Types.PropertyExtensions;
using Little_Choice_Based_RPG.Types.DescriptorConditions;

namespace Little_Choice_Based_RPG.Resources.Systems.InformationalSystems.Descriptor.DescriptorExtensions
{
    /// <summary> Holds conditional descriptors. Each conditional descriptor is a possible descriptor that applies under a condition. </summary>
    public class ConditionalDescriptors : IPropertyExtension
    {
        public void Add(IDescriptorCondition condition)
        {
            PropertyExtensionChanged?.Invoke(this, new PropertyExtensionChangedArgs("ConditionalDescriptor.Added", condition));
            Descriptors.Add(condition);
        }

        public void Remove(IDescriptorCondition condition)
        {
            PropertyExtensionChanged?.Invoke(this, new PropertyExtensionChangedArgs("ConditionalDescriptor.Removed", condition));
            Descriptors.Remove(condition);
        }

        public event EventHandler<PropertyExtensionChangedArgs> PropertyExtensionChanged;
        public string UniqueIdentifier { get; init; } = "ConditionalDescriptors";
        public List<IDescriptorCondition> Descriptors { get; private set; } = new List<IDescriptorCondition>();
    }
}
