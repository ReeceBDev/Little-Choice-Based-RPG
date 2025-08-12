using Little_Choice_Based_RPG.Types.EntityProperties;

namespace Little_Choice_Based_RPG.Resources.Entities
{
    internal interface IPropertyContainer
    {
        public PropertyHandler Properties { get; set; }
    }
}
