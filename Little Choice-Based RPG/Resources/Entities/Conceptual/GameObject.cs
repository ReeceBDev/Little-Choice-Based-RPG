using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory;
using Little_Choice_Based_RPG.Types.PropertySystem.Containers;
using Little_Choice_Based_RPG.Types.PropertySystem.Properties;
using Little_Choice_Based_RPG.Resources.Components.ComponentAccessors;

namespace Little_Choice_Based_RPG.Resources.Entities.Conceptual
{
    internal abstract class GameObject : PropertyContainer
    {
        private readonly static List<Func<PropertyContainer, IProperty?>> requiredProperties = new()
        {
            { i => i.Weight.WeightInKG }
        };

        private readonly static List<Action<PropertyContainer>> defaultProperties = new()
        {
        };

        private protected GameObject(List<Action<PropertyContainer>>? derivedProperties = null)
            : base(ConcatenateProperties(derivedProperties, defaultProperties))
        {
            //Validate required properties have been set on entityProperties
            ValidateRequiredProperties(requiredProperties);

            //Validate that pickup and putdown descriptors have been set if it falls under the current max-strength.
            InventoryPropertyValidation.ValidateInventoryDescriptors(this);
        }
    }
}
