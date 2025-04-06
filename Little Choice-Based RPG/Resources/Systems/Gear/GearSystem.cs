using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.EntityProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems.Gear
{
    internal class GearSystem
    {
        static GearSystem()
        {
            //Component
            PropertyValidation.CreateValidProperty("Component.GearSlots", PropertyType.Boolean);

            PropertyValidation.CreateValidProperty("HasGearSlots", PropertyType.Boolean);
            PropertyValidation.CreateValidProperty("Gear.Slot.Helmet.ID", PropertyType.UInt32);
        }
    }
}
