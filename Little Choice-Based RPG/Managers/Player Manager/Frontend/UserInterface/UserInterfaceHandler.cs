using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface.UserInterfaceStyles;
using Little_Choice_Based_RPG.Managers.World;
using Little_Choice_Based_RPG.Resources.Choices;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;

namespace Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface
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
    public class UserInterfaceHandler
    {
        // Instantiate a new UserInterface
        // Handle the logic and the errors

        public delegate void ChangeInterfaceStyleCallback(IUserInterface newUserInterfaceStyle);
        private IUserInterface currentInterfaceStyle; // This uses the Strategy Pattern

        public UserInterfaceHandler(Player currentPlayer, GameEnvironment currentEnvironment)
        {
            ChoiceHandler currentChoiceHandler = new ChoiceHandler(currentPlayer, currentEnvironment);

            ChangeInterfaceStyleCallback changeInterface = new ChangeInterfaceStyleCallback(ChangeUserInterfaceStyle);
            currentInterfaceStyle = new ExploreStyle(changeInterface, currentPlayer, currentEnvironment, currentChoiceHandler); 
        }
        public void GenerateOutput()
        {
            bool exitGame = false;

            while (exitGame == false)
            {
                Console.WriteLine(currentInterfaceStyle.OutputMainBody());
                string? userInput = Console.ReadLine();

                while (userInput == null)
                {
                    userInput = Console.ReadLine();
                }

                if (userInput == "1")
                    ChangeUserInterfaceStyle(new InteractionMenuStyle());
                else
                    ChangeUserInterfaceStyle(new MainMenuStyle());

                UserInterfaceUtilities.Pause();
                Console.Clear();
                userInput = null;
            }
        }

        private void ChangeUserInterfaceStyle(IUserInterface newInterfaceStyle)
        {
            currentInterfaceStyle = newInterfaceStyle;
        }
    }
}
