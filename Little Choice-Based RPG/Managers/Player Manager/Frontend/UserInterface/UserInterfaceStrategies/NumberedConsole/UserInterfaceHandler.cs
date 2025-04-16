using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface.UserInterfaceStyles;
using Little_Choice_Based_RPG.Managers.World;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Types.Interaction;

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

        bool exitGame = false;

        public UserInterfaceHandler(PlayerController currentPlayerController)
        {
            ChangeInterfaceStyleCallback changeInterface = new ChangeInterfaceStyleCallback(ChangeUserInterfaceStyle);
            currentInterfaceStyle = new ExploreMenu(changeInterface, currentPlayerController); 
        }
        public void GenerateOutput()
        {
            while (exitGame == false)
            {
                Console.Clear();
                currentInterfaceStyle.RunMenu();
                UserInterfaceUtilities.Pause();
            }

            Console.WriteLine("Exiting the game...");
            UserInterfaceUtilities.Pause();
        }

        public void OnExitGameRequest() //EVENT :) PLEASE PUT THIS IN!!
        {
            exitGame = true;
        }

        private void ChangeUserInterfaceStyle(IUserInterface newInterfaceStyle)
        {
            currentInterfaceStyle = newInterfaceStyle;
        }
    }
}
