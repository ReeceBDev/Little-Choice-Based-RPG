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
using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface;
using Little_Choice_Based_RPG.Managers.World;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Resources.Rooms.Premade.Unique.Test;
using Little_Choice_Based_RPG.Types;
using Little_Choice_Based_RPG.Types.EntityProperty;

internal class TextBasedRPG
{
    private static void Main(string[] args)
    {
        PropertyDeclarations.InitialiseProperties();

        var mainWorld = new GameEnvironment();
        mainWorld.GenerateAllRooms();

        uint spawnRoomID = mainWorld.Rooms.ElementAt(1).Key;
        var currentPlayer = new Player("Player One", spawnRoomID, 66, 32);
        var currentUserInterfaceHandler = new UserInterfaceHandler(currentPlayer, mainWorld);

        while (true)
        {
            currentUserInterfaceHandler.GenerateOutput();
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
