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
using Little_Choice_Based_RPG.Entities.Derived.Living.Players;
using Little_Choice_Based_RPG.Frontend;
using Little_Choice_Based_RPG.Types;
using Little_Choice_Based_RPG.World.Managers;

internal class Program
{
    private static void Main(string[] args)
    {
        RoomManager mainWorld = new RoomManager();
        Player currentPlayer = new Player();
        mainWorld.GenerateAllRooms();

        while (true)
        {
            currentPlayer.CurrentInterface.GenerateUserInterface(currentPlayer);
            //SanitizedString currentInput = PlayerInterface.GetInput();
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
