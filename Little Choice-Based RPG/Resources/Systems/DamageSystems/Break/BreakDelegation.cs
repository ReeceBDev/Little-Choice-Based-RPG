using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Systems.InformationalSystems.Descriptor;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PublicInteractionsSystems;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.Interactions;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.SingleParameterDelegates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.InteractionUsingNothing;
using static Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.SingleParameterDelegates.InteractionUsingGameObject;

namespace Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Break
{
    public static class BreakDelegation
    {
        public static InteractionUsingGameObject GetBreakUsingNothing(PropertyContainer target, PropertyHandler sourceProperties)
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
                DescriptorProcessor.GetDescriptor(target, "Breakable.Interaction.Title"),
                DescriptorProcessor.GetDescriptor(target, "Breakable.Interaction.Invoking"),
                (GameObject)target
            );

            return breakUsingGameObject;
        }
    }
}
