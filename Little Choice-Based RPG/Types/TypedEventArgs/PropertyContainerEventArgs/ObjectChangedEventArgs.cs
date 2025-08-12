using Little_Choice_Based_RPG.Resources.Entities;

namespace Little_Choice_Based_RPG.Types.TypedEventArgs.PropertyContainerEventArgs
{
    internal record struct ObjectChangedEventArgs(PropertyContainer Source, string Property, object Change);
}
