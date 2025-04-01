using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Little_Choice_Based_RPG.Types.EntityProperties;

namespace Little_Choice_Based_RPG.Resources.Systems.Damage
{
    public sealed class DamageCommon
    {
        private static readonly DamageCommon singletonInstance = new DamageCommon();
        private DamageCommon()
        {
            PropertyValidation.CreateValidProperty("IsBroken", PropertyType.Boolean); //Indicates when an object becomes broken. Used by BreakSystem and RepairSystem.
        }

        public static DamageCommon Instance
        { 
            get
            {
                return singletonInstance;
            }
        }
    }
}
