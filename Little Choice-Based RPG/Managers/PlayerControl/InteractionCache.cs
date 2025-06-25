using Little_Choice_Based_RPG.Resources.Entities;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.PropertyContainerEventArgs;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PrivateInteractionsSystems.PrivateInteractionsExtensions;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PublicInteractionsSystems.PublicInteractionsExtensions;
using Little_Choice_Based_RPG.Types;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using Little_Choice_Based_RPG.Types.PlayerControl;
using Little_Choice_Based_RPG.Types.TypedEventArgs;
using System.Collections.Concurrent;
using System.Data;

namespace Little_Choice_Based_RPG.Managers.PlayerControl
{
    /// <summary> The point of authority for all available player interactions. Maintains a live cache of every available interaction referenced by its unique interactionID. 
    /// Responsible for listening for environmental changes so that it may keep up-to-date with which interactions are available at all times. </summary>
    public class InteractionCache
    {
        private PlayerController currentPlayerController;

        private ConcurrentDictionary<ulong, IInvokableInteraction> availableInteractionsCache = new(); //Authoritative list of interactions available to the player. <interactionID, interaction>.
        private ConcurrentDictionary<ulong, PropertyContainer> publicInteractionSources = new(); //All targets currently being listened to for public Interactions.

        private bool freezeCache = false; //Mutex flag to tell incoming interaction events to queue up until cache updates are finished.
        private ConcurrentQueue<PendingInteractionCacheUpdate> pendingInteractionUpdates = new(); //The boolean is 1 for add, 0 for remove.

        public event EventHandler<IInvokableInteraction> InteractionAvailable; //Fired upon an interaction being added to the cache.
        public event EventHandler<IInvokableInteraction> InteractionRemoved; //Fired upon an interaction being removed from the cache.
        public event EventHandler CacheRefreshed; //Fired upon the cache being rebuilt.

        public InteractionCache(PlayerController setPlayerController)
        {
            List<GameObject> localObjects;

            currentPlayerController = setPlayerController;

            //Subscribe to changes to the Player's Private Interactions
            currentPlayerController.CurrentPlayer.ObjectChanged += OnPlayerChanged;

            //Subscribe to changes to the Player's Position
            currentPlayerController.PlayerChangedRoom += OnPlayerChangedRoom;

            //Subscribe to changes to that Room's Inventory
            currentPlayerController.CurrentRoom.ObjectChanged += OnRoomChanged;

            //Subscribe to changes to that Inventory's items' Public Interactions
            localObjects = InventoryProcessor.GetInventoryEntities(currentPlayerController.CurrentRoom);

            foreach (GameObject item in localObjects)
            {
                item.ObjectChanged += OnItemChanged;
            }
        }

        public void RefreshCache(PlayerController currentPlayerController)
        {
            //Locked thread guard clause
            if (!Monitor.TryEnter(this)) //Lock if unlocked, then proceed
                return; //Skip if already on-going. This is just a latest cache refresh, so it's not essential to repeat. Changes will be being recorded, anyway.

            ConcurrentDictionary<ulong, IInvokableInteraction> availableChoices = new(); //interactionIdentity, interaction
            long cacheFetchesBegan;

            //Record - freeze the cache
            freezeCache = true;

            //Mark the current timestamp of existing fetches for later reference.
            //Any recordings beyond this timestamp will have authority to replace entries having been fetched, as recordings are always the most up-to-date.
            cacheFetchesBegan = new DateTime().Ticks;

            //Fetch existing interactions. These will be considered to all use the above timestamp.
            availableChoices.Concat(GetPublicInteractions());
            availableChoices.Concat(GetPrivateInteractions());

            //Push existing interactions to the cache.
            availableInteractionsCache = availableChoices;

            //Apply recordings, replacing cache fetches if the recordings are newer.
            while (pendingInteractionUpdates.Count > 0)
            {
                foreach (var interactionUpdate in pendingInteractionUpdates)
                {
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
            }

            //Release cache from being frozen when recordings are empty
            freezeCache = false;

            //Notify of completion
            CacheRefreshed?.Invoke(this, EventArgs.Empty);

            //Release lock
            Monitor.Exit(this);
        }

        private void OnPlayerChanged(object sender, ObjectChangedEventArgs e)
        {
            switch (e.Property, e.Change)
            {
                //If the player gained a new PrivateInteraction
                case ("PrivateInteractions.Added", IInvokableInteraction interaction):
                    {
                        if (freezeCache)
                        {
                            pendingInteractionUpdates.Enqueue(new PendingInteractionCacheUpdate(interaction.UniqueInstanceID, interaction, true, new DateTime().Ticks));
                            break;
                        }

                        if (!availableInteractionsCache.ContainsKey(interaction.UniqueInstanceID))
                            availableInteractionsCache.TryAdd(interaction.UniqueInstanceID, interaction);
                    }
                    break;

                //If the player lost an old PrivateInteraction
                case ("PrivateInteractions.Removed", Interaction interaction):
                    {

                        if (freezeCache)
                        {
                            pendingInteractionUpdates.Enqueue(new PendingInteractionCacheUpdate(interaction.UniqueInstanceID, interaction, false, new DateTime().Ticks));
                            break;
                        }

                        if (!availableInteractionsCache.ContainsKey(interaction.UniqueInstanceID))
                            availableInteractionsCache.TryRemove(new KeyValuePair<ulong, IInvokableInteraction>(interaction.UniqueInstanceID, interaction));
                    }
                    break;
            }
        }

        private void OnPlayerChangedRoom(object sender, PlayerChangedRoomEventArgs e)
        {
            //Start listening to the new Room.
            e.NewRoom.ObjectChanged += OnRoomChanged;

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
                        //Listen for changes to its Public Interactions list
                        itemGained.ObjectChanged += OnItemChanged;
                    }
                    break;

                //If the room lost an item
                case ("ItemContainer.Removed", GameObject itemLost):
                    {
                        //Stop listening to changes in its Public Interactions list
                        itemLost.ObjectChanged -= OnItemChanged;
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
                        if (freezeCache)
                        {
                            pendingInteractionUpdates.Enqueue(new PendingInteractionCacheUpdate(interaction.UniqueInstanceID, interaction, true, new DateTime().Ticks));
                            break;
                        }

                        if (!availableInteractionsCache.ContainsKey(interaction.UniqueInstanceID))
                            availableInteractionsCache.TryAdd(interaction.UniqueInstanceID, interaction);
                    }
                    break;

                //If the player lost an old public interaction
                case ("PublicInteractions.Removed", Interaction interaction):
                    {
                        if (freezeCache)
                        {
                            pendingInteractionUpdates.Enqueue(new PendingInteractionCacheUpdate(interaction.UniqueInstanceID, interaction, false, new DateTime().Ticks));
                            break;
                        }

                        if (!availableInteractionsCache.ContainsKey(interaction.UniqueInstanceID))
                            availableInteractionsCache.TryRemove(new KeyValuePair<ulong, IInvokableInteraction>(interaction.UniqueInstanceID, interaction));
                    }
                    break;
            }
        }

        /// <summary> Returns interactionIdentity, interaction. </summary>
        private Dictionary<ulong, IInvokableInteraction> GetPublicInteractions()
        {
            var interactionData = new Dictionary<ulong, IInvokableInteraction>();
            var targetObjects = InventoryProcessor.GetInventoryEntities(currentPlayerController.CurrentRoom);

            foreach (GameObject target in targetObjects)
            {
                if (target.Extensions.Contains("PublicInteractions"))
                    foreach (var interactionKeyValuePair in ((PublicInteractions)target.Extensions.Get("PublicInteractions")).PublicInteractionsList)
                    {
                        interactionData.Add(interactionKeyValuePair.Key.UniqueInstanceID, interactionKeyValuePair.Key);
                    }
            }

            return interactionData;
        }

        /// <summary> Returns interactionIdentity, interaction. </summary>
        private Dictionary<ulong, IInvokableInteraction> GetPrivateInteractions()
        {
            if (!currentPlayerController.CurrentPlayer.Extensions.Contains("PrivateInteractions"))
                throw new ArgumentException($"The current player, {currentPlayerController.CurrentPlayer} does not contain an extension named \"PrivateInteractions\" and therefore can't hold the private interactions! Extensions list: {currentPlayerController.CurrentPlayer.Extensions}.");

            var interactionData = new Dictionary<ulong, IInvokableInteraction>();
            var playerInteractions = (PrivateInteractions)currentPlayerController.CurrentPlayer.Extensions.Get("PrivateInteractions");

            //Grab each private interaction
            foreach (KeyValuePair<KeyValuePair<PropertyContainer, IInvokableInteraction>, Unit> interactionKeyValuePair in playerInteractions.PrivateInteractionsList)
            {
                interactionData.Add(interactionKeyValuePair.Key.Value.UniqueInstanceID, interactionKeyValuePair.Key.Value);
            }

            return interactionData;
        }
    }
}
