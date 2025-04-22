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
        public static void GiveInteraction(PropertyContainer target, IInvokableInteraction interaction)
        {
            target.Interactions.Add(interaction);
        }
    }
}
