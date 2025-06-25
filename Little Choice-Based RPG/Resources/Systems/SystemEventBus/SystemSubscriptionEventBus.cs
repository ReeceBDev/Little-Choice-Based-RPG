using Little_Choice_Based_RPG.Resources.Entities;

namespace Little_Choice_Based_RPG.Resources.Systems.SystemEventBus
{
    public static class SystemSubscriptionEventBus
    {
        public static event EventHandler<SystemSubscriptionRequestEventArgs> SystemSubcriptionRequest;
        public static event EventHandler<SystemSubscriptionRequestEventArgs> SystemUnsubscriptionRequest;

        public static void Subscribe(PropertyContainer targetObject, string systemReferenceName) =>
            SystemSubcriptionRequest?.Invoke(targetObject, new SystemSubscriptionRequestEventArgs(targetObject, systemReferenceName));

        public static void Unsubscribe(PropertyContainer targetObject, string systemReferenceName) =>
            SystemUnsubscriptionRequest?.Invoke(targetObject, new SystemSubscriptionRequestEventArgs(targetObject, systemReferenceName));
    }
}
