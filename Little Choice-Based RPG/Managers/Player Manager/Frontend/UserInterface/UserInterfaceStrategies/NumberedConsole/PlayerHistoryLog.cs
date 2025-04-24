using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface.UserInterfaceStrategies.NumberedConsole
{
    public class PlayerHistoryLog
    {
        public Stack<string> HistoryLog { get; set; } = new Stack<string>();

        public void AddNewHistoryLog(string newHistoryEntry)
        {
            var logTime = DateTime.UtcNow.AddYears(641);

            string entryPrefix = "\n ╓ ";
            string entryInfix = "\n ║ ";
            string entrySuffix = "\n ╙ ";

            string logPrefix = $"<>-<>-< {logTime} >-<>-<>\n";
            string newInformationalHistoryEntry = logPrefix + newHistoryEntry;
            string newLogConcatenated = entryInfix; //Leaves space before new value

            List<string> newLogLines = UserInterfaceUtilities.SplitIntoLines(newInformationalHistoryEntry, entryPrefix, entryInfix, entrySuffix);

            //Turn the new lines into a concatenated string
            foreach (string line in newLogLines)
                newLogConcatenated += line;

            HistoryLog.Push(newInformationalHistoryEntry); // Add new content to the history log.
        }

        public string GetHistoryLog(int historyLineMaxCount) //Should display bottom to top :)
        {
            int logAgeIndex = HistoryLog.Count(); //Older is higher

            string historyLogOutput = "";

            string entryPrefix = "\n ╓ ";
            string entryInfix = "\n ║ ";
            string entrySuffix = "\n ╙ ";

            List<string> historyLogLines = new List<string>();
            List<string> orderedLogLines = new List<string>();
            List<KeyValuePair<int, string>> savedLogLines = new List<KeyValuePair<int, string>>();
            List<KeyValuePair<int, string>> splitLogLines = new List<KeyValuePair<int, string>>();
            Stack<string> holdingStack = new Stack<string>();

            uint iteration = 0;

            //Create prefix title
            historyLogLines.Add("\n ╔══════════════════╤════════════════════════════════════════════════════════════════" + "══════-=════-=═=-=--=-=-- - - -");
            historyLogLines.Add("\n ║  Historical Log  │ ");
            historyLogLines.Add("\n ╙──────────────────┘ ");

            //Grabs entries and puts them at the bottom of a new stack.
            while (HistoryLog.Count > 0 && historyLineMaxCount > iteration)
                holdingStack.Push(HistoryLog.Pop());

            // Returns entries back to their stack.
            while (holdingStack.Count > 0)
            {
                string poppedLog = holdingStack.Pop();

                var newSplitLines = UserInterfaceUtilities.SplitIntoLines(poppedLog, entryPrefix, entryInfix, entrySuffix);

                foreach (string line in newSplitLines)
                    splitLogLines.Add(new KeyValuePair<int, string>(logAgeIndex, line));

                // Add a blank space between entries
                if (holdingStack.Count != 0) //If not the last historyLog
                    splitLogLines.Add(new KeyValuePair<int, string>(logAgeIndex, entryInfix));

                HistoryLog.Push(poppedLog);
                logAgeIndex--; //Reduce the age for each iteration, as each additional iteration is younger
            }

            //Ascend the logIndex and retain groups of entries until the max line count is reached.
            for (logAgeIndex = 1; logAgeIndex <= HistoryLog.Count(); logAgeIndex++)
            {
                List<string> newLines = new List<string>();

                //Grab the entries for the current index
                foreach (var entry in splitLogLines)
                {
                    if (entry.Key == logAgeIndex)
                        newLines.Add(entry.Value);
                }

                //If the additional lines don't exceed the line limit, add them.
                if ((savedLogLines.Count() + newLines.Count()) <= historyLineMaxCount)
                    foreach (var line in newLines)
                        savedLogLines.Add(new KeyValuePair<int, string>(logAgeIndex, line));
                else
                    break; //Once it will exceed, stop
            }

            //Put the groups of logs in reverse order
            for (logAgeIndex--; logAgeIndex > 0; logAgeIndex--) //Revert logAgeIndex to the index of the final saved entry
            {
                //Add the entries for the current index
                foreach (var entry in splitLogLines)
                {
                    if (entry.Key == logAgeIndex)
                        orderedLogLines.Add(entry.Value);
                }
            }

            // Insert whitespace to pad the number of line up until the maximum history line count.
            while ((historyLogLines.Count() + orderedLogLines.Count()) < historyLineMaxCount)
                historyLogLines.Add(entryInfix);

            //Add the ordered lines to the padded history lsog
            historyLogLines.AddRange(orderedLogLines);

            //Concatenate into a string
            foreach (string line in historyLogLines)
                historyLogOutput += line;

            return historyLogOutput;

        }
    }
}
