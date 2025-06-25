using Little_Choice_Based_RPG.Resources.Entities;
using Little_Choice_Based_RPG.Types;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using Little_Choice_Based_RPG.Types.PropertyExtensions;
using Little_Choice_Based_RPG.Types.PropertyExtensions.PropertyExtensionEventArgs;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PrivateInteractionsSystems.PrivateInteractionsExtensions
{
    public class PrivateInteractions : IPropertyExtension
    {
        //Note: This has to be KeyValuePair, since several different PropertyContainers might share the same IInvokableInteraction fingerprints.
        //Similarly, this data can't be swapped, since IInvokableInteractions might match, too!
        //This was previously a list, without the ulong, before being forced to change into ConcurrentDictionary to support concurrent reads.

        private ConcurrentDictionary<KeyValuePair<PropertyContainer, IInvokableInteraction>, Unit> privateInteractionsList = new();

        public event EventHandler<PropertyExtensionChangedArgs> PropertyExtensionChanged;
        public string UniqueIdentifier { get; init; } = "PrivateInteractions";

        public ReadOnlyDictionary<KeyValuePair<PropertyContainer, IInvokableInteraction>, Unit> PrivateInteractionsList => privateInteractionsList.AsReadOnly();

        public void TryAddPrivateInteraction(PropertyContainer associatedContainer, IInvokableInteraction interaction)
        {
            lock(this) { 
            if (privateInteractionsList.ContainsKey(KeyValuePair.Create(associatedContainer, interaction)))
                throw new Exception("The interaction already existed on the target object {target}! Remember, like removes, you could always skip duplicate values...? But I'll leave this here for now just in case :) <- Best exception message. Wow.");

            privateInteractionsList.TryAdd(KeyValuePair.Create(associatedContainer, interaction), Unit.Value);

            PropertyExtensionChanged?.Invoke(this, new PropertyExtensionChangedArgs("PrivateInteractions.Added", interaction));
            }
        }

        /// <summary> Attempts to remove the interaction. Supports optional removes. Returns an error code in case error-handling is required. </summary>
        public int TryRemovePrivateInteraction(PropertyContainer associatedContainer, IInvokableInteraction interaction)
        {
            lock (this)
            {
                int errorCode;

            var targetInteractionKeyPair = KeyValuePair.Create(associatedContainer, interaction);

            if (!privateInteractionsList.Any(i => i.Value.Equals(targetInteractionKeyPair)))
                return errorCode = 2; //throw new Exception($"TryRemovePrivateInteraction() Returned error code: 2. Tried to remove an interaction but it did not exist on the player {targetPlayer}!");

            if (!privateInteractionsList.TryRemove(KeyValuePair.Create(targetInteractionKeyPair, Unit.Value)))
                return errorCode = 3; //throw new Exception($"TryRemovePrivateInteraction() Returned error code: 3. Tried to remove an interaction but this did not succeed for an unknown reason! {targetPlayer}!");

            PropertyExtensionChanged?.Invoke(this, new PropertyExtensionChangedArgs("PrivateInteractions.Removed", interaction));
            
            return errorCode = -1; //Success
            }
        }
    }
}
