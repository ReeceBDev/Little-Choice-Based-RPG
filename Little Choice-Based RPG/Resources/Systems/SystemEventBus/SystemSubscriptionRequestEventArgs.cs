using Little_Choice_Based_RPG.Resources.Entities;

namespace Little_Choice_Based_RPG.Resources.Systems.SystemEventBus
{
    internal record struct SystemSubscriptionRequestEventArgs(PropertyContainer targetPropertyContainer, string systemReferenceName);
}
