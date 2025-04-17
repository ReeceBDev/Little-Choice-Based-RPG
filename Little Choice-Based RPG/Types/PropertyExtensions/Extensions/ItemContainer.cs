using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.PropertyExtensions.ExtensionEventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.PropertyExtensions.Extensions
{
    public class ItemContainer : IPropertyExtension
    {
        public void Add(GameObject target)
        {
            PropertyExtensionChanged?.Invoke(this, new PropertyExtensionChangedArgs("ItemContainer.Added", target));
            Inventory.Add(target);
        }

        public void Remove(GameObject target)
        {
            PropertyExtensionChanged?.Invoke(this, new PropertyExtensionChangedArgs("ItemContainer.Removed", target));
            Inventory.Remove(target);
        }

        public event EventHandler<PropertyExtensionChangedArgs> PropertyExtensionChanged;
        public string UniqueIdentifier { get; init; } = "ItemContainer";
        public List<GameObject> Inventory { get; private set; } = new List<GameObject>();
    }
}
