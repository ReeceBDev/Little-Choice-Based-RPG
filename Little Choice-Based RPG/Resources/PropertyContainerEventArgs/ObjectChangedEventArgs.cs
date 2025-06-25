using Little_Choice_Based_RPG.Resources.Entities;

namespace Little_Choice_Based_RPG.Resources.PropertyContainerEventArgs
{
    public record struct ObjectChangedEventArgs(PropertyContainer Source, string Property, object Change);
}
