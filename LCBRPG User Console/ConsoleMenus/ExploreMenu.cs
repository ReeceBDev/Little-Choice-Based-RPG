using LCBRPG_User_Console.ConsoleUtilities;
using LCBRPG_User_Console.MenuResource;
using LCBRPG_User_Console.Types;
using LCBRPG_User_Console.Types.ConsoleElements;
using LCBRPG_User_Console.Types.DisplayData;
using Little_Choice_Based_RPG.External.EndpointServices;
using Little_Choice_Based_RPG.External.Types;
using Little_Choice_Based_RPG.Managers.PlayerControl;
using Little_Choice_Based_RPG.Types.Interactions;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;

namespace LCBRPG_User_Console.ConsoleMenus
{
    internal class ExploreMenu : NumberedConsoleMenu
    {
        public ExploreMenu(LocalPlayerSession setPlayerSession, InteractionCache setInteractionCache, HistoryLogCache setHistoryLogCache, 
            ChangeInterfaceCallback changeMenuCallback) : base(setPlayerSession, setInteractionCache, setHistoryLogCache, changeMenuCallback)
        {
        }

        protected override List<InteractionDisplayData> InitialiseSystemChoices()
        {
            List<InteractionDisplayData> newSystemChoices = base.InitialiseSystemChoices();
            ulong systemInteractionID = 0;

            // Add a choice which opens the player's inventory
            newSystemChoices.Add(new InteractionDisplayData(++systemInteractionID, "Open your inventory.", InteractionRole.System.ToString(), 
                PlayerSession.PlayerStatusServiceInstance.GetPlayerID()));

            seededSystemInteractions.Add(
                systemInteractionID, //Must be the same as its respective display data above
                () => SystemChoices.SwitchMenuLogic(
                    this,
                    ChangeInterfaceCallbackInstance,
                    new InventoryMenu(PlayerSession, interactionCacheResource, historyLogCacheResource, ChangeInterfaceCallbackInstance))
                );

            return newSystemChoices;
        }

        protected override DisplayDataList InitialiseMenuElements(List<InteractionDisplayData> displayedInteractions)
        {
            DisplayDataList newElements = base.InitialiseMenuElements(displayedInteractions);

            newElements.ChangePriority(2, ElementIdentities.TransitionalAction, this);
            newElements.UpsertElement(3, new ExploreDescriptionElement(ElementIdentities.CurrentDescription, PlayerSession), this);
            newElements.UpsertElement(4, new HistoryLogElement(ElementIdentities.HistoryLog, historyLogCacheResource), this);
            newElements.ChangePriority(5, ElementIdentities.AvailableChoices, this);

            return newElements;
         }
    }
}
