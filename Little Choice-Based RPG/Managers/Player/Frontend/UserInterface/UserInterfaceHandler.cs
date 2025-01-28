using Little_Choice_Based_RPG.Managers.World;
using Little_Choice_Based_RPG.Resources.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Managers.Player.Frontend.UserInterface
{
    internal class UserInterfaceHandler
    {
        // Instantiate a new UserInterface
        // Handle the logic and the errors

        public string GenerateWorldview()
        {
            /*
Console.WriteLine($"\t{currentRoomTemporary.Name}\t -=-\t Potsun Burran\t -=-\t Relative, {currentCoordinates}\t -=-\t {DateTime.UtcNow.AddYears(641)}" +
                      $"\n ===========-==========----========-========-=--..-- ." +
                      $"\n\t{lastActionDescription}" +
                      $"\n{baseDescription}" +
                      $"\n ====-====-===-=--=-=--_-----_--= =- -_ ._" +
                      //$"\n {listChoices}" +
                      $"===========" +
                      $"\n\t>>> ");
    string? userInput = Console.ReadLine();

    while (userInput == null)
    {
        userInput = Console.ReadLine();
    }

    Pause();
    Console.Clear();
*/
            return "Hi this is the main game screen";
        }

        public string GeneratePlayerMenu()
        {
            return "Welcome to the Player menu...";
        }

        public string GenerateMainMenu()
        {
            return "You are at the main menu :)";
        }
    }
}
