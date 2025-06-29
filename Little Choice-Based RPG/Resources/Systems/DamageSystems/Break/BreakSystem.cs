﻿using Little_Choice_Based_RPG.Resources.Entities;
using Little_Choice_Based_RPG.Resources.PropertyContainerEventArgs;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PublicInteractionsSystems.PublicInteractionsExtensions;
using Little_Choice_Based_RPG.Types.EntityProperties;

namespace Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Break
{
    /// <summary> Allows objects to break. </summary>
    public class BreakSystem : PropertyLogic
    {
        static BreakSystem()
        {
            //BreakSystem logic
            PropertyValidation.CreateValidProperty("Breakable.ByChoice", PropertyType.Boolean); //Lets players choose to break it by choice. 

            //Descriptors
            PropertyValidation.CreateValidProperty("Descriptor.Breakable.Interaction.Title", PropertyType.String); //Describes the interact option presented to the player.
            PropertyValidation.CreateValidProperty("Descriptor.Breakable.Interaction.Invoking", PropertyType.String); //Describes the action of breaking it when a player uses the Break() choice.
            PropertyValidation.CreateValidProperty("Descriptor.Generic.Broken", PropertyType.String); //Broken at a distance
            PropertyValidation.CreateValidProperty("Descriptor.Inspect.Broken", PropertyType.String); //A closer look
        }

        protected override void OnObjectChanged(object sender, ObjectChangedEventArgs propertyChangedData)
        {
            PropertyContainer modifiedContainer = propertyChangedData.Source;
            PropertyHandler modifiedProperties = modifiedContainer.Properties;

            // When to do with Damage.Broken
            if (propertyChangedData.Property == "Damage.Broken")
            {
                // When something breakable is repaired
                if (propertyChangedData.Property == "Damage.Broken" && propertyChangedData.Change.Equals(false))
                {
                    if (modifiedProperties.HasPropertyAndValue("Breakable.ByChoice", true))
                        PublicInteractions.TryAddPublicInteraction(modifiedContainer, BreakDelegation.GetBreakUsingNothing(modifiedContainer, modifiedProperties));
                }

                // When something breakable is broken, remove its choice
                if (propertyChangedData.Property == "Damage.Broken" && propertyChangedData.Change.Equals(true))
                {
                    PublicInteractions.TryRemovePublicInteraction(modifiedContainer, BreakDelegation.GetBreakUsingNothing(modifiedContainer, modifiedProperties));
                }
            }
        }

        protected override void InitialiseNewSubscriber(PropertyContainer sourceContainer, PropertyHandler sourceProperties)
        {
            // If the object is not broken
            if (!sourceProperties.HasPropertyAndValue("Damage.Broken", true))
            {
                if (sourceProperties.HasPropertyAndValue("Breakable.ByChoice", true))
                    PublicInteractions.TryAddPublicInteraction(sourceContainer, BreakDelegation.GetBreakUsingNothing(sourceContainer, sourceProperties));
            }

            //If the object is already broken
            if (sourceProperties.HasPropertyAndValue("Damage.Broken", true))
                BreakLogic.SetBrokenDescriptors(sourceContainer); //Ensure its descriptors are correct.
        }
    }
}
