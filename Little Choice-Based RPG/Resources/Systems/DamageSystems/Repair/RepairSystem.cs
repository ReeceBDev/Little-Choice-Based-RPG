using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Resources.PropertyContainerEventArgs;
using Little_Choice_Based_RPG.Resources.Entities;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PublicInteractionsSystems.PublicInteractionsExtensions;

namespace Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Repair
{
    /// <summary> Allows repairs to occur on this object. Requires DamageLogic. </summary>
    public class RepairSystem : PropertyLogic
    {
        static RepairSystem()
        {
            //RepairSystem logic
            PropertyValidation.CreateValidProperty("Repairable.ByChoice", PropertyType.Boolean); //Lets players choose to repair it by choice.
            PropertyValidation.CreateValidProperty("Repairable.RequiresTool", PropertyType.Boolean); //Requires tools if true, but may be repaired by hand if not.
            PropertyValidation.CreateValidProperty("Repairable.RequiredToolType", PropertyType.String); //Must match the type on the repair tool (subject to change upon fleshing out this system.)
            PropertyValidation.CreateValidProperty("Repairable.ToolType", PropertyType.String); //Must match the type on the repair tool (subject to change upon fleshing out this system.)

            //Descriptors
            PropertyValidation.CreateValidProperty("Descriptor.Repair.Repairing", PropertyType.String); //Describes how it looks when it gets repaired.
            PropertyValidation.CreateValidProperty("Descriptor.Repair.Interaction.Title", PropertyType.String); //Describes the interact option presented to the player.
            PropertyValidation.CreateValidProperty("Descriptor.Repair.Interaction.Invoking", PropertyType.String); //Describes the action of repairing it when a player uses the Repair() choice.
        }

        protected override void OnObjectChanged(object sender, ObjectChangedEventArgs propertyChangedData)
        {
            PropertyContainer sourceContainer = propertyChangedData.Source;
            PropertyHandler sourceProperties = sourceContainer.Properties;

            // When to do with Damage.Broken
            if (propertyChangedData.Property == "Damage.Broken")
            {
                // When something repairable breaks
                if (propertyChangedData.Property == "Damage.Broken" && propertyChangedData.Change.Equals(true))
                {
                    if (sourceProperties.HasPropertyAndValue("Repairable.ByChoice", true))
                        PublicInteractions.AddPublicInteractionz(sourceContainer, RepairDelegation.GetRepairChoice(sourceContainer, sourceProperties));
                }

                // When something repairable is repaired
                if (propertyChangedData.Property == "Damage.Broken" && propertyChangedData.Change.Equals(false))
                    PublicInteractions.TryRemovePublicInteraction(sourceContainer, RepairDelegation.GetRepairChoice(sourceContainer, sourceProperties));
            }
        }

        protected override void InitialiseNewSubscriber(PropertyContainer sourceContainer, PropertyHandler sourceProperties)
        {
            // If the object is broken already
            if (sourceProperties.HasPropertyAndValue("Damage.Broken", true))
            {
                if (sourceProperties.HasPropertyAndValue("Repairable.ByChoice", true))
                    PublicInteractions.TryAddPublicInteraction(sourceContainer, RepairDelegation.GetRepairChoice(sourceContainer, sourceProperties));
            }

            // If the object is already repaired
            if (!sourceProperties.HasPropertyAndValue("Damage.Broken", true))
                RepairProcessor.SetRepairDescriptors(sourceContainer); //Ensure its descriptors are correct.
        }
    }
}
