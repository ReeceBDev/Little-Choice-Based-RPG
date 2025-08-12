using Little_Choice_Based_RPG.Resources.Entities;
using Little_Choice_Based_RPG.Types;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using Little_Choice_Based_RPG.Types.PlayerControl;
using Little_Choice_Based_RPG.Types.PropertyExtensions;
using Little_Choice_Based_RPG.Types.PropertyExtensions.PropertyExtensionEventArgs;
using Microsoft.VisualBasic;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PublicInteractionsSystems
{
    internal class PublicInteractions : IPropertyExtension
    {
        private ReaderWriterLockSlim writeLock = new();
        private KeyValuePair<ImmutableArray<IInvokableInteraction>, long> lastKnownState = new(); // KVP<Last Known State Array, Timestamp when last modified>

        public string UniqueIdentifier { get; init; }
        protected string interactionAddedEventID { get; init; }
        protected string interactionRemovedEventID { get; init; }
        private ConcurrentQueue<PublicInteractionStateChange> PendingStateChanges { get; set; } = new();

        public event EventHandler<PropertyExtensionChangedArgs> PropertyExtensionChanged;

        public PublicInteractions()
        {
            UniqueIdentifier = "PublicInteractions";
            interactionAddedEventID = $"{UniqueIdentifier}.Added";
            interactionRemovedEventID = $"{UniqueIdentifier}.Removed";
        }

        /// <summary> Concurrently provides a technically-historical aggregated snapshot of the interactions list, at least as recent as when this property was called.
        /// For the most up to date information, subscribe to notifications from this class to listen for changes first, before getting this historical data. </summary>
        public ImmutableArray<IInvokableInteraction> RecentInteractions
            => PendingStateChanges.Count.Equals(0) ? lastKnownState.Key : BuildCurrentState().Key;

        /// <summary> Add a new interaction. </summary>
        public void AddInteraction(IInvokableInteraction interaction)
        {
            writeLock.EnterWriteLock();
            try
            {
                if (lastKnownState.Key.Contains(interaction))
                    throw new Exception("The interaction already existed on this object!");

                //Add interaction
                PendingStateChanges.Enqueue(new PublicInteractionStateChange(interaction, true, DateTime.UtcNow.Ticks));

                //Report state as having changed
                PropertyExtensionChanged?.Invoke(this, new PropertyExtensionChangedArgs(interactionAddedEventID, interaction));

                //Commit with the new state
                UpdateLastKnownState();
            }
            finally
            {
                writeLock.ExitWriteLock();
            }
        }

        /// <summary> Tries to remove an interaction. Supports optional-removes. Returns true if successful, in case error handling is required. </summary>
        public bool TryRemoveInteraction(IInvokableInteraction interaction)
        {
            writeLock.EnterWriteLock();
            try
            {
                if (lastKnownState.Key.Contains(interaction))
                {
                    //Remove interaction
                    PendingStateChanges.Enqueue(new PublicInteractionStateChange(interaction, false, DateTime.UtcNow.Ticks));

                    //Report state as having changed 
                    PropertyExtensionChanged?.Invoke(this, new PropertyExtensionChangedArgs(interactionRemovedEventID, interaction));

                    //Commit with the new state
                    UpdateLastKnownState();

                    return true;
                }

                return false;
            }
            finally
            {
                writeLock.ExitWriteLock();
            }
        }

        private KeyValuePair<ImmutableArray<IInvokableInteraction>, long> BuildCurrentState()
        {
            //Take snapshots of current information, as of now, starting with pending changes.
            PublicInteractionStateChange[] changesSnapshot;
            KeyValuePair<ImmutableArray<IInvokableInteraction>, long> lastKnownSnapshot;
            ImmutableArray<IInvokableInteraction>.Builder currentStateBuilder;
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
                        if (!currentStateBuilder.Contains(change.Interaction))
                            currentStateBuilder.Add(change.Interaction);
                    }
                    else //Remove interaction
                    {
                        currentStateBuilder.Remove(change.Interaction);
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
            while (PendingStateChanges.TryPeek(out PublicInteractionStateChange test) && test.TimeStamp <= lastKnownState.Value)
                PendingStateChanges.TryDequeue(out _);
        }
    }
}