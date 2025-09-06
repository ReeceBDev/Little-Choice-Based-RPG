using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PublicInteractionsSystems;
using Little_Choice_Based_RPG.Types;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using Little_Choice_Based_RPG.Types.PropertyExtensions;
using Little_Choice_Based_RPG.Types.PropertyExtensions.PropertyExtensionEventArgs;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using Little_Choice_Based_RPG.Types.PropertySystem.Properties;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;

namespace Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PrivateInteractionsSystems
{
    internal class PrivateInteractions : IPropertyExtension, ITransparentCollectionSource<KeyValuePair<IPropertyContainer, IInvokableInteraction>>
    {
        private ReaderWriterLockSlim writeLock = new();
        private KeyValuePair<ImmutableArray<KeyValuePair<IPropertyContainer, IInvokableInteraction>>, long> lastKnownState = new(); // KVP<Last Known State Array<KVP<subject, interaction>>, Timestamp when last modified>
        
        public string UniqueIdentifier { get; init; }
        protected string interactionAddedEventID { get; init; }
        protected string interactionRemovedEventID { get; init; }
        private ConcurrentQueue<PrivateInteractionStateChange> PendingStateChanges { get; set; } = new();

        public event EventHandler<PropertyExtensionChangedArgs> PropertyExtensionChanged;

        public PrivateInteractions()
        {
            UniqueIdentifier = "PrivateInteractions";
            interactionAddedEventID = $"{UniqueIdentifier}.Added";
            interactionRemovedEventID = $"{UniqueIdentifier}.Removed";

        }

        /// <summary> Concurrently provides a technically-historical aggregated snapshot of the interactions list, at least as recent as when this property was called.
        /// For the most up to date information, subscribe to notifications from this class to listen for changes first, before getting this historical data. </summary>
        public IEnumerable<object> GetAllEntries() => GetRecentInteractions().Cast<object>();

        /// <summary> Concurrently provides a technically-historical aggregated snapshot of the interactions list, at least as recent as when this property was called.
        /// For the most up to date information, subscribe to notifications from this class to listen for changes first, before getting this historical data. </summary>
        public ImmutableArray<KeyValuePair<IPropertyContainer, IInvokableInteraction>> GetAll() => GetRecentInteractions();

        /// <summary> Add a new interaction. </summary>
        public void Add(KeyValuePair<IPropertyContainer, IInvokableInteraction> interactionKVP)
            //IInvokableInteraction interaction, IPropertyContainer subject)
        {
            writeLock.EnterWriteLock();
            try
            {
                if (lastKnownState.Key.Contains(interactionKVP))
                    throw new Exception("The interaction already existed on this object!");

                //Add interaction
                PendingStateChanges.Enqueue(new PrivateInteractionStateChange(interactionKVP, true, DateTime.UtcNow.Ticks));

                //Report state as having changed
                PropertyExtensionChanged?.Invoke(this, new PropertyExtensionChangedArgs(interactionAddedEventID, interactionKVP));

                //Commit with the new state
                UpdateLastKnownState();
            }
            finally
            {
                writeLock.ExitWriteLock();
            }
        }

        /// <summary> Removes an interaction. Supports optional-removes. </summary>
        public void Remove(KeyValuePair<IPropertyContainer, IInvokableInteraction> interactionKVP)
        {
            writeLock.EnterWriteLock();
            try
            {
                if (lastKnownState.Key.Contains(interactionKVP))
                {
                    //Remove interaction
                    PendingStateChanges.Enqueue(new PrivateInteractionStateChange(interactionKVP, false, DateTime.UtcNow.Ticks));

                    //Report state as having changed 
                    PropertyExtensionChanged?.Invoke(this, new PropertyExtensionChangedArgs(interactionRemovedEventID, interactionKVP));

                    //Commit with the new state
                    UpdateLastKnownState();
                }
            }
            finally
            {
                writeLock.ExitWriteLock();
            }
        }

        /// <summary> Concurrently provides a technically-historical aggregated snapshot of the interactions list, at least as recent as when this property was called.
        /// For the most up to date information, subscribe to notifications from this class to listen for changes first, before getting this historical data. </summary>
        private ImmutableArray<KeyValuePair<IPropertyContainer, IInvokableInteraction>> GetRecentInteractions()
            => PendingStateChanges.Count.Equals(0) ? lastKnownState.Key : BuildCurrentState().Key;

        private KeyValuePair<ImmutableArray<KeyValuePair<IPropertyContainer, IInvokableInteraction>>, long> BuildCurrentState()
        {
            //Take snapshots of current information, as of now, starting with pending changes.
            PrivateInteractionStateChange[] changesSnapshot;
            KeyValuePair<ImmutableArray<KeyValuePair<IPropertyContainer, IInvokableInteraction>>, long> lastKnownSnapshot;
            ImmutableArray<KeyValuePair<IPropertyContainer, IInvokableInteraction>>.Builder currentStateBuilder;
            long lastModified;

            changesSnapshot = PendingStateChanges.ToArray(); //Must come before taking the lastKnownState, in order to ensure pending changes are not skipped when applied concurrently.
            lastKnownSnapshot = lastKnownState; //Must come after taking changesSnapshot
            currentStateBuilder = lastKnownSnapshot.Key.ToBuilder();
            lastModified = lastKnownSnapshot.Value;

            //Apply pending changes to last known state
            foreach (var change in changesSnapshot)
            {
                if (change.TimeStamp >= lastKnownSnapshot.Value)
                {
                    if (change.IsAvailable) //Add interaction
                    {
                        if (!currentStateBuilder.Contains(change.InteractionKVP))
                            currentStateBuilder.Add(change.InteractionKVP);
                    }
                    else //Remove interaction
                    {
                        currentStateBuilder.Remove(change.InteractionKVP);
                    }

                    lastModified = change.TimeStamp;
                }
            }

            var newState = currentStateBuilder.ToImmutable();

            return KeyValuePair.Create(newState, lastModified);
        }

        private void UpdateLastKnownState()
        {
            //Update last known state
            Interlocked.Exchange(ref lastKnownState, BuildCurrentState());

            //Dequeue pending changes
            while (PendingStateChanges.TryPeek(out PrivateInteractionStateChange test) && test.TimeStamp <= lastKnownState.Value)
                PendingStateChanges.TryDequeue(out _);
        }
    }
}
