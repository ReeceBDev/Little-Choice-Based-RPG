using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.DescriptorConditions;
using Little_Choice_Based_RPG.Types.PropertySystem.Containers;
using Little_Choice_Based_RPG.Types.PropertySystem.Properties;
using Little_Choice_Based_RPG.Resources.Components.ComponentAccessors;

namespace Little_Choice_Based_RPG.Resources.Entities.Rooms
{
    internal class Room : PropertyContainer
    {
        private readonly static List<Func<PropertyContainer, IProperty?>> requiredProperties = new()
        {
        };

        private readonly static List<Action<PropertyContainer>> defaultProperties = new()
        {
            { i => i.Inventory.UsesSystem = true },
            { i => i.Directions.UsesSystem = true }
        };

        public Room(List<Action<PropertyContainer>>? derivedProperties = null)
            : base(ConcatenateProperties(derivedProperties, defaultProperties))
        {
            //Validate required properties have been set on entityProperties
            ValidateRequiredProperties(requiredProperties);
        }
    }
}
