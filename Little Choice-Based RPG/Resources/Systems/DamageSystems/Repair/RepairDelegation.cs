using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.SingleParameterDelegates;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using Little_Choice_Based_RPG.Types.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.InteractionUsingNothing;
using static Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.SingleParameterDelegates.InteractionUsingGameObject;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PublicInteractionsSystems;
using Little_Choice_Based_RPG.Resources.Systems.InformationalSystems.Descriptor;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.DoubleParameterDelegates;
using static Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.DoubleParameterDelegates.InteractionUsingTwoGameObjects;

namespace Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Repair
{
    public static class RepairDelegation
    {
        public static IInvokableInteraction GetRepairChoice(PropertyContainer sourceContainer, PropertyHandler sourceProperties)
        {
            IInvokableInteraction? newInteraction = null;

            // If the repair requires no tool, give an interaction using nothing.
            if (!(sourceProperties.HasExistingPropertyName("Repairable.RequiredToolType")))
                newInteraction = InitialiseRepairUsingNothing(sourceContainer, sourceProperties);

            // If the repair require a tool, give an interaction requiring a gameobject of the given repairtooltype.
            if (sourceProperties.HasExistingPropertyName("Repairable.RequiredToolType"))
                newInteraction = InitialiseRepairUsingTool(sourceContainer, sourceProperties);

            return newInteraction ?? throw new Exception($"No repair interaction was chosen as valid, but one was requested for {sourceContainer}!");
        }

        public static InteractionUsingGameObject InitialiseRepairUsingNothing(PropertyContainer target, PropertyHandler sourceProperties)
        {
            //Guard clauses for interaction descriptors
            if (!sourceProperties.HasExistingPropertyName("Descriptor.Repair.Interaction.Title"))
                throw new Exception($"{target} of ID {sourceProperties.GetPropertyValue("ID")} has no property of Descriptor.Repair.Interaction.Title!");

            if (!sourceProperties.HasExistingPropertyName("Descriptor.Repair.Interaction.Invoking"))
                throw new Exception($"{target} of ID {sourceProperties.GetPropertyValue("ID")} has no property of Descriptor.Repair.Interaction.Invoking!");


            InteractionUsingGameObjectDelegate repairUsingGameObjectDelegate = RepairProcessor.InvokeRepair;

            InteractionUsingGameObject repairUsingGameObject = new InteractionUsingGameObject(
                repairUsingGameObjectDelegate,
                DescriptorProcessor.GetDescriptor(target, "Repair.Interaction.Title"),
                DescriptorProcessor.GetDescriptor(target, "Repair.Interaction.Invoking"),
                (GameObject) target
            );

            return repairUsingGameObject;
        }

        public static InteractionUsingTwoGameObjects InitialiseRepairUsingTool(PropertyContainer targetRepairee, PropertyHandler sourceProperties)
        {
            //Guard clauses for interaction descriptors
            if (!sourceProperties.HasExistingPropertyName("Descriptor.Repair.Interaction.Title"))
                throw new Exception($"{targetRepairee} of ID {sourceProperties.GetPropertyValue("ID")} has no property of Descriptor.Repair.Interaction.Title!");

            if (!sourceProperties.HasExistingPropertyName("Descriptor.Repair.Interaction.Invoking"))
                throw new Exception($"{targetRepairee} of ID {sourceProperties.GetPropertyValue("ID")} has no property of Descriptor.Repair.Interaction.Invoking!");


            InteractUsingTwoGameObjectsDelegate repairUsingToolDelegate = RepairProcessor.InvokeRepair;

            InteractionUsingTwoGameObjects repairUsingTool = new InteractionUsingTwoGameObjects(
                repairUsingToolDelegate,
                DescriptorProcessor.GetDescriptor(targetRepairee, "Repair.Interaction.Title"),
                DescriptorProcessor.GetDescriptor(targetRepairee, "Repair.Interaction.Invoking"),
                (GameObject) targetRepairee,
                $"Select a repair tool which is a {(string)sourceProperties.GetPropertyValue("Repairable.RequiredToolType")}.",
                new List<EntityProperty>([new EntityProperty("Repairable.ToolType", (string)sourceProperties.GetPropertyValue("Repairable.RequiredToolType"))])
            );

            return repairUsingTool;
        }
    }
}
