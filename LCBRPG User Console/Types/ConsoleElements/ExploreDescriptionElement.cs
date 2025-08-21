using LCBRPG_User_Console.ConsoleUtilities;
using LCBRPG_User_Console.Types.DisplayData;
using Little_Choice_Based_RPG.External.EndpointServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LCBRPG_User_Console.Types.ConsoleElements
{
    internal class ExploreDescriptionElement : ElementLogic
    {
        private LocalPlayerSession playerSession;
        private PlayerStatusService statusService;

        public ExploreDescriptionElement(ElementIdentities setUniqueIdentity, LocalPlayerSession currentPlayerSession) : base(setUniqueIdentity)
        {
            playerSession = currentPlayerSession;
            statusService = playerSession.PlayerStatusServiceInstance;
        }

        protected override string GenerateContent()
        {
            List<string> roomLines = WritelineUtilities.SplitIntoLines(statusService.GetPlayerLocationDescription(), "\n ╟ ", "\n ║ ", "\n ╟ ");

            string descriptionPrefix =
                    "\n ║ Room Description │ " +
                    "\n ╙──────────────────┘ ";

            string finalRoomDescription = descriptionPrefix;

            foreach (string line in roomLines)
            {
                finalRoomDescription += line;
            }


            return finalRoomDescription;
        }
    }
}
