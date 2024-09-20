using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml;
using Little_Choice_Based_RPG.Choices;
using Little_Choice_Based_RPG.Objects.Entities.Players;

namespace Little_Choice_Based_RPG
{
    internal class UserInterface
    {
        public void GenerateUserInterface(Player player)
        {
            uint currentRoom = player.CurrentRoomID;
            string baseDescription = Description.Write(player);
            string contextualDescription = Description.LastAction(player);
            string listChoices = ListChoices(); 

            Console.WriteLine($"\t\t{currentRoom}  -=- Potsun Burran" +
                              $"====-====-===-=--=-=--_-----_--= =- -_ ._" +
                              $"\n\t{contextualDescription}" +
                              $"\n\t{baseDescription}" +
                              $"===========-==========----========-========-=--..-- ." +
                              $"\n {listChoices}" +
                              $"===========" +
                              $"\n\t>>> ");
        }

        public string ListChoices()
        {
            string output = string.Empty;
            int currentChoiceIndex = 0;

            foreach (var choice in ChoiceHandler.Choices) // Add (player)... later!
            {
                output += ($" {currentChoiceIndex}. /t {choice.Name}");
                currentChoiceIndex++;
            }

            return output;
        }
    }
}
