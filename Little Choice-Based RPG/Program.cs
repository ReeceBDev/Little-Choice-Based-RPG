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
using Little_Choice_Based_RPG;
using Little_Choice_Based_RPG.Choices;
using Little_Choice_Based_RPG.Objects.Entities.Players;
using Little_Choice_Based_RPG.Objects.Gear.Armour.Helmets;
using Little_Choice_Based_RPG.Rooms;
using Little_Choice_Based_RPG.Types;

internal class Program
{
    private static void Main(string[] args)
    {
        GenerateRooms();
        GeneratePlayers();
        Player currentPlayer = new Player(room1);
        UserInterface currentInterface = new UserInterface();
        GenerateObjects();

        while (true)
        {
            currentInterface.GenerateUserInterface(currentPlayer);
            SanitizedString currentInput = UserInterface.GetInput();
            ChoiceHandler.Prime(currentInput);
            ChoiceHandler.InvokePrimed();
        }
    }
    /*
    currentPlayer = player1.
        GenerateUserInterface(player1);
        int index = 0;
        bool isExitingProgram = false; // cant do this because index logic is being through instead. Need a better way of doing index.

        while (!isExitingProgram)
        {
            Console.Clear();
            GetChoice(index++);
            if (!Little_Choice_Based_RPG.Choice.Logic.HandleChoice())
                index--;
        }

        Console.WriteLine("Exiting Program...");
        PlayerInterface.Pause();

    }
    */
}
