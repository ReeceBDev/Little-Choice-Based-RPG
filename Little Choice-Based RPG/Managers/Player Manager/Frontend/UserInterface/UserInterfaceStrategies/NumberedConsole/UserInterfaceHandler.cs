using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface.UserInterfaceStyles;
using Little_Choice_Based_RPG.Managers.World;
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

        bool exitGame = false;
        public UserInterfaceHandler(PlayerController setCurrentPlayerController)
        {        
            CurrentMenu = new ExploreMenu(setCurrentPlayerController);
        }

        public void GenerateOutput()
        {
            while (exitGame == false)
            {
                Console.Clear();
                Console.WriteLine("\x1b[3J"); //Escape code from stackoverflow! It clears the whole console buffer :) *Magic*
                CurrentMenu.RunMenu();
            }

            Console.WriteLine("Exiting the game...");
            UserInterfaceUtilities.Pause();
        }

        public void OnExitGameRequest() //EVENT :) PLEASE PUT THIS IN!!
        {
            exitGame = true;
        }

        public void ChangeInterface(IUserInterface newInterfaceStyle)
        {
            CurrentMenu = newInterfaceStyle;
        }

        public IUserInterface CurrentMenu { get; private set; }

    }
}
