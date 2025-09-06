using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PublicInteractionsSystems;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems;
using Little_Choice_Based_RPG.Types.TypedEventArgs.PropertyContainerEventArgs;
using Little_Choice_Based_RPG.Types.PropertySystem.Archive;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using Little_Choice_Based_RPG.Types.PropertySystem.Systems;

namespace Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Repair
{
    /// <summary> Allows repairs to occur on this object. Requires DamageLogic. </summary>
    internal sealed class RepairSystem : PropertySystem
    {
        protected override void OnObjectChanged(object sender, ObjectChangedEventArgs propertyChangedData)
        {
            IPropertyContainer sourceContainer = propertyChangedData.Source;
            PropertyStore sourceProperties = sourceContainer.Properties;

            // When to do with Damage.Broken
            if (propertyChangedData.Property == "Damage.Broken")
            {
                // When something repairable breaks
                if (propertyChangedData.Property == "Damage.Broken" && propertyChangedData.Change.Equals(true))
                {
                    if (sourceProperties.HasPropertyAndValue("Repairable.ByChoice", true))
                        InteractionRegistrar.TryAddPublicInteraction(sourceContainer, RepairDelegation.GetRepairChoice(sourceContainer, sourceProperties));
                }

                // When something repairable is repaired
                if (propertyChangedData.Property == "Damage.Broken" && propertyChangedData.Change.Equals(false))
                    InteractionRegistrar.TryRemovePublicInteraction(sourceContainer, RepairDelegation.GetRepairChoice(sourceContainer, sourceProperties));
            }
        }

        public override void InitialiseNewSubscriber(IPropertyContainer sourceContainer, PropertyStore sourceProperties)
        {
            // If the object is broken already
            if (sourceProperties.HasPropertyAndValue("Damage.Broken", true))
            {
                if (sourceProperties.HasPropertyAndValue("Repairable.ByChoice", true))
                    InteractionRegistrar.TryAddPublicInteraction(sourceContainer, RepairDelegation.GetRepairChoice(sourceContainer, sourceProperties));
            }

            // If the object is already repaired
            if (!sourceProperties.HasPropertyAndValue("Damage.Broken", true))
                RepairProcessor.SetRepairDescriptors(sourceContainer); //Ensure its descriptors are correct.
        }
    }
}
