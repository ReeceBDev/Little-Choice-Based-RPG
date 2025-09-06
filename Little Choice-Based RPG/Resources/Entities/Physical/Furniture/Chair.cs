using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.PropertySystem.Containers;
using Little_Choice_Based_RPG.Types.PropertySystem.Properties;
using Little_Choice_Based_RPG.Resources.Components.ComponentAccessors;

namespace Little_Choice_Based_RPG.Resources.Entities.Physical.Furniture
{
    internal class Chair : GameObject
    {

        private readonly static List<Func<PropertyContainer, IProperty?>> requiredProperties = new()
        {

        };

        private readonly static List<Action<PropertyContainer>> defaultProperties = new()
        {
            { i => i.Flammability.UsesSystem = true },
            { i => i.Flammability.IsFlammable = true },
            { i => i.Weight.WeightInKG = 22.0m }
        };

        public Chair(List<Action<PropertyContainer>>? derivedProperties = null)
            : base(ConcatenateProperties(derivedProperties, defaultProperties))
        {
            //Validate required properties have been set on entityProperties
            ValidateRequiredProperties(requiredProperties);
        }
    }
}
