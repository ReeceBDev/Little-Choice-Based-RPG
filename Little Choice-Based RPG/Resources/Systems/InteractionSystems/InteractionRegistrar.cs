using Little_Choice_Based_RPG.Resources.Entities;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PrivateInteractionsSystems.PrivateInteractionsExtensions;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PublicInteractionsSystems.PublicInteractionsExtensions;
using Little_Choice_Based_RPG.Types;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;

namespace Little_Choice_Based_RPG.Resources.Systems.InteractionSystems
{
    /// <summary> Static helper which forwards methods from both PublicInteractions and PrivateInteractions, each from their respective system of the same prefix.
    /// Simplifies registering interactions from anywhere, without needing to find, verify and reference the extensions, first. </summary>
    public static class InteractionRegistrar
    {
        /// <summary> Tries to add a new public interaction. Returns success, true being successful. </summary>
        public static bool TryAddPublic(PropertyContainer target, IInvokableInteraction interaction)
        {
            if (!target.Extensions.Contains("PublicInteractions"))
                return false; //PublicInteractions was not found on the target.

            try
            {
                ((PublicInteractions)target.Extensions.Get("PublicInteractions")).PublicInteractionsList.TryAdd(interaction, Unit.Value);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary> Tries to remove a new public interaction. Returns success, true being successful. </summary>
        public static bool TryRemovePublic(PropertyContainer target, IInvokableInteraction interaction)
        {
            if (!target.Extensions.Contains("PublicInteractions"))
                return false; //PublicInteractions was not found on the target.

            try
            {
                ulong bean = 0;
                ((PublicInteractions)target.Extensions.Get("PublicInteractions")).PublicInteractionsList.TryRemove(KeyValuePair.Create(interaction, Unit.Value));
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary> Tries to add a new private interaction. Returns success, true being successful. </summary>
        public static bool TryAddPrivate(PropertyContainer target, PropertyContainer associatedContainer, IInvokableInteraction interaction)
        {
            if (!target.Extensions.Contains("PrivateInteractions"))
                return false; //PrivateInteractions was not found on the target.

            try
            {
                ((PrivateInteractions)target.Extensions.Get("PrivateInteractions")).TryAddPrivateInteraction(associatedContainer, interaction);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary> Tries to remove a new private interaction. Returns success, true being successful. </summary>
        public static bool TryRemovePrivate(PropertyContainer target, PropertyContainer associatedContainer, IInvokableInteraction interaction)
        {
            if (!target.Extensions.Contains("PrivateInteractions"))
                return false; //PrivateInteractions was not found on the target.

            try
            {
                ((PrivateInteractions)target.Extensions.Get("PrivateInteractions")).TryRemovePrivateInteraction(associatedContainer, interaction);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
