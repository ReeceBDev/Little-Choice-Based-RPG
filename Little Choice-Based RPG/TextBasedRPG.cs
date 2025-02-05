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
using Little_Choice_Based_RPG.Managers.PlayerManager.Frontend.UserInterface;
using Little_Choice_Based_RPG.Managers.World;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Types;

internal class TextBasedRPG
{
    private static void Main(string[] args)
    {
        var mainWorld = new GameEnvironment();
        var DefaultSpawnPosition = new Vector2 (0, 0);
        var currentPlayer = new Player(DefaultSpawnPosition);

        mainWorld.GenerateAllRooms();

        //currentPlayer.CurrentRoomID = mainWorld.Rooms.ElementAt(0).ID;

        currentPlayer.UserInterface.GenerateInterface();
        while (true)
        {
            currentPlayer.CurrentInterface.HandleUserInterface(currentPlayer, mainWorld);
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
