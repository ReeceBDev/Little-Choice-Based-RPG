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
    public class ItemContainer : IExtension
    {
        public void Add(GameObject target)
        {
            ExtensionChanged?.Invoke(this, new ExtensionChangedArgs("ItemContainer.Added", target));
            Inventory.Add(target);
        }

        public void Remove(GameObject target)
        {
            ExtensionChanged?.Invoke(this, new ExtensionChangedArgs("ItemContainer.Removed", target));
            Inventory.Remove(target);
        }

        public event EventHandler<ExtensionChangedArgs> ExtensionChanged;
        public string UniqueIdentifier { get; init; } = "ItemContainer";
        public List<GameObject> Inventory { get; private set; } = new List<GameObject>();
    }
}
