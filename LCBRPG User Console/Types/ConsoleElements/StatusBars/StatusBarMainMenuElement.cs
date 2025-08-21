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
    internal class StatusBarMainMenuElement : ElementLogic
    {
        private LocalPlayerSession playerSession;
        private PlayerStatusService statusService;

        public StatusBarMainMenuElement(ElementIdentities setUniqueIdentity, LocalPlayerSession currentPlayerSession) : base(setUniqueIdentity)
        {
            playerSession = currentPlayerSession;
            statusService = playerSession.PlayerStatusServiceInstance;
        }

        protected override string GenerateContent()
        {
            string topStatusPrefix = " ╔══════════════════════════════════════════════════════════════════════════════════" + "══════-=════-=═=-=--=-=-- - - -";
            string topStatusInfix = "\n ║ ";
            string topStatusBar = $"Little Choice-Based RPG :)" +
                $"\t -=-\t WELCOME TO THE MAIN MENU \t -=-\t -=════- \t -=- \t {DateTime.UtcNow.AddYears(641)}";

            string concatenatedStatusBar = topStatusPrefix + topStatusInfix + topStatusBar;

            return concatenatedStatusBar;
        }
    }
}
