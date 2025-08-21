using LCBRPG_User_Console.ConsoleMenus;
using LCBRPG_User_Console.ConsoleUtilities;
using LCBRPG_User_Console.MenuResource;
using LCBRPG_User_Console.Types;
using Little_Choice_Based_RPG.External.Types.Services;
using Little_Choice_Based_RPG.Managers.PlayerControl;

namespace LCBRPG_User_Console
{
    /// <summary>
    /// Provides an overview of the userinterface process, such as concatenating logic, input, output and error handling.
    /// 
    /// This class handles the logic of the userInterface experience, such as recieving input, error handling,
    /// including choosing which interface to display, how to display it, the recieving of user input and so on.
    /// 
    /// It composites the UserInterface class which handles the recieved text and
    /// it also composites the UserInterfaceUtilities class to stylise it.
    /// 
    /// For future reference: If I want to add static style effects mid-interface writing, then that should be done in the UserInterface class.
    /// This class is for creating an overview of the userinterface process, such as concatenating logic, input, output and error handling.
    /// </summary>
    internal class ConsoleEndpoint
    {
        // Instantiate a new UserInterface
        // Handle the logic and the errors
        private LocalPlayerSession playerSession = new();
        private InteractionCache interactionCache;
        private HistoryLogCache historyLogCache;

        bool exitGame = false;

        public ConsoleEndpoint()
        {
            //Initialise callbacks
            ChangeInterfaceCallback changeInterfaceCallback = ChangeInterface;

            //Initialise resources
            interactionCache = new InteractionCache(playerSession);
            historyLogCache = new HistoryLogCache(playerSession);

            //Call the first menu
            CurrentMenu = new ExploreMenu(playerSession, interactionCache, historyLogCache, changeInterfaceCallback);
        }

        public void GenerateOutput()
        {
            while (exitGame == false)
            {
                Console.Clear();
                Console.WriteLine("\x1b[3J"); //Escape code! It clears the whole console buffer :) *Magic*
                CurrentMenu.RunMenu();
            }

            Console.WriteLine("Exiting the game...");
            WritelineUtilities.Pause();
        }

        public void OnExitGameRequest() //EVENT :) PLEASE PUT THIS IN!!
        {
            exitGame = true;
        }

        public void ChangeInterface(IConsoleMenu newInterfaceStyle)
        {
            CurrentMenu = newInterfaceStyle;
        }

        public IConsoleMenu CurrentMenu { get; private set; }
        }
}