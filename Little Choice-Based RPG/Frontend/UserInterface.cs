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
using Little_Choice_Based_RPG.Choices;
using Little_Choice_Based_RPG.Entities.Derived.Living.Players;
using Little_Choice_Based_RPG.World.Managers;
using Little_Choice_Based_RPG.World.Rooms;

namespace Little_Choice_Based_RPG.Frontend
{
    public class UserInterface
    {
        public void GenerateUserInterface(Player player, GameEnvironment currentEnvironment)
        {
            Description currentPlayersDescription = new Description();
            string currentCoordinates = (player.Position.X.ToString() + " - " + player.Position.Y.ToString());
            uint currentRoomID = player.CurrentRoomID;
            string baseDescription = currentPlayersDescription.GetRoomDescriptor(currentEnvironment, currentRoomID);
            //string contextualDescription = Description.LastAction(player);
            string listChoices = ListChoices();
            
            Room? currentRoomTemporary = currentEnvironment.FindRoomByID(currentRoomID);
            if (currentRoomTemporary == null)
                throw new NullReferenceException("Invalid Room.");

            Console.WriteLine($"\t{currentRoomTemporary.Name}\t -=-\t Potsun Burran\t -=-\t Relative, {currentCoordinates}\t -=-\t {DateTime.UtcNow.AddYears(641)}" +
                              $"\n ===========-==========----========-========-=--..-- ." +
                              // $"\n\t{contextualDescription}" +
                              $"\n{baseDescription}" +
                              $"\n ====-====-===-=--=-=--_-----_--= =- -_ ._" +
                              $"\n {listChoices}" +
                              $"===========" +
                              $"\n\t>>> ");
            string? userInput = Console.ReadLine();

            while ( userInput == null)
            {
                userInput = Console.ReadLine();
            }

            int choiceindex = Convert.ToInt32(userInput);

            string? interactedDescriptor = CurrentChoiceHandler.Choices.ElementAt(choiceindex).OnExecute?.Invoke();

            if (interactedDescriptor == null)
                return;
            Console.WriteLine(interactedDescriptor);

            Pause();
            Console.Clear();
        }

        public string ListChoices()
        {
            string output = string.Empty;
            int currentChoiceIndex = 0;

            foreach (var choice in CurrentChoiceHandler.Choices)
            {
                output += $" {currentChoiceIndex}. /t {choice.Name}";
                currentChoiceIndex++;
            }

            return output;
        }

        public static void Pause()
        {
            Console.WriteLine("\n\tPress any key to continue...");
            Console.ReadKey();
        }

        public static string WriteDialogue(string inputText, int textDelayInMs = 40)
        {
            for (int i = 0; i < inputText.Length; i++)
            {
                if (Console.KeyAvailable) textDelayInMs = 1;
                Console.Write(inputText[i]);
                if (i % 2 == 0) Thread.Sleep(textDelayInMs);
            }
            return inputText;

        }

        public static void RemoveDialogue(string message, int sleepTime = 20)
        {
            int messageHeight = message.Split('\n').Length;

            for (int i = messageHeight; i >= 0; i--)
            {
                for (int j = message.Length - 1; j >= 0; j--)
                {
                    if (message[j] == '\n')
                    { Console.WriteLine(); }

                    if (j >= Console.BufferWidth)
                        continue;

                    Console.SetCursorPosition(j, i);
                    Console.Write(' ');
                    Thread.Sleep(sleepTime);
                }
                i--;
            }
        }

        public ChoiceHandler CurrentChoiceHandler { get; init; } = new ChoiceHandler();
    }
}
