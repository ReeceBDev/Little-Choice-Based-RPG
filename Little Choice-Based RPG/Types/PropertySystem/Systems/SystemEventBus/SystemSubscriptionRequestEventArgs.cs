using Little_Choice_Based_RPG.Types.PropertySystem.Entities;

namespace Little_Choice_Based_RPG.Types.PropertySystem.Systems.SystemEventBus
{
    internal record struct SystemSubscriptionRequestEventArgs(IPropertyContainer targetPropertyContainer, string systemReferenceName);
}
