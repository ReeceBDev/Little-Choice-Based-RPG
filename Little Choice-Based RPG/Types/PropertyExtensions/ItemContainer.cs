using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.EntityProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.Extensions
{
    public class ItemContainer : IExtension
    {
        public void Remove(GameObject target) => Inventory.Remove(target);
        public void Add(GameObject target) => Inventory.Add(target);

        public string UniqueIdentifier { get; init; } = "Inventory";
        public List<GameObject> Inventory { get; private set; } = new List<GameObject>();
    }
}
