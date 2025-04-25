using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PrivateInteractionsSystems.PrivateInteractionsExtensions;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PublicInteractionsSystems
{
    public static class PublicInteractionsLogic
    {
        public static void AddPublicInteraction(PropertyContainer target, IInvokableInteraction interaction)
        {
            if (target.Interactions.Contains(interaction))
                throw new Exception("The interaction already existed on the target object {target}! Remember, like removes, you could always skip duplicate values...? But I'll leave this here for now just in case :) <- Best exception message. Wow.");

            target.Interactions.Add(interaction);
        }

        /// <summary> Attempts to remove the interaction. Supports optional removes. Returns bool in case error-handling is required. </summary>
        public static bool TryRemovePublicInteraction(PropertyContainer target, IInvokableInteraction interaction)
        {
            if (target.Interactions.Contains(interaction))
            {
                target.Interactions.Remove(interaction);
                return true;
            }

            return false;
        }
    }
}
