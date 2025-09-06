using Little_Choice_Based_RPG.Resources.Components.ComponentAccessors;
using Little_Choice_Based_RPG.Types.PropertySystem.Containers;
using Little_Choice_Based_RPG.Types.PropertySystem.Properties;

namespace Little_Choice_Based_RPG.Resources.Entities.Physical.Furniture.Lightbulbs
{
    internal class TwentyFourInchLightstrip : Lightbulb
    {
        private readonly static List<Func<PropertyContainer, IProperty?>> requiredProperties = new()
        {

        };

        private readonly static List<Action<PropertyContainer>> defaultProperties = new()
        {
            { i => i.Weight.WeightInKG = 5.8m },
        };

        public TwentyFourInchLightstrip(List<Action<PropertyContainer>>? derivedProperties = null)
            : base(ConcatenateProperties(derivedProperties, defaultProperties))
        {
            //Validate required properties have been set on entityProperties
            ValidateRequiredProperties(requiredProperties);
        }
    }
}
