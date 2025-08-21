using LCBRPG_User_Console.Types.ConsoleElements;
using LCBRPG_User_Console.Types.DisplayData;
using Little_Choice_Based_RPG.External.EndpointServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCBRPG_User_Console.Types.ConsoleElements.StatusBars
{
    internal class StatusBarInspectElement : ElementLogic
    {
        private LocalPlayerSession playerSession;
        private PlayerStatusService statusService;
        private uint inspecteeID;

        public StatusBarInspectElement(ElementIdentities setUniqueIdentity, LocalPlayerSession currentPlayerSession, uint setInspecteeID) : base(setUniqueIdentity)
        {
            playerSession = currentPlayerSession;
            statusService = playerSession.PlayerStatusServiceInstance;
            inspecteeID = setInspecteeID;
        }

        protected override string GenerateContent()
        {
            string topStatusPrefix = " ╔══════════════════════════════════════════════════════════════════════════════════" + "══════-=════-=═=-=--=-=-- - - -";
            string topStatusInfix = "\n ║ ";
            string topStatusBar = $"{playerSession.PlayerInventoryServiceInstance.GetItemName(inspecteeID)}" +
                $"\t -=-\t INSPECT VIEW \t -=-\t -=════- \t -=- \t {DateTime.UtcNow.AddYears(641)}";

            string concatenatedStatusBar = topStatusPrefix + topStatusInfix + topStatusBar;

            return concatenatedStatusBar;
        }
    }
}
