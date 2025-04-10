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

        /// <summary> Splits a string into new lines based on a maximum line count, with a prefix, infix and suffix. </summary>
        public static List<string> SplitIntoLines(string newEntry, string entryPrefix, 
            string entryInfix, string entrySuffix, uint maximumLineLength = 120)
        {
            int prospectiveCharacterIndex = 0;
            List<string> completedEntries = new List<string>();
            string currentLine = entryPrefix;
            string currentWord = "";

            while (prospectiveCharacterIndex < newEntry.Length)
            {
                //if a whitespace does not follow a word
                if (newEntry[prospectiveCharacterIndex].Equals(" ") && currentWord.Length == 0)
                {
                    prospectiveCharacterIndex++; //skip the whitespace
                    continue;
                }

                //While the current word contains at max only one whitespace (after itself)
                while ((!currentWord.Contains(" ")) && prospectiveCharacterIndex < newEntry.Length)
                {
                    //if there's an \n then replace it with an infix)
                    if (newEntry[prospectiveCharacterIndex].Equals('\n'))
                    {
                        //Replace with an infix if its not at the start of new entry
                        if (!(completedEntries.Count == 0 && currentLine == entryPrefix))
                        {
                            currentLine += currentWord;
                            currentWord = "";

                            completedEntries.Add(currentLine);
                            currentLine = entryInfix;
                        }

                        prospectiveCharacterIndex++; //skip the new line character
                        continue;
                    }

                    currentWord += newEntry[prospectiveCharacterIndex++];
                }

                //If the new total is too long, make a new line
                if ((currentWord.Length + currentLine.Length) > maximumLineLength)
                {
                    completedEntries.Add(currentLine); //Save the current line

                    //If the remaining characters fit on one line, add the suffix instead of the infix
                    currentLine = (newEntry.Length - prospectiveCharacterIndex) < maximumLineLength ? entrySuffix : entryInfix; //Reset the line
                }

                currentLine += currentWord;
                currentWord = ""; //Reset the currentWord
            }

            completedEntries.Add(currentLine); // Add the final line

            return completedEntries;
        }
    }
}
