using Little_Choice_Based_RPG.Managers.Player_Manager;
using Little_Choice_Based_RPG.Resources.Systems.InformationalSystems.Descriptor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Break
{
    public static class BreakLogic
    {
        /// <summary> Sets Damage.Broken to true. </summary>
        public static void Break(PlayerController mutexHolder, PropertyContainer target)
        {
            //Guard clauses for the values in use.
            if (target.Properties.HasPropertyAndValue("Damage.Broken", true))
                throw new Exception("This object is already broken! Tried to break an object where there is already an EntityProperty of Damage.Broken = true.");

            if (!target.Properties.HasExistingPropertyName("Descriptor.Generic.Broken"))
                throw new Exception("This object has no broken description! Tried to break an object where there is no EntityProperty of Descriptor.Generic.Broken.");

            target.Properties.UpsertProperty("Damage.Broken", true); //Property found in DamageLogic

            SetBrokenDescriptors(target);
        }

        public static void SetBrokenDescriptors(PropertyContainer target)
        {
            //Set generic descriptor.
            target.Properties.UpsertProperty("Descriptor.Generic.Current", DescriptorProcessor.GetDescriptor(target, "Generic.Broken"));

            //Set inspect descriptor or default to generic.
            if (!target.Properties.HasExistingPropertyName("Descriptor.Inspect.Broken"))
                target.Properties.UpsertProperty("Descriptor.Inspect.Current", DescriptorProcessor.GetDescriptor(target, "Inspect.Broken"));
            else
                target.Properties.UpsertProperty("Descriptor.Inspect.Current", DescriptorProcessor.GetDescriptor(target, "Generic.Broken"));
        }
    }
}
