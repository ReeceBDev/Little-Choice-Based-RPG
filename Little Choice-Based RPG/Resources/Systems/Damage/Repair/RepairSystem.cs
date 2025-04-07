using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Systems.Damage.Repair;
using Little_Choice_Based_RPG.Resources.Systems.SystemEventBus;
using Little_Choice_Based_RPG.Types.EntityProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems.Damage.Repair
{
    public class RepairSystem : PropertyLogic
    {
        private GameObject parentObject;
        static RepairSystem()
        {
            //RepairSystem logic
            PropertyValidation.CreateValidProperty("Repair.IsRepairable", PropertyType.Boolean); //Activates all this class and all of these properties when true :)
            PropertyValidation.CreateValidProperty("Repair.IsRepairableByChoice", PropertyType.Boolean); //Lets players choose to repair it by choice.
            PropertyValidation.CreateValidProperty("Repair.MustRequireTool", PropertyType.Boolean); //Requires tools if true, but may be repaired by hand if not.
            PropertyValidation.CreateValidProperty("Repair.RequiredRepairToolType", PropertyType.String); //Must match the type on the repair tool (subject to change upon fleshing out this system.)
            PropertyValidation.CreateValidProperty("Repair.RepairToolType", PropertyType.String); //Must match the type on the repair tool (subject to change upon fleshing out this system.)

            //Descriptors
            PropertyValidation.CreateValidProperty("Descriptor.Repair.Repairing", PropertyType.String); //Describes how it looks when it gets repaired.
            PropertyValidation.CreateValidProperty("Descriptor.Repair.Choice.Interact", PropertyType.String); //Describes the interact option presented to the player.
            PropertyValidation.CreateValidProperty("Descriptor.Repair.Choice.Repairing", PropertyType.String); //Describes the action of repairing it when a player uses the Repair() choice.
        }

        /// <summary> Allows repairs to occur on this object. Requires DamageCommon. </summary>
        public RepairSystem(GameObject instantiatingObject, SystemSubscriptionEventBus systemSubscriptionEventBusReference) : base(systemSubscriptionEventBusReference)
        {
            DamageCommon damageCommonInstantisation = DamageCommon.Instance;

            parentObject = instantiatingObject;
        }

        /// <summary> Sets IsBroken to false, from the BreakSystem. </summary>
        public void Repair(IUserInterface mutexHolder, GameObject? repairTool = null)
        {
            //Guard clauses for the values in use.
            if (!parentObject.Properties.HasProperty("IsRepairable", true))
                throw new Exception("This object is not repairable! Tried to repair an object where there is no EntityProperty of IsRepairable = true.");

            if (!parentObject.Properties.HasExistingPropertyName("Descriptor.Repairing"))
                throw new Exception("This object has no repairing description! Tried to repair an object where there is no EntityProperty of Descriptor.Repairing.");

            //Check if a repair tool is required.
            if (!parentObject.Properties.HasProperty("Repair.MustRequireTool", true))
            {
                //When no repair tool is required, initate repair.
                InitiateRepair();
                return;
            }

            //When a repair tool is required:
            if (repairTool == null)
                return; // Error, can't repair without a tool!

            //If repair tool doesn't match
            if (!repairTool.Properties.HasProperty("Repair.RepairToolType", parentObject.Properties.GetPropertyValue("Repair.RequiredRepairToolType")))
                return; //Error, repairtooltype doesn't match the requiredrepairtool on this object

            //Repair if repair tool is correct :)
            InitiateRepair();
        }

        private void InitiateRepair()
        {
            //Main repairing logic
            parentObject.Properties.UpsertProperty("IsBroken", false); //Property found in DamageCommon

            //Set the generic and inspect descriptors back to default.
            parentObject.Properties.UpdateProperty("Descriptor.Generic.Current", parentObject.Properties.GetPropertyValue("Descriptor.Generic.Default"));
            parentObject.Properties.UpdateProperty("Descriptor.Inspect.Current", parentObject.Properties.GetPropertyValue("Descriptor.Generic.Default"));
        }
    }
}
