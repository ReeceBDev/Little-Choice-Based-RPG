using Little_Choice_Based_RPG.Managers.World;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;

namespace Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface
{
    /// <summary>
    /// Privdes an overview of the userinterface process, such as concatenating logic, input, output and error handling.
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
    internal class UserInterfaceHandler
    {
        private protected UserInterface currentInterface;
        private protected UserInterfaceUtilities userInterfaceUtility;
        // Instantiate a new UserInterface
        // Handle the logic and the errors

        public UserInterfaceHandler(Player currentPlayer, GameEnvironment currentEnvironment)
        {
            var currentInterface = new UserInterface(currentPlayer, currentEnvironment);
        }
        public void GenerateWorldview()
        {
            string selectedUserInterfaceStyle = currentInterface.Default();

            Console.WriteLine(selectedUserInterfaceStyle);
            string? userInput = Console.ReadLine();

            while (userInput == null)
            {
                userInput = Console.ReadLine();
            }

            userInterfaceUtility.Pause();
            Console.Clear();
        }
    }
}
