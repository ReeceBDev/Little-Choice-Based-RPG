using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Little_Choice_Based_RPG.Types.EntityProperties;

namespace Little_Choice_Based_RPG.Resources.Systems.DamageSystems
{
    public static class GlobalDamageProperties
    {
        static GlobalDamageProperties()
        {
            PropertyValidation.CreateValidProperty("Damage.Broken", PropertyType.Boolean); //Indicates when an object becomes broken. Used by BreakSystem and RepairSystem.
        }
    }
}
