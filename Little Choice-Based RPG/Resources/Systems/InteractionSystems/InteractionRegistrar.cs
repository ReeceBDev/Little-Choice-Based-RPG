using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PrivateInteractionsSystems;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PublicInteractionsSystems;
using Little_Choice_Based_RPG.Types;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;

namespace Little_Choice_Based_RPG.Resources.Systems.InteractionSystems
{
    /// <summary> Static helper which forwards methods from both PublicInteractions and PrivateInteractions, each from their respective system of the same prefix.
    /// Simplifies registering interactions from anywhere, without needing to find, verify and reference the extensions, first. </summary>
    internal static class InteractionRegistrar
    {
        /// <summary> Tries to add a new public interaction. Returns success, true being successful. </summary>
        public static bool TryAddPublicInteraction(IPropertyContainer target, IInvokableInteraction interaction)
        {
            if (!target.Extensions.Contains("PublicInteractions"))
                return false; //PublicInteractions was not found on the target.

            try
            {
                ((PublicInteractions)target.Extensions.Get("PublicInteractions")).AddInteraction(interaction);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary> Tries to remove a new public interaction. Returns success, true being successful. </summary>
        public static bool TryRemovePublicInteraction(IPropertyContainer target, IInvokableInteraction interaction)
        {
            if (!target.Extensions.Contains("PublicInteractions"))
                return false; //PublicInteractions was not found on the target.

            try
            {
                ((PublicInteractions)target.Extensions.Get("PublicInteractions")).TryRemoveInteraction(interaction);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary> Tries to add a new private interaction. Returns success, true being successful. </summary>
        public static bool TryAddPrivateInteraction(IPropertyContainer target, IPropertyContainer associatedContainer, IInvokableInteraction interaction)
        {
            if (!target.Extensions.Contains("PrivateInteractions"))
                return false; //PrivateInteractions was not found on the target.

            try
            {
                ((PrivateInteractions)target.Extensions.Get("PrivateInteractions")).Add(KeyValuePair.Create(associatedContainer, interaction));
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary> Tries to remove a new private interaction. Returns success, true being successful. </summary>
        public static bool TryRemovePrivateInteraction(IPropertyContainer target, IPropertyContainer associatedContainer, IInvokableInteraction interaction)
        {
            if (!target.Extensions.Contains("PrivateInteractions"))
                return false; //PrivateInteractions was not found on the target.

            try
            {
                ((PrivateInteractions)target.Extensions.Get("PrivateInteractions")).Remove(KeyValuePair.Create(associatedContainer, interaction));
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
