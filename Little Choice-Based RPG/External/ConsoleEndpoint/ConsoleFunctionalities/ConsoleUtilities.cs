﻿namespace Little_Choice_Based_RPG.External.ConsoleEndpoint.ConsoleFunctionalities
{
    // All utilities for the writing, style or logic of the userInterface go here.
    internal class ConsoleUtilities
    {
        public static void Pause()
        {
            Console.WriteLine("\n\tPress any key to continue...");
            Console.ReadKey();
        }

        public static string WriteDialogue(string inputText, uint setTextDelayInMs = 0)
        {
            int textDelayInMs = (int)setTextDelayInMs;
            Console.ForegroundColor = GetColourByCode(default);

            for (int i = 0; i < inputText.Length; i++)
            {
                //if (Console.KeyAvailable) textDelayInMs = 1;

                //Change the colour when requested.
                if (inputText[i].Equals('§'))
                {
                    //Change the colour
                    if (i + 1 >= inputText.Remove(0, i).Length) //Ensure that there is more than one input character remaining, to prevent exception out of bound errors.
                    {
                        Console.ForegroundColor = GetColourByCode(inputText[i + 1]);
                        i++; //Skip the next character
                        continue; //Skip the current character
                    }
                }


                Console.Write(inputText[i]);
                if (i % 2 == 0) Thread.Sleep(textDelayInMs);
            }
            return inputText;

        }

        private static ConsoleColor GetColourByCode(char code)
        {
            var defaultColour = ConsoleColor.White;

            Dictionary<char, ConsoleColor> colourCodes = new Dictionary<char, ConsoleColor>
                {
                    { '0', ConsoleColor.Black },
                    { '1', ConsoleColor.DarkBlue },
                    { '2', ConsoleColor.DarkGreen },
                    { '3', ConsoleColor.DarkCyan },
                    { '4', ConsoleColor.DarkRed },
                    { '5', ConsoleColor.DarkMagenta },
                    { '6', ConsoleColor.DarkYellow },
                    { '7', ConsoleColor.Gray },
                    { '8', ConsoleColor.DarkGray },
                    { '9', ConsoleColor.Blue },
                    { 'a', ConsoleColor.Green },
                    { 'b', ConsoleColor.Cyan },
                    { 'c', ConsoleColor.Red },
                    { 'd', ConsoleColor.Magenta },
                    { 'e', ConsoleColor.Yellow },
                    { 'f', ConsoleColor.White },
                };

            return colourCodes.TryGetValue(code, out var newColour) ? newColour : defaultColour;
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
                if (char.IsWhiteSpace(newEntry[prospectiveCharacterIndex]) && currentWord == "")
                {
                    prospectiveCharacterIndex++; //skip the whitespace
                    continue;
                }

                //While the current word contains at max only one whitespace (after itself)
                while (!currentWord.Contains(" ") && prospectiveCharacterIndex < newEntry.Length)
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
                if (currentWord.Length + currentLine.Length > maximumLineLength)
                {
                    completedEntries.Add(currentLine); //Save the current line

                    //If the remaining characters fit on one line, add the suffix instead of the infix
                    currentLine = newEntry.Length - prospectiveCharacterIndex < maximumLineLength ? entrySuffix : entryInfix; //Reset the line
                }

                currentLine += currentWord;
                currentWord = ""; //Reset the currentWord
            }

            completedEntries.Add(currentLine); // Add the final line

            return completedEntries;
        }
    }
}
