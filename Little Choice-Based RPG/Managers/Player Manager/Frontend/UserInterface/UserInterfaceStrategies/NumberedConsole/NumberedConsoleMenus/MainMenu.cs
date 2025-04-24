using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface.UserInterfaceStrategies.NumberedConsole;
using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface.UserInterfaceStrategies.NumberedConsole.NumberedConsoleMenus;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface.UserInterfaceStyles
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
