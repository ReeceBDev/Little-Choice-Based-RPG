using Little_Choice_Based_RPG.Types.PropertySystem.Entities;

namespace Little_Choice_Based_RPG.Types.PropertySystem.Systems.SystemEventBus
{
    internal static class SystemSubscriptionEventBus
    {
        public static event EventHandler<SystemSubscriptionRequestEventArgs> SystemSubcriptionRequest;
        public static event EventHandler<SystemSubscriptionRequestEventArgs> SystemUnsubscriptionRequest;

        public static void Subscribe(IPropertyContainer targetObject, string systemReferenceName) =>
            SystemSubcriptionRequest?.Invoke(targetObject, new SystemSubscriptionRequestEventArgs(targetObject, systemReferenceName));

        public static void Unsubscribe(IPropertyContainer targetObject, string systemReferenceName) =>
            SystemUnsubscriptionRequest?.Invoke(targetObject, new SystemSubscriptionRequestEventArgs(targetObject, systemReferenceName));
    }
}
