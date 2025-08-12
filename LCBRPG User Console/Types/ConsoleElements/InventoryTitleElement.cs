using LCBRPG_User_Console.Types.DisplayDataEntries;
using Little_Choice_Based_RPG.External.EndpointServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCBRPG_User_Console.Types.ActualElements
{
    internal class InventoryTitleElement : ElementLogic
    {
        private LocalPlayerSession playerSession;
        private PlayerStatusService statusService;

        public InventoryTitleElement(ElementIdentities setUniqueIdentity, LocalPlayerSession currentPlayerSession) : base(setUniqueIdentity)
        {
            playerSession = currentPlayerSession;
            statusService = playerSession.PlayerStatusServiceInstance;
        }

        protected override string GenerateContent()
        {
            string bottomStatus =
                "\n                -=-    INVENTORY    -=-        -=-    Current Weight Held: " +
                $"{statusService.GetPlayerWeightHeld()}kg " +
                $"/ {statusService.GetPlayerStrength()}kg " +
                "    -=-               ";

            return bottomStatus;
        }
    }
}
