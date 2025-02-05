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
using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.Description;
using Little_Choice_Based_RPG.Managers.World;
using Little_Choice_Based_RPG.Resources.Choices;
using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Types;

namespace Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface
{
    // Outlines the possible interface styles to be used by the UserInterfaceHandler
    internal class UserInterface
    {
        private protected uint currentRoomID;
        private protected string baseDescription;
        private protected DescriptionHandler currentPlayersDescription;
        private protected Room? currentRoomTemporary;

        public UserInterface(Player player, GameEnvironment currentEnvironment)
        {
            currentRoomID = player.CurrentRoomID;
            baseDescription = currentPlayersDescription.GetRoomDescriptors(currentEnvironment, currentRoomID);
            currentPlayersDescription = new DescriptionHandler();
            currentRoomTemporary = currentEnvironment.FindRoomByID(currentRoomID);
        }

        public string DefaultStyle()
        {
            string userInterfaceStyle = string.Join(
                          $"\n\t{currentRoomTemporary.Name}\t -=-\t Potsun Burran\t -=-\t Relative, {currentRoomID}\t -=-\t {DateTime.UtcNow.AddYears(641)}",
                          $"\n ===========-========== ----========-========-= --..-- .",
                          $"\n{baseDescription}",
                          $"\n ====-====-===-=--=-=--_-----_--= =- -_ ._",
                          $"\n ===========",
                          $"\n\t>>> ");

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
