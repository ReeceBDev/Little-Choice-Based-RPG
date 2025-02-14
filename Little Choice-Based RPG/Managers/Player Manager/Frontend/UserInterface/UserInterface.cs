using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Managers.World;
using Little_Choice_Based_RPG.Resources.Choices;
using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Types;

namespace Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface
{
    // Outlines the possible interface styles to be used by the UserInterfaceHandler
    public class UserInterface
    {
        private protected uint currentRoomID;
        private protected string currentRoomName;
        private protected string currentRoomDescription;

        public UserInterface(Player player, GameEnvironment currentEnvironment)
        {
            currentRoomID = player.Position;
            Room currentRoomPrinciple = currentEnvironment.GetRoomByID(currentRoomID);
            currentRoomName = currentRoomPrinciple.Name;
            currentRoomDescription = CreateRoomDescription(currentRoomPrinciple.GetRoomDescriptors());
        }

        private string CreateRoomDescription(List<string> roomDescriptors)
        {
            string createdRoomDescription = "";
            foreach (string roomDescriptor in roomDescriptors)
            {
                createdRoomDescription += (roomDescriptor + " ");
            }
            return createdRoomDescription;
        }

        public string DefaultStyle()
        {
            string userInterfaceStyle = string.Join( "\n",
                          $"{this.currentRoomName}\t -=-\t Potsun Burran\t -=-\t Relative, {currentRoomID}\t -=-\t {DateTime.UtcNow.AddYears(641)}",
                          $" ===========-========== ----========-========-= --..-- .",
                          $"{currentRoomDescription}",
                          $" ====-====-===-=--=-=--_-----_--= =- -_ ._",
                          $" ===========",
                          $">>> ");

            return userInterfaceStyle;
        }

        /*
                 //string lastActionDescription = player.LastActionDescriptor.Value;
                    //string listChoices = ListChoices();
        private protected string lastActionDescription;
        private protected string listChoices;

        //int choiceindex = Convert.ToInt32(userInput);



        lastActionDescription = CurrentChoiceHandler.Choices.ElementAt(choiceindex).OnExecute?.Invoke();

            if (lastActionDescription == null)
                return;
            Console.WriteLine(lastActionDescription);


        }


        public string lastActionDescription = "";

        public ChoiceHandler CurrentChoiceHandler { get; init; } = new ChoiceHandler();
        */
    }
}
