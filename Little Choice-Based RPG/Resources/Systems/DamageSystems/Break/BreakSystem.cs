using Little_Choice_Based_RPG.Managers.Player_Manager;
using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.PropertyContainerEventArgs;
using Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Repair;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PublicInteractionsSystems;
using Little_Choice_Based_RPG.Resources.Systems.SystemEventBus;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.InteractionUsingNothing;

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
                        PublicInteractionsLogic.AddPublicInteraction(modifiedContainer, BreakDelegation.GetBreakUsingNothing(modifiedContainer, modifiedProperties));
                }

                // When something breakable is broken, remove its choice
                if (propertyChangedData.Property == "Damage.Broken" && propertyChangedData.Change.Equals(true))
                {
                    PublicInteractionsLogic.TryRemovePublicInteraction(modifiedContainer, BreakDelegation.GetBreakUsingNothing(modifiedContainer, modifiedProperties));
                }
            }
        }

        protected override void InitialiseNewSubscriber(PropertyContainer sourceContainer, PropertyHandler sourceProperties)
        {
            // If the object is not broken
            if (!sourceProperties.HasPropertyAndValue("Damage.Broken", true))
            {
                if (sourceProperties.HasPropertyAndValue("Breakable.ByChoice", true))
                    PublicInteractionsLogic.AddPublicInteraction(sourceContainer, BreakDelegation.GetBreakUsingNothing(sourceContainer, sourceProperties));
            }

            //If the object is already broken
            if (sourceProperties.HasPropertyAndValue("Damage.Broken", true))
                BreakLogic.SetBrokenDescriptors(sourceContainer); //Ensure its descriptors are correct.
        }
    }
}
