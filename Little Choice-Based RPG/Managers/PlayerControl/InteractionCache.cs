using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory.InventoryExtensions;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PrivateInteractionsSystems;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PublicInteractionsSystems;
using Little_Choice_Based_RPG.Types;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using Little_Choice_Based_RPG.Types.PlayerControl;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using Little_Choice_Based_RPG.Types.TypedEventArgs;
using Little_Choice_Based_RPG.Types.TypedEventArgs.InteractionCache;
using Little_Choice_Based_RPG.Types.TypedEventArgs.PropertyContainerEventArgs;
using System.Collections.Concurrent;
using System.Data;

namespace Little_Choice_Based_RPG.Managers.PlayerControl
{
    /// <summary> The point of authority for all available player interactions. Maintains a live cache of every available interaction referenced by its unique interactionID. 
    /// Responsible for listening for environmental changes so that it may keep up-to-date with which interactions are available at all times. </summary>
    internal class InteractionCache
    {
        private Mutex dictionaryLock = new();
        private volatile int refreshingCacheMutex = 0; //Incremental flag tells incoming RefreshCache() whether one is already running. Int instead of bool to account fora rare race condition, which is ignorable.
        private int freezeCacheMutex = 0; //Incremental flag to tell incoming interaction events to queue up until cache updates are finished. More than 0 means currently in held by that many threads.

        private ConcurrentDictionary<ulong, IInvokableInteraction> availableInteractionsCache = new(); //Authoritative list of interactions available to the player. <interactionID, interaction>.
        private ConcurrentQueue<PendingInteractionCacheUpdate> pendingInteractionUpdates = new(); //The boolean is 1 for add, 0 for remove.

        private PlayerController currentPlayerController;
        private HashSet<IPropertyContainer> subscribedTargets = new(); //Unique set of every item listened to. Targets should be subscribed to when added, and unsubscribed upon removing - preserving one-off subscriptions.

        public event EventHandler<InteractionAddedEventArgs> InteractionAdded; //Fired upon an interaction being added to the cache.
        public event EventHandler<InteractionRemovedEventArgs> InteractionRemoved; //Fired upon an interaction being removed from the cache.
        public event EventHandler CacheRefreshed; //Fired upon the cache being rebuilt.

        public InteractionCache(PlayerController setPlayerController)
        {
            List<GameObject> localObjects;

            currentPlayerController = setPlayerController;

            //Subscribe to changes to the Player's Private Interactions
            currentPlayerController.CurrentPlayer.ContainerChanged += OnPlayerChanged;

            //Subscribe to changes to the Player's Position
            currentPlayerController.PlayerChangedRoom += OnPlayerChangedRoom;

            //Subscribe to changes to that Room's Inventory
            currentPlayerController.CurrentRoom.ContainerChanged += OnRoomChanged;

            //Subscribe to changes to that Inventory's items' Public Interactions
            localObjects = InventoryProcessor.GetInventoryEntities(currentPlayerController.CurrentRoom);

            foreach (GameObject item in localObjects)
            {
                if (subscribedTargets.Add(item)) //If the item has been added, and is therefore the only unique instance of it being added (Hashsets are unique, being sets)
                    item.ContainerChanged += OnItemChanged; //Subscribe to that item, only once, since it was only just added :)
            }
        }

        /// <summary> Returns all state in both the cache and queue, valid at the point in time in which this method was called. </summary>
        public (Dictionary<ulong, IInvokableInteraction>, long) GetAvailableInteractions()
        {
            Dictionary<ulong, IInvokableInteraction> cacheSnapshot;
            long timeQueried = DateTime.UtcNow.Ticks;

            //Hold dictionary
            dictionaryLock.WaitOne();
            Interlocked.Increment(ref freezeCacheMutex); //Now we can be sure availableInteractionsCache wont be written to

            //Commit any lingering queue state to the cache. Commit each peek into the cache.
            while (pendingInteractionUpdates.TryPeek(out PendingInteractionCacheUpdate test) && test.TimeStamp <= timeQueried)
            {
                pendingInteractionUpdates.TryDequeue(out PendingInteractionCacheUpdate targetState);

                availableInteractionsCache.TryAdd(targetState.InteractionIdentity, targetState.InteractionProfile); //Make available.
            }

            //Get the cache state
            cacheSnapshot = availableInteractionsCache.ToDictionary();

            //Release dictionary
            Interlocked.Decrement(ref freezeCacheMutex); 
            dictionaryLock.ReleaseMutex();

            return (cacheSnapshot, timeQueried);
        }

        public void RefreshCache(PlayerController currentPlayerController)
        {
            //Locked thread guard clause
            if (!dictionaryLock.WaitOne(0)) //Lock if unlocked, then proceed
                if (refreshingCacheMutex > 0) //Check whether it's this method, or another which is currently using this mutex. For example, GetAvailableInteractions() also uses this mutex.
                {
                    return; //Skip if RefreshCache is already on-going. This is just a latest cache refresh, so it's not essential to repeat.
                            // Changes will be being recorded, anyway, and subscribed classes will be listening for the CacheRefreshed event.
                }
                else //If it is a different mutex in use, and not this class, then continue with this method's execution and await the mutex becoming free.
                {
                    dictionaryLock.WaitOne(); //Wait
                }

            Interlocked.Increment(ref refreshingCacheMutex);

            //Record - freeze the cache
            Interlocked.Increment(ref freezeCacheMutex);

            //Mark the current timestamp of existing fetches for later reference.
            //Any recordings beyond this timestamp will have authority to replace entries having been fetched, as recordings are always the most up-to-date.
            long cacheFetchesBegan = DateTime.UtcNow.Ticks;

            //Update the cache with the latest existing interactions. These will be considered to all use the above timestamp.
            //Due to performance constraints with concatenating results, the following functions apply to the availableInteractionsCache directly, rather than returning the values here first.
            availableInteractionsCache.Clear();
            GetPublicInteractions();
            GetPrivateInteractions();

            //Apply recordings, replacing cache fetches if the recordings are newer.
            while (pendingInteractionUpdates.Count > 0)
            {
                pendingInteractionUpdates.TryDequeue(out PendingInteractionCacheUpdate interactionUpdate);

                //Skip an entry if it falls before the time in which the fetches were taken.
                if (interactionUpdate.TimeStamp < cacheFetchesBegan)
                    continue;

                //Modify the cache based on the recordings taken since the cache was frozen.
                if (interactionUpdate.IsAvailable) //Interaction is to be added.
                {
                    if (availableInteractionsCache.ContainsKey(interactionUpdate.InteractionIdentity)) //Skip if already present (which in this case, also means already available).
                        continue;

                    availableInteractionsCache.TryAdd(interactionUpdate.InteractionIdentity, interactionUpdate.InteractionProfile); //Make available.
                }
                else //Interaction is to be removed
                {
                    if (!availableInteractionsCache.ContainsKey(interactionUpdate.InteractionIdentity)) //Skip if not present (which in this case, also means already unavailable).
                        continue;

                    availableInteractionsCache.TryRemove(KeyValuePair.Create(interactionUpdate.InteractionIdentity, interactionUpdate.InteractionProfile)); //Make unavailable.

                    if (availableInteractionsCache.ContainsKey(interactionUpdate.InteractionIdentity))
                        throw new Exception("Failed to remove an interaction, likely because the InteractionProfile didn't match the InteractionIdentity. This exception warrants investigation. This method here has a slight mis-match in how it checks for interactions it wants to remove. It checks if the interaction's InteractionIdentity is present, and if it is, it continues to try to remove it. However, removing it requires both key and value, so it is assumed that the value matches the key, since they both should come from/reference the same IInvokableInteraction. Therefore if this error occurs, check that the identity was truly matching the IInvokableInteraction, and go from there. If it was matching, there's probably some bigger issue, like duplicate InteractionIdentities, or maybe an interaction was set as both public and private.");
                }
            }
 
            //Release cache from being frozen when recordings are empty
            Interlocked.Decrement(ref freezeCacheMutex);

            //Notify of completion
            CacheRefreshed?.Invoke(this, EventArgs.Empty);

            //Release mutex
            dictionaryLock.ReleaseMutex();

            //Release cache refresh
            Interlocked.Decrement(ref refreshingCacheMutex);
        }

        private void OnPlayerChanged(object sender, ObjectChangedEventArgs e)
        {
            switch (e.Property, e.Change)
            {
                //If the player gained a new PrivateInteraction
                case ("PrivateInteractions.Added", IInvokableInteraction interaction):
                    {
                        if (Volatile.Read(ref freezeCacheMutex) > 0)
                        {
                            pendingInteractionUpdates.Enqueue(new PendingInteractionCacheUpdate(interaction.UniqueInstanceID, interaction, true, DateTime.UtcNow.Ticks));
                            break;
                        }

                        availableInteractionsCache.TryAdd(interaction.UniqueInstanceID, interaction);

                        //Announce gained interaction
                        InteractionAdded?.Invoke(this, new InteractionAddedEventArgs(interaction));
                    }
                    break;

                //If the player lost an old PrivateInteraction
                case ("PrivateInteractions.Removed", IInvokableInteraction interaction):
                    {
                        if (Volatile.Read(ref freezeCacheMutex) > 0)
                        {
                            pendingInteractionUpdates.Enqueue(new PendingInteractionCacheUpdate(interaction.UniqueInstanceID, interaction, false, DateTime.UtcNow.Ticks));
                            break;
                        }

                        availableInteractionsCache.TryRemove(new KeyValuePair<ulong, IInvokableInteraction>(interaction.UniqueInstanceID, interaction));

                        //Announce lost interaction
                        InteractionRemoved?.Invoke(this, new InteractionRemovedEventArgs(interaction));
                    }
                    break;
            }
        }

        private void OnPlayerChangedRoom(object sender, PlayerChangedRoomEventArgs e)
        {
            //Stop listening to the old Room.
            e.OldRoom.ContainerChanged -= OnRoomChanged;

            //Stop listening to old items
            foreach (var target in subscribedTargets.ToList())
            {
                //Try to remove from the subscriptions, and then unsubscribe if it was listed as listened-to.
                if (subscribedTargets.Remove(target))
                    target.ContainerChanged -= OnItemChanged;
            }

            //Start listening to the new Room.
            e.NewRoom.ContainerChanged += OnRoomChanged;

            //Start listening to the new rooms' items
            foreach (GameObject item in InventoryProcessor.GetInventoryEntities(e.NewRoom))
            {
                //Subscribe to that item, only once, since it was only just added, uniquely, and wont be re-added, making double-subscriptions impossible.
                if (subscribedTargets.Add(item)) 
                    item.ContainerChanged += OnItemChanged; 
            }

            //Refresh cache.
            RefreshCache(currentPlayerController);
        }

        private void OnRoomChanged(object sender, ObjectChangedEventArgs e)
        {
            switch (e.Property, e.Change)
            {
                //If the room gained an item
                case ("ItemContainer.Added", GameObject itemGained):
                    {
                        ///Listen for changes to its Public Interactions list
                        if (subscribedTargets.Add(itemGained)) //Subscribe to that item only once
                            itemGained.ContainerChanged += OnItemChanged;
                    }
                    break;

                //If the room lost an item
                case ("ItemContainer.Removed", GameObject itemLost):
                    {
                        //Stop listening to changes in its Public Interactions list
                        //Try to remove from the subscription list, and then unsubscribe if needed.
                        if (subscribedTargets.Remove(itemLost))
                            itemLost.ContainerChanged -= OnItemChanged;  
                    }
                    break;
            }
        }

        private void OnItemChanged(object sender, ObjectChangedEventArgs e)
        {
            switch (e.Property, e.Change)
            {
                //If the player gained a new public interaction
                case ("PublicInteractions.Added", Interaction interaction):
                    {
                        if (Volatile.Read(ref freezeCacheMutex) > 0)
                        {
                            pendingInteractionUpdates.Enqueue(new PendingInteractionCacheUpdate(interaction.UniqueInstanceID, interaction, true, DateTime.UtcNow.Ticks));
                            break;
                        }

                        availableInteractionsCache.TryAdd(interaction.UniqueInstanceID, interaction);

                        //Announce gained interaction
                        InteractionAdded?.Invoke(this, new InteractionAddedEventArgs(interaction));
                    }
                    break;

                //If the player lost an old public interaction
                case ("PublicInteractions.Removed", Interaction interaction):
                    {
                        if (Volatile.Read(ref freezeCacheMutex) > 0)
                        {
                            pendingInteractionUpdates.Enqueue(new PendingInteractionCacheUpdate(interaction.UniqueInstanceID, interaction, false, DateTime.UtcNow.Ticks));
                            break;
                        }

                        availableInteractionsCache.TryRemove(new KeyValuePair<ulong, IInvokableInteraction>(interaction.UniqueInstanceID, interaction));

                        //Announce lost interaction
                        InteractionRemoved?.Invoke(this, new InteractionRemovedEventArgs(interaction));
                    }
                    break;
            }
        }

        /// <summary> Returns interactionIdentity, interaction. 
        /// Due to performance constraints with concatenating results, the following method applies updates directly to the availableInteractionsCache. </summary>
        private void GetPublicInteractions()
        {
            var localObjects = InventoryProcessor.GetInventoryEntities(currentPlayerController.CurrentRoom);

            foreach (GameObject target in localObjects)
            {
                if (target.Extensions.Contains("PublicInteractions"))
                    foreach (var interactionKeyValuePair in ((PublicInteractions)target.Extensions.Get("PublicInteractions")).RecentInteractions)
                    {
                        availableInteractionsCache.TryAdd(interactionKeyValuePair.UniqueInstanceID, interactionKeyValuePair);
                    }
            }
        }

        /// <summary> Returns interactionIdentity, interaction. 
        /// Due to performance constraints with concatenating results, the following method applies updates directly to the availableInteractionsCache. </summary>
        private void GetPrivateInteractions()
        {
            if (!currentPlayerController.CurrentPlayer.Extensions.Contains("PrivateInteractions"))
                throw new ArgumentException($"The current player, {currentPlayerController.CurrentPlayer} does not contain an extension named \"PrivateInteractions\" and therefore can't hold the private interactions! Extensions list: {currentPlayerController.CurrentPlayer.Extensions}.");

            var playerInteractions = (PrivateInteractions)currentPlayerController.CurrentPlayer.Extensions.Get("PrivateInteractions");

            //Grab each private interaction
            foreach (var interactionKeyValuePair in playerInteractions.RecentInteractions)
            {
                availableInteractionsCache.TryAdd(interactionKeyValuePair.Value.UniqueInstanceID, interactionKeyValuePair.Value);
            }
        }
    }
}
