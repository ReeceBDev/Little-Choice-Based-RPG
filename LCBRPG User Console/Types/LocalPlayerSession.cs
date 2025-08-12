using Little_Choice_Based_RPG.External.EndpointServices;
using Little_Choice_Based_RPG.External.Types.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCBRPG_User_Console.Types
{
    /// <summary> Holds services for the player's user interface. </summary>
    internal class LocalPlayerSession
    {
        public ulong SessionToken { get; }

        public SessionService PlayerSessionServiceInstance { get; }
        public ServiceWorker PlayerServiceWorkerInstance { get; }
        public UserInputService UserInputServiceInstance { get; }
        public InteractionService InteractionServiceInstance { get; }
        public HistoryLogService HistoryLogServiceInstance { get; }
        public PlayerStatusService PlayerStatusServiceInstance { get; }
        public PlayerInventoryService PlayerInventoryServiceInstance { get; }
        public PlayerMapService PlayerMapServiceInstance { get; }
        public TimeService TimeServiceInstance { get; }


        public LocalPlayerSession()
        {
            PlayerSessionServiceInstance = new(); //Initialise a player session service, first.     
            PlayerServiceWorkerInstance = new(); //Initialise a new worker thread for the player.

            //Begin by creating a guest session and caching its session token.
            SessionToken = PlayerSessionServiceInstance.CreateGuestSession();

            //Initialise all other required services.
            UserInputServiceInstance = new();
            InteractionServiceInstance = new(SessionToken, PlayerServiceWorkerInstance);
            HistoryLogServiceInstance = new(SessionToken);
            PlayerStatusServiceInstance = new(SessionToken);
            PlayerInventoryServiceInstance = new();
            PlayerMapServiceInstance = new();
            TimeServiceInstance = new();
        }
    }
}
