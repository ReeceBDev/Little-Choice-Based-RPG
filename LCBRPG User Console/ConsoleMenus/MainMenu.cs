using LCBRPG_User_Console.ConsoleUtilities;
using LCBRPG_User_Console.MenuResource;
using LCBRPG_User_Console.Types;
using Little_Choice_Based_RPG.External.Types;
using Little_Choice_Based_RPG.Managers.PlayerControl;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;

namespace LCBRPG_User_Console.ConsoleMenus
{
    internal class MainMenu : NumberedConsoleMenu
    {
        public MainMenu(LocalPlayerSession setPlayerSession,InteractionCache setInteractionCache, HistoryLogCache setHistoryLogCache, 
            ChangeInterfaceCallback changeMenuCallback) : base(setPlayerSession, setInteractionCache, setHistoryLogCache, changeMenuCallback)
        {
        }

        protected override List<InteractionDisplayData> InitialiseSystemChoices()
        {
            List<InteractionDisplayData> newSystemChoices = new();

            ulong systemInteractionID = 0;

            //Add a choice to switch to the exploration menu.
            newSystemChoices.Add(new InteractionDisplayData(++systemInteractionID, "-=- START THE GAME -=-", InteractionRole.System.ToString(),
                PlayerSession.PlayerStatusServiceInstance.GetPlayerID()));

            seededSystemInteractions.Add(
                systemInteractionID, //Must be the same as its respective display data above
                () => SystemChoices.SwitchMenuLogic(
                    this,
                    ChangeInterfaceCallbackInstance,
                    new ExploreMenu(PlayerSession, interactionCacheResource, historyLogCacheResource, ChangeInterfaceCallbackInstance))
                );

            return newSystemChoices;
        }
    }
}