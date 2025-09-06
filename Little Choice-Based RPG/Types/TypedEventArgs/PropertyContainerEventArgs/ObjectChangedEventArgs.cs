using Little_Choice_Based_RPG.Types.PropertySystem.Entities;

namespace Little_Choice_Based_RPG.Types.TypedEventArgs.PropertyContainerEventArgs
{
    internal record struct ObjectChangedEventArgs(IPropertyContainer Source, string Property, object Change);
}
