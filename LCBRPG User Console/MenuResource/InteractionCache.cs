using LCBRPG_User_Console.Types;`
using Little_Choice_Based_RPG.External.Types.Services;
using Little_Choice_Based_RPG.External.Types.TypedEventArgs.InteractionService;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCBRPG_User_Console.MenuResource
{
    /// <summary> Synchronises a local interaction cache with the player's authoritative cache. Upon changes being made, requests that the interface updates. </summary>
    internal class InteractionCache : IMenuResource
    {
        private LocalPlayerSession playerSession;
        private HashSet<InteractionDisplayData> interactionCache; //Local interaction data to be displayed on the client.

        public event EventHandler ResourceUpdated;

        public InteractionCache(LocalPlayerSession setPlayerSession)
        {
            playerSession = setPlayerSession;

            //Subscribe to interaction service updates
            playerSession.InteractionServiceInstance.InteractionCacheRefreshRequired += OnInteractionRefreshRequired;
            playerSession.InteractionServiceInstance.InteractionAvailable += OnInteractionAvailable;
            playerSession.InteractionServiceInstance.InteractionRemoved += OnInteractionRemoved;

            //Initialise
            RefreshCache();
        }

        public ImmutableHashSet<InteractionDisplayData> GetCache() => interactionCache.ToImmutableHashSet();
        protected virtual void OnInteractionRefreshRequired(object? sender, EventArgs e)
        {
            RefreshCache();
        }

        protected virtual void OnInteractionAvailable(object? sender, InteractionServiceAdditionEventArgs e)
        {
            InteractionServiceData interaction = e.NewInteractionData;

            //Transmute the interaction from the other assemblies data type into the local data type for displaying that data
            InteractionDisplayData displayData = new InteractionDisplayData(interaction.InteractionID, interaction.InteractionTitle, interaction.PresentationContext);

            AddInteraction(displayData);
        }

        protected virtual void OnInteractionRemoved(object? sender, InteractionServiceRemovalEventArgs e)
        {
            RemoveInteraction(e.OldInteractionID);
        }

        protected virtual void InvokeResourceUpdated()
        {
            ResourceUpdated?.Invoke(this, EventArgs.Empty);
        }

        private void RefreshCache()
        {
            //Clear the old local cache.
            interactionCache.Clear();

            //Requeue the historical interaction cache. The new data will populate automatically.
            playerSession.InteractionServiceInstance.RequeueCacheHistory();
        }

        private void AddInteraction(InteractionDisplayData i)
        {
            //Check whether the InteractionID already exists
            InteractionDisplayData existingData = interactionCache.FirstOrDefault(x => x.InteractionID.Equals(i.InteractionID));

            if (existingData != default) //If the data already exist
            { 
                //Since it exists, check whether its data is the same
                if (existingData.InteractionTitle.Equals(i.InteractionTitle) && existingData.PresentationContext.Equals(i.PresentationContext))
                    return; //If it's the same, do nothing.

                //Otherwise remove its entry
                interactionCache.Remove(existingData);
            }

            //Add interaction by its display data
            interactionCache.Add(i);

            //Notify
            InvokeResourceUpdated();
        }

        private void RemoveInteraction(ulong interactionID)
        {
            //Remove the first entry matching the interactionID
            interactionCache.Remove(interactionCache.FirstOrDefault(i => i.InteractionID.Equals(interactionID)));

            //Notify
            InvokeResourceUpdated();
        }
    }
}