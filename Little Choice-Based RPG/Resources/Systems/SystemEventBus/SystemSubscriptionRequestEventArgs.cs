using Little_Choice_Based_RPG.Resources.Entities;

namespace Little_Choice_Based_RPG.Resources.Systems.SystemEventBus
{
    public record struct SystemSubscriptionRequestEventArgs(PropertyContainer targetPropertyContainer, string systemReferenceName);
}
