using Little_Choice_Based_RPG.Types.External.Services;

namespace Little_Choice_Based_RPG.External.EndpointServices
{
    /// <summary> Provides endpoints with interactions and updates a cache to be replicated on the client. </summary>
    internal class InteractionService
    {
        public event EventHandler<InteractionServiceData> InteractionAvailable; //Sends a new interaction to an endpoint.
        public event EventHandler<InteractionServiceData> InteractionRemoved; //Sends a removal instruction to an endpoint.
        public event EventHandler InteractionCacheRefreshRequired; //Instructs endpoints to re-request their cache.
    }
}
