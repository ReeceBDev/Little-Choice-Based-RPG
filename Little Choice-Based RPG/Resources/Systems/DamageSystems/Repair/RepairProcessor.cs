using Little_Choice_Based_RPG.Managers.PlayerControl;
using Little_Choice_Based_RPG.Resources.Entities;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Systems.InformationalSystems.Descriptor;

namespace Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Repair
{
    public static class RepairProcessor
    {
        /// <summary> Sets Damage.Broken to false, from the BreakSystem. </summary>
        public static void InvokeRepair(PlayerController mutexHolder, PropertyContainer target) => InvokeRepair(mutexHolder, target, null);

        /// <summary> Sets Damage.Broken to false, from the BreakSystem. </summary>
        public static void InvokeRepair(PlayerController mutexHolder, PropertyContainer target, GameObject? repairTool = null)
        {
            //Guard clauses for the values in use.
            if (!target.Properties.HasPropertyAndValue("Damage.Broken", true))
                throw new Exception("This object is not broken! Tried to repair an object where there is no EntityProperty of Damage.Broken = true.");

            //Check if a repair tool is required.
            if (!target.Properties.HasPropertyAndValue("Repairable.RequiresTool", true))
            {
                //When no repair tool is required, initate repair.
                Repair(target);
                return;
            }

            //When a repair tool is required:
            if (repairTool == null)
                return; // Error, can't repair without a tool!

            //If repair tool doesn't match
            if (!repairTool.Properties.HasPropertyAndValue("Repairable.ToolType", target.Properties.GetPropertyValue("Repairable.RequiredToolType")))
                return; //Error, repairtooltype doesn't match the requiredrepairtool on this object

            //Repair if repair tool is correct :)
            Repair(target);
        }

        private static void Repair(PropertyContainer target)
        {
            //Main repairing logic
            target.Properties.UpsertProperty("Damage.Broken", false); //Property found in DamageLogic

            SetRepairDescriptors(target);
        }

        public static void SetRepairDescriptors(PropertyContainer target)
        {
            //Set the generic and inspect descriptors back to default.
            target.Properties.UpsertProperty("Descriptor.Generic.Current", DescriptorProcessor.GetDescriptor(target, "Generic.Default"));
            target.Properties.UpsertProperty("Descriptor.Inspect.Current", DescriptorProcessor.GetDescriptor(target, "Generic.Default"));
        }
    }
}
