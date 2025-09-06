using Little_Choice_Based_RPG.Types.PropertySystem.Containers;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using Little_Choice_Based_RPG.Types.PropertySystem.Properties;
using Little_Choice_Based_RPG.Resources.Components.ComponentAccessors;

namespace Little_Choice_Based_RPG.Resources.Entities.Conceptual
{
    internal class Immaterial : PropertyContainer
    {
        private readonly static List<Func<PropertyContainer, IProperty?>> requiredProperties = new()
        {
            { i => i.Immaterial.IsImmaterial },
        };


        private readonly static List<Action<PropertyContainer>> defaultProperties = new()
        {
            {  i => i.Immaterial.IsImmaterial ??= true },
            { i => i.Identity.Name ??= $"Unknown Immaterial of type \"{i.GetType().Name.ToString()}\"" }
        };

        private protected Immaterial(List<Action<PropertyContainer>>? derivedProperties = null)
            : base(ConcatenateProperties(derivedProperties, defaultProperties))
        {
           //Ensure immaterial
            this.Immaterial.IsImmaterial = true;

            //Freeze IsImmaterial property.
            this.Immaterial.IsImmaterial.Freeze();
 
            //Validate required properties have been set on entityProperties
            ValidateRequiredProperties(requiredProperties);
        }
    }
}
