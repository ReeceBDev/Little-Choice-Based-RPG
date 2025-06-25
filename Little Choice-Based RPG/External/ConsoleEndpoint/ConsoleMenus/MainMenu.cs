using Little_Choice_Based_RPG.External.ConsoleEndpoint.ConsoleFunctionalities;
using Little_Choice_Based_RPG.Managers.PlayerControl;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;

namespace Little_Choice_Based_RPG.External.ConsoleEndpoint.ConsoleMenus
{
    public class MainMenu : NumberedConsoleMenu
    {
        public MainMenu(PlayerController setPlayerController) : base(setPlayerController)
        {
        }

        protected override List<IInvokableInteraction> InitialiseSystemChoices()
        {
            List<IInvokableInteraction> newSystemChoices = new();

            //Add a choice to switch to the exploration menu.
            newSystemChoices.Add(SystemChoices.LaunchExploreMenu);

            return newSystemChoices;
        }

        protected override string GenerateTopStatusBar()
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
