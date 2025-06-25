using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;

namespace Little_Choice_Based_RPG.Types.PlayerControl
{
    public readonly record struct PendingInteractionCacheUpdate(ulong InteractionIdentity, IInvokableInteraction InteractionProfile, Boolean IsAvailable, long TimeStamp);
}
