using Little_Choice_Based_RPG.Managers.World;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Resources.Entities.Rooms;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;

namespace Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory
{
    internal static class InventoryPropertyValidation
    {
        public static void ValidateInventoryDescriptors(GameObject target)
        {
            //Check weight against global maximum strength value
            decimal currentWeight = (decimal)target.Properties.GetPropertyValue("WeightInKG");

            if (currentWeight > GlobalMaximumValues.MaximumStrengthLimit)
                return; //Inventory descriptors aren't required since it falls over the maximum strength limit.

            //Ensure pickup and putdown descriptors exist
            if (!target.Properties.HasExistingPropertyName("Descriptor.InventorySystem.Interaction.Pickup.Title"))
                throw new Exception($"The GameObject or derivative {target}, weighed less than the maximum strength limit, but didn't have a Pickup Title descriptor!");

            if (!target.Properties.HasExistingPropertyName("Descriptor.InventorySystem.Interaction.Pickup.Invoking"))
                throw new Exception($"The GameObject or derivative {target}, weighed less than the maximum strength limit, but didn't have a Pickup Invoking descriptor!");

            if (!target.Properties.HasExistingPropertyName("Descriptor.InventorySystem.Interaction.Drop.Title"))
                throw new Exception($"The GameObject or derivative {target}, weighed less than the maximum strength limit, but didn't have a Pickup Title descriptor!");

            if (!target.Properties.HasExistingPropertyName("Descriptor.InventorySystem.Interaction.Drop.Invoking"))
                throw new Exception($"The GameObject or derivative {target}, weighed less than the maximum strength limit, but didn't have a Pickup Invoking descriptor!");
        }

        public static void ValidateOpenInventoryDescriptors(IPropertyContainer target)
        {
            if ((target is not Room) && (target is not Player))
            {
                if (!target.Properties.HasExistingPropertyName("Descriptor.InventorySystem.Interaction.Open.Title"))
                    throw new Exception($"The GameObject or derivative {target}, was an openable container, but didn't have an Open Title descriptor!");

                if (!target.Properties.HasExistingPropertyName("Descriptor.InventorySystem.Interaction.Open.Invoking"))
                    throw new Exception($"The GameObject or derivative {target}, was an openable container, but didn't have the Open Invoking descriptor!");
            }
        }
    }
}
