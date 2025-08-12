using Little_Choice_Based_RPG.Types.EntityProperties;

namespace Little_Choice_Based_RPG.Resources.Systems.DamageSystems
{
    internal static class GlobalDamageProperties
    {
        static GlobalDamageProperties()
        {
            PropertyValidation.CreateValidProperty("Damage.Broken", PropertyType.Boolean); //Indicates when an object becomes broken. Used by BreakSystem and RepairSystem.
            PropertyValidation.CreateValidProperty("Damage.Destroyed", PropertyType.Boolean); //Indicates whether an object is completely destroyed. Upon being destroyed, it should cease all functionality irrepairably, and possibly be deleted, and turned into a wastage object in its place, such as ash!
        }
    }
}
