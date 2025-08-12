using Little_Choice_Based_RPG.External.Types;
using Little_Choice_Based_RPG.External.Types.Services;
using Little_Choice_Based_RPG.External.Types.TypedEventArgs.InteractionService;
using Little_Choice_Based_RPG.Managers.PlayerControl;
using Little_Choice_Based_RPG.Managers.Server;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.SingleParameterDelegates;
using Little_Choice_Based_RPG.Types.TypedEventArgs.InteractionCache;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace Little_Choice_Based_RPG.External.EndpointServices
{
    /// <summary> Watches a players interaction cache and sends replication data to endpoints so they may maintain a copy locally. </summary>
    public sealed class InteractionService
    {
        private readonly ServiceWorker workerThread;

        private readonly InteractionCache localCache;
        private Channel<InteractionServiceData> orderedUpdates = Channel.CreateUnbounded<InteractionServiceData>(); //Interaction data in chronological order.
        private Channel<EventArgs> cacheRefreshedEvent = Channel.CreateUnbounded<EventArgs>(); //A list of fired cacheRefreshed events for offloading the events onto worker.

        private bool clientRefreshOngoing; //When true, postpone updates from being sent to the client until after the refresh has completed.
        private ConcurrentQueue<InteractionServiceData> postponedUpdates = new(); //Temporarily withheld client updates. 

        private readonly object cacheHistoryLock = new();

        public InteractionService(ulong authenticationToken, ServiceWorker setServiceWorker)
        {
            workerThread = setServiceWorker;
            PlayerController currentPlayerController = SessionManager.GetPlayerController(authenticationToken);

            localCache = currentPlayerController.CurrentInteractionCache;

            localCache.InteractionAdded += OnInteractionAvailable;
            localCache.InteractionRemoved += OnInteractionRemoved;
            localCache.CacheRefreshed += OnCacheRefreshed;

            //Hand tasks over to the service worker
            Func<CancellationToken, Task<bool>> pushInteractionEvents = PushNextInteractionUpdate;
            workerThread.AssignTask(pushInteractionEvents);

            Func<CancellationToken, Task<bool>> cacheRefreshEvent = PushCacheRefreshEvent;
            workerThread.AssignTask(cacheRefreshEvent);
        }

        /// <summary> Re-queues historical updates which are still valid. The historical updates are then announced as if they were new. 
        /// Incoming updates from the moment this method is called will be postponed until after the historical updates are queued.
        /// 
        /// Note: It is recommended that Endpoints clear their local cache before calling this method, to ensure a clean slate.
        /// Do not worry about updates that slip in between the clear and the refresh. No updates are ever missed, so eventual consistency will be reached nearly immediately.
        /// </summary>
        public void RequeueCacheHistory()
        {
            //if already ongoing, skip.
            if (!Monitor.TryEnter(cacheHistoryLock))
                return;

            clientRefreshOngoing = true; //Postpone events from being sent directly to the client, until after this function has finished.

            //Fetch the up-to-date interaction cache
            InteractionServiceData[] upToDateInteractions = GetInteractionCache();

            //Send the up-to-date cache to the client.
            foreach(var entry in upToDateInteractions)
            { 
                orderedUpdates.Writer.TryWrite(entry);
            }

            //Send changes that occured during the above process to the client, after the up-to-date is sent.
            while (postponedUpdates.Count > 0)
            {
                postponedUpdates.TryDequeue(out InteractionServiceData pendingChange);
                orderedUpdates.Writer.TryWrite(pendingChange);

                //Release changes from being postponed.
                if (postponedUpdates.Count == 0)
                    clientRefreshOngoing = false;
            }

            //Release
            Monitor.Exit(cacheHistoryLock);
        }


        private async Task<bool> PushNextInteractionUpdate(CancellationToken cancelToken)
        {
            InteractionServiceData update = await orderedUpdates.Reader.ReadAsync(cancelToken);

            if (cancelToken.IsCancellationRequested)
                return false;

            if (update.IsAdded)
                InvokeInteractionAvailableEvent(this, new InteractionServiceAdditionEventArgs(update));
            else
                InvokeInteractionRemovedEvent(this, new InteractionServiceRemovalEventArgs(update.InteractionID));

            return true;
        }

        private async Task<bool> PushCacheRefreshEvent(CancellationToken cancelToken)
        {
            EventArgs eventFired = await cacheRefreshedEvent.Reader.ReadAsync(cancelToken);

            if (cancelToken.IsCancellationRequested)
                return false;

            InvokeCacheRefreshRequiredEvent(this, EventArgs.Empty);

            return true;
        }


        private InteractionServiceData[] GetInteractionCache()
        {
            var fetchData = localCache.GetAvailableInteractions();
            var interactionData = fetchData.Item1.Values.ToArray();
            var transposedCache = new InteractionServiceData[interactionData.Length];

            for (int i = 0; i < interactionData.Length; i++)
            {
                var interaction = interactionData[i];

                transposedCache[i] = new InteractionServiceData(interaction.UniqueInstanceID, interaction.InteractionTitle, interaction.InteractionContext.ToString(), true);
            }

            return transposedCache;
        }

        private void OnInteractionAvailable(object? sender, InteractionAddedEventArgs e)
        {
            IInvokableInteraction newInteraction = e.NewInteraction;

            InteractionServiceData interactionData = 
                new InteractionServiceData(newInteraction.UniqueInstanceID, newInteraction.InteractionTitle, newInteraction.InteractionContext.ToString(), true);

            if (clientRefreshOngoing)
            {
                postponedUpdates.Enqueue(interactionData);
                return;
            }                
            
            orderedUpdates.Writer.TryWrite(interactionData);
        }

        private void OnInteractionRemoved(object? sender, InteractionRemovedEventArgs e)
        {
            InteractionServiceData interactionData = 
                new InteractionServiceData(e.OldInteraction.UniqueInstanceID, e.OldInteraction.InteractionTitle, e.OldInteraction.InteractionContext.ToString(), false);

            if (clientRefreshOngoing)
            {
                postponedUpdates.Enqueue(interactionData);
                return;
            }
                
            orderedUpdates.Writer.TryWrite(interactionData);
        }

        private void OnCacheRefreshed(object? sender, EventArgs? e)
        {
            cacheRefreshedEvent.Writer.TryWrite(e);
        }

        private void InvokeInteractionAvailableEvent(object? sender, InteractionServiceAdditionEventArgs e)
        {
            InteractionAvailable?.Invoke(this, e);
        }

        private void InvokeInteractionRemovedEvent(object? sender, InteractionServiceRemovalEventArgs e)
        {
            InteractionRemoved?.Invoke(this, e);
        }

        private void InvokeCacheRefreshRequiredEvent(object? sender, EventArgs? e)
        {
            InteractionCacheRefreshRequired?.Invoke(this, e);
        }

        public event EventHandler<InteractionServiceAdditionEventArgs> InteractionAvailable; //Sends a new interaction to an endpoint.
        public event EventHandler<InteractionServiceRemovalEventArgs> InteractionRemoved; //Sends an interaction removal instruction to an endpoint, using only that interaction's ID.
        public event EventHandler InteractionCacheRefreshRequired; //Instructs endpoints to re-request their cache.
    }
}
