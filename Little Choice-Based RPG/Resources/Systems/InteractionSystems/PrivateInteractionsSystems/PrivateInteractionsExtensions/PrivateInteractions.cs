using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using Little_Choice_Based_RPG.Types.PropertyExtensions;
using Little_Choice_Based_RPG.Types.PropertyExtensions.PropertyExtensionEventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PrivateInteractionsSystems.PrivateInteractionsExtensions
{
    public class PrivateInteractions : IPropertyExtension
    {
        public List<KeyValuePair<PropertyContainer, IInvokableInteraction>>PrivateInteractionsList { get; private set; } = new();

        public event EventHandler<PropertyExtensionChangedArgs> PropertyExtensionChanged; //No need to invoke this yet, it has no practical purpose on this class (yet). :)
        public string UniqueIdentifier { get; init; } = "PrivateInteractions";

        public static void GiveNew(Player targetPlayer, PropertyContainer associatedContainer, IInvokableInteraction interaction)
        {
            if (!targetPlayer.Extensions.Contains("PrivateInteractions"))
                targetPlayer.Extensions.AddExtension(new PrivateInteractions());

            PrivateInteractions currentPrivateInteractions = (PrivateInteractions)targetPlayer.Extensions.Get("PrivateInteractions");

            currentPrivateInteractions.PrivateInteractionsList.Add(KeyValuePair.Create(associatedContainer, interaction));
        }

        /// <summary> Attempts to remove the interaction. Supports optional removes. Returns an error code in case error-handling is required. </summary>
        public static int TryRemove(Player targetPlayer, PropertyContainer associatedContainer, IInvokableInteraction interaction)
        {
            int errorCode;

            if (!targetPlayer.Extensions.Contains("PrivateInteractions"))
                return errorCode = 1; //throw new Exception($"TryRemovePrivateInteraction() Returned error code: 1. Tried to remove an interaction but the player {targetPlayer} did not have the extension \"PrivateInteractions\" to begin with!");

            var targetInteractionKeyPair = KeyValuePair.Create(associatedContainer, interaction);
            PrivateInteractions currentPrivateInteractions = (PrivateInteractions)targetPlayer.Extensions.Get("PrivateInteractions");

            if (!currentPrivateInteractions.PrivateInteractionsList.Any(i => (i.Value.Equals(targetInteractionKeyPair.Value)) && (i.Key.Equals(targetInteractionKeyPair.Key))))
                return errorCode = 2; //throw new Exception($"TryRemovePrivateInteraction() Returned error code: 2. Tried to remove an interaction but it did not exist on the player {targetPlayer}!");

            currentPrivateInteractions.PrivateInteractionsList.Remove(targetInteractionKeyPair);
            return errorCode = -1; //Success
        }
    }
}
