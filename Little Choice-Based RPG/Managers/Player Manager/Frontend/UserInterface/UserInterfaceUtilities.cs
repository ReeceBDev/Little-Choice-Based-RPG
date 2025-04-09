using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface
{
    // All utilities for the writing, style or logic of the userInterface go here.
    internal class UserInterfaceUtilities
    {
        public static void Pause()
        {
            Console.WriteLine("\n\tPress any key to continue...");
            Console.ReadKey();
        }

        public static string WriteDialogue(string inputText, uint setTextDelayInMs = 0)
        {
            int textDelayInMs = (int) setTextDelayInMs;

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
    }
}
