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
    public class RepairSystem : IRepairable
    {
        private GameObject parentObject;
        static RepairSystem()
        {
            //RepairSystem logic
            PropertyValidation.CreateValidProperty("IsRepairable", PropertyType.Boolean); //Activates all this class and all of these properties when true :)
            PropertyValidation.CreateValidProperty("IsRepairableByChoice", PropertyType.Boolean); //Lets players choose to repair it by choice.
            PropertyValidation.CreateValidProperty("Repair.MustRequireTool", PropertyType.Boolean); //Requires tools if true, but may be repaired by hand if not.
            PropertyValidation.CreateValidProperty("Repair.RepairToolType", PropertyType.String); //Must match the type on the repair tool (subject to change upon fleshing out this system.)

            //Descriptors
            PropertyValidation.CreateValidProperty("Descriptor.Repairing", PropertyType.String); //Describes how it looks when it gets repaired.
            PropertyValidation.CreateValidProperty("Descriptor.Choice.Repair.Interact", PropertyType.String); //Describes the interact option presented to the player.
            PropertyValidation.CreateValidProperty("Descriptor.Choice.Repairing", PropertyType.String); //Describes the action of repairing it when a player uses the Repair() choice.
        }

        /// <summary> Allows repairs to occur on this object. Requires DamageCommon. </summary>
        public RepairSystem(GameObject instantiatingObject)
        {
            //Enforces IRepairable on the instantiating class
            if (!(instantiatingObject is IRepairable)) 
                throw new Exception($"{instantiatingObject.GetType()} does not implement IRepairable!");

            DamageCommon damageCommonInstantisation = DamageCommon.Instance;

            parentObject = instantiatingObject;
        }

        /// <summary> Sets IsBroken to false, from the BreakSystem. </summary>
        public void Repair(GameObject? repairTool = null)
        {
            //Guard clauses for the values in use.
            if (!parentObject.entityProperties.HasPropertyAndValue("IsRepairable", true))
                throw new Exception("This object is not repairable! Tried to repair an object where there is no EntityProperty of IsRepairable = true.");

            if (!parentObject.entityProperties.HasProperty("Descriptor.Repairing"))
                throw new Exception("This object has no repairing description! Tried to repair an object where there is no EntityProperty of Descriptor.Repairing.");

            //Main repairing logic.
            parentObject.entityProperties.UpsertProperty("IsBroken", false); //Property found in DamageCommon

            //Set the generic and insepct descriptors back to default.
            parentObject.entityProperties.UpdateProperty("Descriptor.Generic.Current", parentObject.entityProperties.GetPropertyValue("Descriptor.Generic.Default"));
            parentObject.entityProperties.UpdateProperty("Descriptor.Inspect.Current", parentObject.entityProperties.GetPropertyValue("Descriptor.Generic.Default"));
        }
    }
}
