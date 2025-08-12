using LCBRPG_User_Console.ConsoleUtilities;
using LCBRPG_User_Console.MenuResource;
using LCBRPG_User_Console.Types;
using Little_Choice_Based_RPG.Managers.PlayerControl;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;

namespace LCBRPG_User_Console.ConsoleMenus
{
    internal class MainMenu : NumberedConsoleMenu
    {
        public MainMenu(LocalPlayerSession setPlayerSession,InteractionCache setInteractionCache, HistoryLogCache setHistoryLogCache) 
            : base(setPlayerSession, setInteractionCache, setHistoryLogCache)
        {
        }

        protected override List<InteractionDisplayData> InitialiseSystemChoices()
        {
            List<InteractionDisplayData> newSystemChoices = new();

            //Add a choice to switch to the exploration menu.
            newSystemChoices.Add(SystemChoices.LaunchExploreMenu);

            return newSystemChoices;
        }
    }
}