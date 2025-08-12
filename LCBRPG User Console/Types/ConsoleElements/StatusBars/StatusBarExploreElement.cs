using LCBRPG_User_Console.Types.DisplayDataEntries;
using Little_Choice_Based_RPG.External.EndpointServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCBRPG_User_Console.Types.ActualElements.StatusBars
{
    internal class StatusBarExploreElement : ElementLogic
    {
        private LocalPlayerSession playerSession;
        private PlayerStatusService statusService;

        public StatusBarExploreElement(ElementIdentities setUniqueIdentity, LocalPlayerSession currentPlayerSession) : base(setUniqueIdentity)
        {
            playerSession = currentPlayerSession;
            statusService = playerSession.PlayerStatusServiceInstance;
        }

        protected override string GenerateContent()
        { 
            string topStatusPrefix = " ╔══════════════════════════════════════════════════════════════════════════════════" + "══════-=════-=═=-=--=-=-- - - -";
            string topStatusInfix = "\n ║ ";
            string topStatusBar = $"{statusService.GetPlayerLocation()}\t -=-\t Potsun Burran\t -=-\t Relative, {statusService.GetPlayerID()}\t -=-\t {TimeService.GetServerTime()}";

            string concatenatedStatusBar = topStatusPrefix + topStatusInfix + topStatusBar;

            return concatenatedStatusBar;
        }
    }
}
