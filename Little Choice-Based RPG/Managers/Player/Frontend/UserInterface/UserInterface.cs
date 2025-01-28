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
using Little_Choice_Based_RPG.Entities.Derived.Living.Players;
using Little_Choice_Based_RPG.Managers.Player.Frontend.Description;
using Little_Choice_Based_RPG.Managers.World;
using Little_Choice_Based_RPG.Resources.Choices;
using Little_Choice_Based_RPG.Resources.Entities.Derived.Living.Players;
using Little_Choice_Based_RPG.Resources.Rooms;

namespace Little_Choice_Based_RPG.Managers.Player.Frontend.UserInterface
{
    internal class UserInterface
    {

        private public string currentCoordinates;
        private public uint currentRoomID;
        private public string baseDescription;
        private protected DescriptionHandler currentPlayersDescription;
        private protected Room? currentRoomTemporary;

        public void UserInterface(Player player, GameEnvironment currentEnvironment)
        {
            currentCoordinates = player.Position.X.ToString() + " - " + player.Position.Y.ToString();
            currentRoomID = player.CurrentRoomID;
            baseDescription = currentPlayersDescription.GetRoomDescriptors(currentEnvironment, currentRoomID);
            currentPlayersDescription = new DescriptionHandler();
            currentRoomTemporary = currentEnvironment.FindRoomByID(currentRoomID);
            if (currentRoomTemporary == null)
                throw new UserInterface("Invalid Room.");
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
