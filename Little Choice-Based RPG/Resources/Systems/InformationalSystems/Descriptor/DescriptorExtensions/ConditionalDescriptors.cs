using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.PropertyExtensions.PropertyExtensionEventArgs;
using Little_Choice_Based_RPG.Types.PropertyExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Little_Choice_Based_RPG.Types.RoomSpecificTypes;

namespace Little_Choice_Based_RPG.Resources.Systems.InformationalSystems.Descriptor.DescriptorExtensions
{
    /// <summary> Holds conditional descriptors. Each conditional descriptor is a possible descriptor that applies under a condition. </summary>
    public class ConditionalDescriptors : IPropertyExtension
    {
        public void Add(ConditionalDescriptor condition)
        {
            PropertyExtensionChanged?.Invoke(this, new PropertyExtensionChangedArgs("ConditionalDescriptor.Added", condition));
            Descriptors.Add(condition);
        }

        public void Remove(ConditionalDescriptor condition)
        {
            PropertyExtensionChanged?.Invoke(this, new PropertyExtensionChangedArgs("ConditionalDescriptor.Removed", condition));
            Descriptors.Remove(condition);
        }

        public event EventHandler<PropertyExtensionChangedArgs> PropertyExtensionChanged;
        public string UniqueIdentifier { get; init; } = "ConditionalDescriptors";
        public List<ConditionalDescriptor> Descriptors { get; private set; } = new List<ConditionalDescriptor>();
    }
}
