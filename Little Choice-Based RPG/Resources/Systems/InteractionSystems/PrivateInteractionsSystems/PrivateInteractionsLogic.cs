using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PrivateInteractionsSystems.PrivateInteractionsExtensions;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PrivateInteractionsSystems
{
    public static class PrivateInteractionsLogic
    {
        public static void GivePrivateInteraction(Player targetPlayer, IInvokableInteraction interaction)
        {
            if (!targetPlayer.Extensions.ContainsExtension("PrivateInteractions"))
                targetPlayer.Extensions.AddExtension(new PrivateInteractions());

            PrivateInteractions currentPrivateInteractions = (PrivateInteractions)targetPlayer.Extensions.GetExtension("PrivateInteractions");

            currentPrivateInteractions.PrivateInteractionsList.Add(interaction);
        }

        public static void TryRemovePrivateInteraction(Player targetPlayer, IInvokableInteraction interaction)
        {
            if (!targetPlayer.Extensions.ContainsExtension("PrivateInteractions"))
                return; //For the player does not have any interactions to begin with.

            PrivateInteractions currentPrivateInteractions = (PrivateInteractions)targetPlayer.Extensions.GetExtension("PrivateInteractions");

            if (!currentPrivateInteractions.PrivateInteractionsList.Exists(i => i.Equals(interaction)))
                return; //For the interaction did not exist on the player;

            var targetInteraction = currentPrivateInteractions.PrivateInteractionsList.Find(i => i.Equals(interaction));
            currentPrivateInteractions.PrivateInteractionsList.Remove(targetInteraction);
        }
    }
}
