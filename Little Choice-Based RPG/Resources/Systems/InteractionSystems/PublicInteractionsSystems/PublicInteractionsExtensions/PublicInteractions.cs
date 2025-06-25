using Little_Choice_Based_RPG.Resources.Entities;
using Little_Choice_Based_RPG.Types;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using Little_Choice_Based_RPG.Types.PropertyExtensions;
using Little_Choice_Based_RPG.Types.PropertyExtensions.PropertyExtensionEventArgs;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PublicInteractionsSystems.PublicInteractionsExtensions
{
    public class PublicInteractions : IPropertyExtension
    {
        private ConcurrentDictionary<IInvokableInteraction, Unit> publicInteractionsList { get; private set; } = new(); //interaction, uniqueID

        public event EventHandler<PropertyExtensionChangedArgs> PropertyExtensionChanged;
        public string UniqueIdentifier { get; init; } = "PublicInteractions";
        public ReadOnlyDictionary<IInvokableInteraction, Unit> PublicInteractionsList => publicInteractionsList.AsReadOnly();

        /// <summary> Adds a new public interaction. </summary>
        public void TryAddPublicInteraction(IInvokableInteraction interaction)
        {
            lock (this)
            {
                if (publicInteractionsList.ContainsKey(interaction))
                    throw new Exception("The interaction already existed on this object!");

                if (publicInteractionsList.TryAdd(interaction, Unit.Value))
                    throw new Exception("Return false! Was not able to add an interaction, for some unknown reason!");

                PropertyExtensionChanged?.Invoke(this, new PropertyExtensionChangedArgs("PublicInteractions.Added", interaction));
            }
        }

        /// <summary> Tries to remove a public interaction. Supports optional-removes. Returns true if successful, in case error handling is required. </summary>
        public bool TryRemovePublicInteraction(IInvokableInteraction interaction)
        {
            lock (this)
            {
                if (publicInteractionsList.ContainsKey(interaction))
                    if (publicInteractionsList.TryRemove(KeyValuePair.Create(interaction, Unit.Value)))
                    {
                        PropertyExtensionChanged?.Invoke(this, new PropertyExtensionChangedArgs("PublicInteractions.Removed", interaction));
                        return true;
                    }

                return false;
            }
        }
    }
}
