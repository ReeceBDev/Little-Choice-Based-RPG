using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Components.ComponentAccessors;
using Little_Choice_Based_RPG.Types.PropertySystem.Containers;
using Little_Choice_Based_RPG.Types.PropertySystem.Properties;

namespace Little_Choice_Based_RPG.Resources.Entities.Physical.Living
{
    internal class LivingCreature : GameObject
    {
        private readonly static List<Func<PropertyContainer, IProperty?>> requiredProperties = new()
        {
            { i => i.Healthpool.IsLiving },
            { i => i.Healthpool.Health },
        };

        private readonly static List<Action<PropertyContainer>> defaultProperties = new()
        {
            { i => i.Healthpool.IsLiving = true },
            { i => i.Healthpool.Health = 100u },
        };

        private protected LivingCreature(List<Action<PropertyContainer>>? derivedProperties = null)
            : base(ConcatenateProperties(derivedProperties, defaultProperties))
        {
            //Validate required properties have been set on entityProperties
            ValidateRequiredProperties(requiredProperties);
        }
    }
}
