using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Systems.Damage.Repair;
using Little_Choice_Based_RPG.Types.EntityProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems.Damage.Repair
{
    internal class RepairSystem : IRepairable
    {
        static RepairSystem()
        {
            //RepairSystem logic
            PropertyValidation.CreateValidProperty("IsRepairable", PropertyType.Boolean); //Activates all this class and all of these properties when true :)
            PropertyValidation.CreateValidProperty("IsRepairableByChoice", PropertyType.Boolean); //Lets players choose to repair it by choice.

            //Descriptors
            PropertyValidation.CreateValidProperty("Descriptor.Repairing", PropertyType.String); //Describes how it looks when it gets repaired.
            PropertyValidation.CreateValidProperty("Descriptor.Choice.Repair", PropertyType.String); //Describes the action of repairing it when a player uses the Repair() choice.
        }

        public void Repair(PropertyHandler sourcePropertyHandler, GameObject withRepairAsset)
        {

        }
    }
}
