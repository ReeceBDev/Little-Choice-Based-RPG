using Little_Choice_Based_RPG.Managers.World;
using Little_Choice_Based_RPG.Resources.Choices;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Resources.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface.UserInterfaceHandler;

namespace Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface.UserInterfaceStyles
{
    public class ExploreStyle : IUserInterface
    {
        private protected uint currentRoomID;
        private protected string currentRoomName;
        private protected string currentRoomDescription;
        private protected ChoiceHandler currentChoiceHandler;
        private protected string currentlyDisplayedChoices;

        public ExploreStyle(ChangeInterfaceStyleCallback changeInterfaceStyle, Player player, GameEnvironment currentEnvironment, ChoiceHandler setCurrentChoiceHandler)
        {
            currentRoomID = player.Position;
            Room currentRoomPrinciple = currentEnvironment.GetRoomByID(currentRoomID);
            currentRoomName = currentRoomPrinciple.Name;
            currentRoomDescription = ConcatenateDescriptors(currentRoomPrinciple.GetRoomDescriptors());
            currentChoiceHandler = setCurrentChoiceHandler;
            currentlyDisplayedChoices = ConcatenateChoices(DisplayChoices());
        }
        public string OutputMainBody()
        {
            string userInterfaceStyle = string.Join( "\n",
                          $"{this.currentRoomName}\t -=-\t Potsun Burran\t -=-\t Relative, {currentRoomID}\t -=-\t {DateTime.UtcNow.AddYears(641)}",
                          $" ===========-========== ----========-========-= --..-- .",
                          $"{currentRoomDescription}",
                          $" ====-====-===-=--=-=--_-----_--= =- -_ ._",
                          $"{currentlyDisplayedChoices}",
                          $" ===========",
                          $">>> ");

            return userInterfaceStyle;
        }

        public string ConcatenateDescriptors(List<string> roomDescriptors)
        {
            string createdRoomDescription = "";
            foreach (string roomDescriptor in roomDescriptors)
            {
                createdRoomDescription += (roomDescriptor + " ");
            }
            return createdRoomDescription;
        }

        private List<Choice> DisplayChoices()
        {
            List<Choice> allChoices = currentChoiceHandler.GetChoices();
            return allChoices;
        }

        public string ConcatenateChoices(List<Choice> selectedChoices)
        {
            string createdChoiceList = "";

            uint choiceIndex = 1;

            foreach (Choice displayedChoices in selectedChoices)
            {
                createdChoiceList += ($"{choiceIndex++}. {displayedChoices.Name}");
            }

            return createdChoiceList;
        }
    }
}
