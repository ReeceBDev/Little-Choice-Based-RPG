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
using Little_Choice_Based_RPG.Objects.Gear.Armour.Helmets;
using Little_Choice_Based_RPG.Story;

internal class Program
{
        static void GetChoice(int index = 0)
    {
        if (index == 0) { Introduction.Begin(); PlayerInterface.RemoveDialogue(Introduction.currentDialogue); }
        else if (index == 1) { Introduction.BeginSecond(); }
        else if (index == 2) { Introduction.BeginThird(); }
    }
    private static void Main(string[] args)
    {
        
        Room testRoom = new Room("bean", "bean", "", "");
        Room test2Room = new Room("bean", "bean", "", "");
        Room test3Room = new Room("bean", "bean", "", "");
        Player player1 = new Player();
        player1.PlayerCanHear = true;
        DavodianMkI helmet1 = new DavodianMkI();

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
}
