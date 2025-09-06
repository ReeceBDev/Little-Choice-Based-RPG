using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PublicInteractionsSystems;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using Little_Choice_Based_RPG.Types.PropertySystem.Systems;
using Little_Choice_Based_RPG.Types.TypedEventArgs.PropertyContainerEventArgs;

namespace Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Break
{
    /// <summary> Allows objects to break. </summary>
    internal sealed class BreakSystem : PropertySystem
    {
        protected override void OnObjectChanged(object sender, ObjectChangedEventArgs propertyChangedData)
        {
            IPropertyContainer modifiedContainer = propertyChangedData.Source;
            PropertyStore modifiedProperties = modifiedContainer.Properties;

            // When to do with Damage.Broken
            if (propertyChangedData.Property == "Damage.Broken")
            {
                // When something breakable is repaired
                if (propertyChangedData.Property == "Damage.Broken" && propertyChangedData.Change.Equals(false))
                {
                    if (modifiedProperties.HasPropertyAndValue("Breakable.ByChoice", true))
                        InteractionRegistrar.TryAddPublicInteraction(modifiedContainer, BreakDelegation.GetBreakUsingNothing(modifiedContainer, modifiedProperties));
                }

                // When something breakable is broken, remove its choice
                if (propertyChangedData.Property == "Damage.Broken" && propertyChangedData.Change.Equals(true))
                {
                    InteractionRegistrar.TryRemovePublicInteraction(modifiedContainer, BreakDelegation.GetBreakUsingNothing(modifiedContainer, modifiedProperties));
                }
            }
        }

        public override void InitialiseNewSubscriber(IPropertyContainer sourceContainer, PropertyStore sourceProperties)
        {
            // If the object is not broken
            if (!sourceProperties.HasPropertyAndValue("Damage.Broken", true))
            {
                if (sourceProperties.HasPropertyAndValue("Breakable.ByChoice", true))
                    InteractionRegistrar.TryAddPublicInteraction(sourceContainer, BreakDelegation.GetBreakUsingNothing(sourceContainer, sourceProperties));
            }

            //If the object is already broken
            if (sourceProperties.HasPropertyAndValue("Damage.Broken", true))
                BreakLogic.SetBrokenDescriptors(sourceContainer); //Ensure its descriptors are correct.
        }
    }
}
