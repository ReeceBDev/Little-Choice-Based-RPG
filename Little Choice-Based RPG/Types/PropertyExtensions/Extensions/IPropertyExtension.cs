using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.PropertyExtensions.PropertyExtensionEventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.PropertyExtensions.Extensions
{
    public interface IPropertyExtension
    {
        public string UniqueIdentifier { get; init; }
        public event EventHandler<PropertyExtensionChangedArgs> PropertyExtensionChanged;
    }
}
