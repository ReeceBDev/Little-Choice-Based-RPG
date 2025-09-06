using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Systems.InformationalSystems.Descriptor;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.SingleParameterDelegates;
using Little_Choice_Based_RPG.Types.PropertySystem.Archive;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using static Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.SingleParameterDelegates.InteractionUsingGameObject;

namespace Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Break
{
    internal static class BreakDelegation
    {
        public static InteractionUsingGameObject GetBreakUsingNothing(IPropertyContainer target, PropertyStore sourceProperties)
        {
            //Guard clauses for interaction descriptors
            if (!sourceProperties.HasExistingPropertyName("Descriptor.Breakable.Interaction.Title"))
                throw new Exception($"{target} of ID {sourceProperties.GetPropertyValue("ID")} has no property of Descriptor.Breakable.Interaction.Title!");

            if (!sourceProperties.HasExistingPropertyName("Descriptor.Breakable.Interaction.Invoking"))
                throw new Exception($"{target} of ID {sourceProperties.GetPropertyValue("ID")} has no property of Descriptor.Breakable.Interaction.Invoking!");

            //Initialise the Interaction for Break()
            InteractionUsingGameObjectDelegate breakUsingGameObjectDelegate = BreakLogic.Break;

            InteractionUsingGameObject breakUsingGameObject = new InteractionUsingGameObject(
                breakUsingGameObjectDelegate,
                (uint)sourceProperties.GetPropertyValue("ID"),
                DescriptorProcessor.GetDescriptor(target, "Breakable.Interaction.Title"),
                DescriptorProcessor.GetDescriptor(target, "Breakable.Interaction.Invoking"),
                (GameObject)target
            );

            return breakUsingGameObject;
        }
    }
}
