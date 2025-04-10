using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface.UserInterfaceStrategies.NumberedConsole;
using Little_Choice_Based_RPG.Managers.World;
using Little_Choice_Based_RPG.Resources;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Immaterial.System;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.Interaction;
using Little_Choice_Based_RPG.Types.Interactions.InteractDelegate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface.UserInterfaceHandler;
using static Little_Choice_Based_RPG.Types.Interactions.InteractDelegate.InteractionUsingNothing;

namespace Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface.UserInterfaceStyles
{
    public class ExploreMenu : IUserInterface
    {
        private protected uint currentRoomID;
        private protected Room currentRoom;
        private protected Player currentPlayer;

        private List<IInvokableInteraction> systemChoices;
        private List<IInvokableInteraction> subMenuSystemChoices;

        private bool exitExploreMenu;
        private bool subMenuActive = false;

        private List<ExploreMenuTextComponent> mainBodyText = new List<ExploreMenuTextComponent>();
        private List<ExploreMenuTextComponent> subMenuText = new List<ExploreMenuTextComponent>();

        private Stack<string> historyLog = new Stack<string>();
        private int historyLineMaxCount = 20;
        private string transitionalAction = "";
        private readonly string defaultTransitionalAction;
        private List<IInvokableInteraction> relevantInteractions = new List<IInvokableInteraction>();

        private int choiceIndexOffset = 1;


        private protected ChangeInterfaceStyleCallback changeInterfaceStyleCallback;
        public ExploreMenu(ChangeInterfaceStyleCallback changeInterfaceStyle, Player player, GameEnvironment currentEnvironment)
        {
            exitExploreMenu = false;

            currentPlayer = player;
            currentRoomID = (uint)player.Properties.GetPropertyValue("Position");
            currentRoom = currentEnvironment.GetRoomByID(currentRoomID);

            systemChoices = InitialiseSystemChoices();
            subMenuSystemChoices = InitialiseSubMenuSystemChoices();

            defaultTransitionalAction = $"A tsunami of a thousand glass-like reflections tear open reality with a roar. \nWhen they close, you are left standing in their place.";
            transitionalAction = defaultTransitionalAction;
        }

        private List<IInvokableInteraction> InitialiseSystemChoices()
        {
            List<IInvokableInteraction> allCurrentChoices = new List<IInvokableInteraction>();

            //Initialise a System PropertyContainer for filling out the delegate to make it compatible with the Interaction system.
            PropertyContainer emptySystemContainer = new UserInterfaceContainer();

            InteractionUsingNothingDelegate returnToMainMenu = new InteractionUsingNothingDelegate(ReturnToMainMenu);
            allCurrentChoices.Add(new InteractionUsingNothing(returnToMainMenu, emptySystemContainer, "Go back to main menu.", "Returning to main menu...", InteractionRole.System));

            return allCurrentChoices;
        }

        private List<IInvokableInteraction> InitialiseSubMenuSystemChoices()
        {
            List<IInvokableInteraction> allCurrentChoices = new List<IInvokableInteraction>();

            //Add permanent sub-menu choices here.

            return allCurrentChoices;
        }

        public void RunMenu()
        {
            while (!exitExploreMenu)
            {
                List<IInvokableInteraction> listedInteractions = GetExploreMenuInteractions();

                //If its the first time this menu has been initialised, then intialise, otherwise update the main body.
                if (mainBodyText.Count == 0)
                    InitialiseMainTextEntries();
                else
                    UpdateMainTextEntries();

                DrawUserInterface();

                int userInput = AwaitUserInput(listedInteractions.Count);
                InvokeInteraction(userInput, listedInteractions);
            }
        }

        /// <summary> Draw the User Interface. This writes all assigned text entries to the interface </summary>
        private void DrawUserInterface()
        {
            Console.Clear();

            if (subMenuActive)
                WriteToUserInterface(subMenuText);
            else
                WriteToUserInterface(mainBodyText);
        }

        /// <summary> Generates a Sub-Menu asking the player to choose an object from the room, which optionally matches property filters. </summary>
        private void GenerateSubMenu(IInvokableInteraction sender, string requirementDescription, IInvokableInteraction abortInteraction, List<EntityProperty>? setFilters = null)
        {
            List<GameObject> possibleObjects = currentRoom.GetRoomObjects(setFilters);
            List<GameObject> listedObjects = new List<GameObject>();

            subMenuSystemChoices.Add(abortInteraction);

            //Update the submenu text cache, or initiliase it if its not yet been set.
            if (subMenuText.Count() == 0)
            {
                List<ExploreMenuTextComponent> listToWrite = InitialiseSubMenuTextEntries(mainBodyText, requirementDescription, listedObjects);
                this.subMenuText = listToWrite;
            }
            else
            {
               //Update the cached output with the new content.
                ExploreMenuTextComponent targetComponent = GetTextEntryByIdentifier(ExploreMenuIdentity.SubMenuTitle);
                int targetIndex = subMenuText.IndexOf(targetComponent);

                this.subMenuText[targetIndex].Content = SetSubMenuHeader(requirementDescription);

                targetComponent = GetTextEntryByIdentifier(ExploreMenuIdentity.SubMenuOptions);
                targetIndex = subMenuText.IndexOf(targetComponent);

                this.subMenuText[targetIndex].Content = FormatSubMenuChoices(listedObjects);
            }

            //Draw the SubMenu
            this.subMenuActive = true;
            DrawUserInterface();

            //Await the user's answer.
            int userInput = AwaitUserInput(listedObjects.Count + subMenuSystemChoices.Count);

            if (userInput <= (choiceIndexOffset + (listedObjects.Count - 1)))
            {
                GameObject chosenObject = listedObjects.ElementAt(userInput - choiceIndexOffset);

                sender.GiveRequiredParameter(chosenObject, this);
                
                //Reset the additional SubMenu choice
                subMenuSystemChoices.Remove(abortInteraction);
            }
            
            if (userInput > (choiceIndexOffset + (listedObjects.Count - 1)))
            {

                subMenuSystemChoices.ElementAt(userInput - (listedObjects.Count + choiceIndexOffset));
                abortInteraction.AttemptInvoke(this);

                //Reset the additional SubMenu choice
                subMenuSystemChoices.Remove(abortInteraction);
            }

            //Deactivate the SubMenu
            this.subMenuActive = false;
        }

        private List<IInvokableInteraction> GetExploreMenuInteractions()
        {
            List<IInvokableInteraction> allInteractions = new List<IInvokableInteraction>();

            allInteractions.AddRange(GetAvailableChoices(InteractionRole.Explore));
            allInteractions.AddRange(GetAvailableChoices(InteractionRole.Navigation));
            allInteractions.AddRange(GetAvailableChoices(InteractionRole.System));

            return allInteractions;
        }

        private int AwaitUserInput(int numberOfInteractions)
        {
            string? userInput = null;
            string errorMessage = "Unknown error.";
            bool isInputValid = false;
            bool sendErrorMessage = false;
            int userSelection = -1;

            int minimumInputValue = choiceIndexOffset;
            int maximumInputValue = choiceIndexOffset + numberOfInteractions - 1;

            //Loop until a choice is selected.
            while (isInputValid == false)
            {
                //Send error message or write normal input.
                if (sendErrorMessage == false)
                    WriteUserEntry();
                else
                    WriteUserError(errorMessage);
                
                //Reset error message flag.
                sendErrorMessage = false;

                //Await user input
                userInput = Console.ReadLine();

                //Validate user input
                if (userInput == "") //input must not be empty
                {
                    sendErrorMessage = true;
                    errorMessage = "Please enter the number of your choice. Pressing enter will do nothing on its own!";
                    continue;
                }

                if (userInput.All(Char.IsDigit)) //input must be a number
                    userSelection = Convert.ToInt32(userInput);
                else
                {
                    sendErrorMessage = true;
                    errorMessage = "Please enter a number and nothing else. Trying to enter other things won't have any use!";
                    continue;
                }

                if (minimumInputValue <= userSelection && userSelection <= maximumInputValue) //input must match the range of choices.
                    isInputValid = true; //Break
                else
                {
                    sendErrorMessage = true;
                    errorMessage = "Please choose a number from the available choices. Trying a number not listed will have nothing to do!";
                }

            }

            return userSelection;
        }

        private void WriteUserEntry()
        {
            UserInterfaceUtilities.WriteDialogue("\n ║\n ║\n ╚", 160);
            UserInterfaceUtilities.WriteDialogue("═>> Choose an option: > ", 5);
        }

        private void WriteUserError(string message)
        {
            Console.Clear();
            DrawUserInterface();

            UserInterfaceUtilities.WriteDialogue("\n ╠", 160);
            UserInterfaceUtilities.WriteDialogue(" Hang on: ", 0);
            UserInterfaceUtilities.WriteDialogue("\n ╠", 160);
            UserInterfaceUtilities.WriteDialogue($" {message}", 4);
            UserInterfaceUtilities.WriteDialogue("\n ╚", 160);
            UserInterfaceUtilities.WriteDialogue("═>> Choose an option: > ", 5);
        }

        private void InvokeInteraction(int userInput, List<IInvokableInteraction> listedInteractions)
        {
            IInvokableInteraction selectedInteraction = listedInteractions.ElementAt(userInput - choiceIndexOffset);

            //Invoke!
            selectedInteraction.AttemptInvoke(this);

            //Record the action in the log.
            AddNewHistoryLog(selectedInteraction.InteractDescriptor);

            //Record the action in the TransitionalAction if the location around the player changed.
            if ((selectedInteraction.InteractionContext == InteractionRole.Navigation))
            {
                transitionalAction = selectedInteraction.InteractDescriptor;
                UpdateTransitionalAction();
            }
        }

        //Record the action in the TransitionalAction if the location around the player changed.
        private void OnEventRoomDescriptorChanged(Interaction trigger)
        {
            transitionalAction = trigger.InteractDescriptor;
            UpdateTransitionalAction();
        }

        public void InitialiseMainTextEntries()
        {
            List<ExploreMenuTextComponent> exploreMenu = new List<ExploreMenuTextComponent>
            {
            new ExploreMenuTextComponent(ExploreMenuIdentity.TopStatusBar, GetTopStatusBar(), 0),
            new ExploreMenuTextComponent(ExploreMenuIdentity.TransitionalAction, GetTransitionalAction(), 0),
            new ExploreMenuTextComponent(ExploreMenuIdentity.CurrentDescription, GetCurrentDescription(), 0),
            new ExploreMenuTextComponent(ExploreMenuIdentity.HistoryLog, GetHistoryLog(), 0),
            new ExploreMenuTextComponent(ExploreMenuIdentity.AvailableChoices, FormatChoices(GetExploreMenuInteractions()), 0),
            new ExploreMenuTextComponent(ExploreMenuIdentity.None, GetStyleLine(4), 0),
            };

            mainBodyText = exploreMenu;
        }

        //Update the cached output with the new content.
        public void UpdateMainTextEntries()
        {
            ExploreMenuTextComponent targetEntry = GetTextEntryByIdentifier(ExploreMenuIdentity.TopStatusBar);
            int targetIndex = mainBodyText.IndexOf(targetEntry);
            this.mainBodyText[targetIndex].Content = GetTopStatusBar();

            targetEntry = GetTextEntryByIdentifier(ExploreMenuIdentity.TransitionalAction);
            targetIndex = mainBodyText.IndexOf(targetEntry);
            this.mainBodyText[targetIndex].Content = GetTransitionalAction();

            targetEntry = GetTextEntryByIdentifier(ExploreMenuIdentity.CurrentDescription);
            targetIndex = mainBodyText.IndexOf(targetEntry);
            this.mainBodyText[targetIndex].Content = GetCurrentDescription();

            targetEntry = GetTextEntryByIdentifier(ExploreMenuIdentity.HistoryLog);
            targetIndex = mainBodyText.IndexOf(targetEntry);
            this.mainBodyText[targetIndex].Content = GetHistoryLog();

            targetEntry = GetTextEntryByIdentifier(ExploreMenuIdentity.AvailableChoices);
            targetIndex = mainBodyText.IndexOf(targetEntry);
            this.mainBodyText[targetIndex].Content = FormatChoices(GetExploreMenuInteractions());
        }

        private List<ExploreMenuTextComponent> InitialiseSubMenuTextEntries(List<ExploreMenuTextComponent> overwrittenInterfaceEntries, string requirementDescription, List<GameObject> listedObjects)
        {
            if (!(overwrittenInterfaceEntries.Count < 1))
                throw new Exception($"The required list of overwrittenInterfaceEntries does not contain enough lines to be used here. It might not have been initialised. There must be an existing user interface to put the sub-menu over the top of!");

            List<ExploreMenuTextComponent> subMenu = new List<ExploreMenuTextComponent>(overwrittenInterfaceEntries.SkipLast(3));

            subMenu.Append(new ExploreMenuTextComponent(ExploreMenuIdentity.SubMenuTitle, SetSubMenuHeader(requirementDescription), 0));
            subMenu.Append(new ExploreMenuTextComponent(ExploreMenuIdentity.SubMenuOptions, FormatSubMenuChoices(listedObjects), 0));
            subMenu.Append(new ExploreMenuTextComponent(ExploreMenuIdentity.None, FormatChoices(subMenuSystemChoices), 0));

            subMenu.Append(new ExploreMenuTextComponent(ExploreMenuIdentity.None, GetStyleLine(4), 0));
            subMenu.Append(new ExploreMenuTextComponent(ExploreMenuIdentity.None, GetStyleLine(5), 0));

            return subMenu;
        }

        private string SetSubMenuHeader(string title)
        {
            string subMenuHeader = "";

            subMenuHeader += $"\n == +] ======][========][======= MAKE A SELECTION =======][======][=======. =- = - -[ =  - .";
            subMenuHeader += $"\n  . - = -- - - ===========-===========================--======-=---=-. ---=== =-----. -  - .";
            subMenuHeader += $"\n                          {title}";
            subMenuHeader += $"\n  .- =  --  - =-==--==--  --==--  --===-==- -=====-=-  -======-=---=-. ---=== =--=--. -  - .";

            return subMenuHeader;
        }

        private ExploreMenuTextComponent GetTextEntryByIdentifier(ExploreMenuIdentity targetIdentifier)
        {
            ExploreMenuTextComponent? targetEntry = null;

            foreach (ExploreMenuTextComponent entry in mainBodyText)
            {
                if (entry.UniqueIdentity == targetIdentifier)
                    return entry;
            }

            if (targetEntry == null)
                throw new ArgumentException($"The TopStatusBar didnt seem to exist. No UserInterfaceTextEntry within {mainBodyText} had the identifier {ExploreMenuIdentity.TopStatusBar}!");

            throw new Exception($"Critical failure! This method didn't complete! This shouldn't have happened. It looks like this method somehow didn't complete any other path, but it should have!");
        }

        private string GetTopStatusBar()
        {
            string topStatusPrefix = " ╔══════════════════════════════════════════════════════════════════════════════════" + "══════-=════-=═=-=--=-=-- - - -";
            string topStatusInfix = "\n ║ ";
            string topStatusBar = $"{currentRoom.Name}\t -=-\t Potsun Burran\t -=-\t Relative, {currentRoomID}\t -=-\t {DateTime.UtcNow.AddYears(641)}";

            string concatenatedStatusBar = topStatusPrefix + topStatusInfix + topStatusBar;

            return concatenatedStatusBar;
        }

        private string GetCurrentDescription()
        {
            string newDescriptionContent = ConcatenateDescriptors(currentRoom.GetRoomDescriptors());
            List<string> roomLines = UserInterfaceUtilities.SplitIntoLines(newDescriptionContent, "\n ╟ ", "\n ║ ", "\n ╟ ");

            string descriptionPrefix =
                    "\n ║ Room Description │ " +
                    "\n ╙──────────────────┘ ";

            string finalRoomDescription = descriptionPrefix;

            foreach (string line in roomLines)
            {
                finalRoomDescription += line;
            }


            return finalRoomDescription;
        }

        private string GetHistoryLog() //Should display bottom to top :)
        {
            int logAgeIndex = historyLog.Count(); //Older is higher

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
            while (historyLog.Count > 0 && historyLineMaxCount > iteration)
                holdingStack.Push(historyLog.Pop());

            // Returns entries back to their stack.
            while (holdingStack.Count > 0)
            {
                string poppedLog = holdingStack.Pop();

                var newSplitLines = UserInterfaceUtilities.SplitIntoLines(poppedLog, entryPrefix, entryInfix, entrySuffix);

                foreach ( string line in newSplitLines )
                    splitLogLines.Add(new KeyValuePair<int, string>(logAgeIndex, line));

                // Add a blank space between entries
                if (holdingStack.Count != 0) //If not the last historyLog
                    splitLogLines.Add(new KeyValuePair<int, string>(logAgeIndex, entryInfix));

                historyLog.Push(poppedLog);
                logAgeIndex--; //Reduce the age for each iteration, as each additional iteration is younger
            }

            //Ascend the logIndex and retain groups of entries until the max line count is reached.
            for (logAgeIndex = 1; logAgeIndex <= historyLog.Count(); logAgeIndex++)
            {
                List<string> newLines = new List<string>();

                //Grab the entries for the current index
                foreach(var entry in splitLogLines)
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
            for (logAgeIndex-- ; logAgeIndex > 0; logAgeIndex--) //Revert logAgeIndex to the index of the final saved entry
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

        private void UpdateHistoryLog()
        {
            ExploreMenuTextComponent targetComponent = GetTextEntryByIdentifier(ExploreMenuIdentity.HistoryLog);
            int targetIndex = mainBodyText.IndexOf(targetComponent);

            //Update the cached output with the new content, at a slow write speed.
            this.mainBodyText[targetIndex].Content = GetHistoryLog();

            //Write to the interface
            DrawUserInterface();
        }

        private void UpdateTopStatusBar()
        {
            ExploreMenuTextComponent targetEntry = GetTextEntryByIdentifier(ExploreMenuIdentity.TopStatusBar);
            int targetIndex = mainBodyText.IndexOf(targetEntry);

            //Update the cached output with the new content, at a slow write speed.
            this.mainBodyText[targetIndex].Content = GetTopStatusBar();
            this.mainBodyText[targetIndex].WriteSpeed = 40;

            //Write to the interface
            DrawUserInterface();

            //Reset text write speed to instant
            this.mainBodyText[targetIndex].WriteSpeed = 0;
        }

        private void UpdateCurrentDescription()
        {
            ExploreMenuTextComponent targetEntry = GetTextEntryByIdentifier(ExploreMenuIdentity.CurrentDescription);
            int targetIndex = mainBodyText.IndexOf(targetEntry);

            //Update the cached output with the new content, at a slow write speed.
            this.mainBodyText[targetIndex].Content = GetCurrentDescription();
            this.mainBodyText[targetIndex].WriteSpeed = 40;

            //Write to the interface
            DrawUserInterface();

            //Reset text write speed to instant
            this.mainBodyText[targetIndex].WriteSpeed = 0;
        }

        private void UpdateTransitionalAction()
        {
            ExploreMenuTextComponent targetEntry = GetTextEntryByIdentifier(ExploreMenuIdentity.TransitionalAction);
            int targetIndex = mainBodyText.IndexOf(targetEntry);

            //Update the cached output with the new content, at a slow write speed.
            this.mainBodyText[targetIndex].Content = transitionalAction;
            this.mainBodyText[targetIndex].WriteSpeed = 40;

            //Write to the interface
            DrawUserInterface();

            //Reset text write speed to instant
            this.mainBodyText[targetIndex].WriteSpeed = 0;
        }

        private void UpdateAvailableChoices() //THIS WONT WORK, because choices come in different types and so on. This should probably just add new choices and remove old choices upon event call (of what?)
        {
            /*
            ExploreMenuTextComponent targetEntry = GetTextEntryByIdentifier(ExploreMenuIdentity.AvailableChoices);
            int targetIndex = Array.IndexOf(mainBodyText, targetEntry);

            mainBodyText[targetIndex] = new ExploreMenuTextComponent(ExploreMenuIdentity.AvailableChoices, GetAvailableChoices(), 40);

            DrawUserInterface();
            
            this.mainBodyText[targetIndex].WriteSpeed = 0;
            */
        }

        private void AddNewHistoryLog(string newHistoryEntry)
        {
            var logTime = DateTime.UtcNow.AddYears(641);

            string entryPrefix = "\n ╓ ";
            string entryInfix = "\n ║ ";
            string entrySuffix = "\n ╙ ";

            string logPrefix = $"<>-<>-< {logTime} >-<>-<>\n";
            string newInformationalHistoryEntry = logPrefix + newHistoryEntry;
            string newLogConcatenated = entryInfix; //Leaves space before new value

            ExploreMenuTextComponent targetComponent = GetTextEntryByIdentifier(ExploreMenuIdentity.HistoryLog);
            int targetIndex = mainBodyText.IndexOf(targetComponent);

            List<string> newLogLines = UserInterfaceUtilities.SplitIntoLines(newInformationalHistoryEntry, entryPrefix, entryInfix, entrySuffix);

            //Update the old content
            historyLineMaxCount -= newLogLines.Count + 1;
            mainBodyText.ElementAt(targetIndex).Content = GetHistoryLog();
            historyLineMaxCount += newLogLines.Count + 1;

            //Turn the new lines into a concatenated string
            foreach (string line in newLogLines)
                newLogConcatenated += line;

            //Slowly write the new content
            this.mainBodyText.Insert(targetIndex + 1, new ExploreMenuTextComponent(ExploreMenuIdentity.None, newLogConcatenated, 40));

            //Draw the userinterface
            DrawUserInterface();

            //Return the mainBodyText to normal
            this.historyLog.Push(newInformationalHistoryEntry); // Add new content to the history log.
            this.mainBodyText.RemoveAt(targetIndex + 1); //Remove the extra mainBodyEntry
        }

        private void ClearHistoryLog()
        {
            historyLog.Clear();
            UpdateHistoryLog();
        }

        private List<IInvokableInteraction> GetAvailableChoices(InteractionRole ofInteractionRole)
        {
            List<IInvokableInteraction> allCurrentChoices = new List<IInvokableInteraction>();

            //If requesting system, initialise system choices specific to here:
            if (ofInteractionRole == InteractionRole.System)
                allCurrentChoices = systemChoices;
            else
                //Initialise choices from the Room of the given InteractionRole:
                allCurrentChoices = InteractionController.GetInteractions(ofInteractionRole, currentRoom);

            return allCurrentChoices;
        }

        private string GetStyleLine(uint style)
        {
            switch (style)
            {
                case 1:
                    return
                        ("\n ╠══════════════════════════════════════════════════════════════════════════════════ " + "-"
                        );
                case 2:
                    return "\n ╟────────────────────────────────────────────────────────────────────────────────" + "-";
                case 3:
                    return " ══════===═══=";
                case 4:
                    return "═══════-=════-=═=-=--=-=-- - - -";
                case 5:
                    return " >> > > ";
                case 6:
                    return "\n ╟──";

                default:
                    throw new ArgumentException($"No switch case for the style matching {style}");
            }
        }

        private string GetTransitionalAction()
        {
            int longestLineCount = 0;

            const int topPaddingInset = 1;
            const int bottomPaddingInset = 8;
            const int bottomLength = 84;

            string concatenatedTop = "";
            string concatenatedMid = "";
            string concatenatedBottom = "";

            string transitionalTopPrefix = "\n ╟";
            string transitionalTopInfix = "─";
            string transitionalTopSuffix = "╖";

            string transitionalMidPrefix = "\n ╠├ ";
            string transitionalMidInfix = " ";
            string transitionalMidSuffix = "┤╣";

            string transitionalBottomPrefix = "\n ╠══════════════════╤";
            string transitionalBottomInfix = "═";
            string transitionalBottomMark = "╩";
            string transitionalBottomSuffix = "═";

            string topPadding = "";
            string middlePadding = "";
            string bottomPadding = "";
            string bottomEnding = "";
            string bottomEndingSuffix = "══════-=════-=═=-=--=-=-- - - -";


            List<string> transitionalActionLines = UserInterfaceUtilities.SplitIntoLines(transitionalAction, transitionalMidPrefix, transitionalMidPrefix, transitionalMidPrefix, 70);

            foreach (string line in transitionalActionLines)
            {
                if (longestLineCount < line.Count())
                    longestLineCount = line.Count() - transitionalMidPrefix.Count();
            }
            /*
            //bottomPadding
            for (int i = transitionalBottomPrefix.Count(); i <= (longestLineCount); i++)
                bottomPadding += transitionalBottomInfix;

            //bottomEnding
            for (int i = 0; i <= (bottomLength - (transitionalBottomPrefix.Count() + bottomPadding.Count() + transitionalBottomMark.Count())); i++)
                bottomEnding += transitionalBottomSuffix;

            foreach (string transitionalActionContent in transitionalActionLines)
            {
                middlePadding = "";

                //middlePadding
                for (int i = 1; i <= (longestLineCount) - (transitionalActionContent.Count()); i++)
                    middlePadding += transitionalMidInfix;

                if ((middlePadding.Count() + transitionalMidPrefix.Count()) < (transitionalBottomPrefix.Count() + bottomPadding.Count()))
                    for (int i = middlePadding.Count(); i <= (transitionalBottomPrefix.Count() - transitionalActionContent.Count() - 2); i++)
                        middlePadding += transitionalMidInfix;

                if ((middlePadding.Count() == 0))
                {
                    topPadding += transitionalTopInfix;
                    middlePadding += transitionalMidInfix;
                    bottomPadding += transitionalBottomInfix;
                }

                concatenatedMid += transitionalActionContent + middlePadding + transitionalMidSuffix;
            }

            //topPadding
            for (int i = (0); i <= (longestLineCount - (transitionalMidSuffix.Count()) - 1); i++)
                topPadding += transitionalTopInfix;
            */


            //bottomPadding
            for (int i = 0; i <= 9 + (longestLineCount - transitionalBottomPrefix.Count()); i++)
                bottomPadding += transitionalBottomInfix;

            //bottomEnding
            for (int i = 0; i <= (bottomLength - (transitionalBottomPrefix.Count() + bottomPadding.Count() + transitionalBottomMark.Count())); i++)
                bottomEnding += transitionalBottomSuffix;

            foreach (string transitionalActionContent in transitionalActionLines)
            {
                middlePadding = "";


                //middlePadding
                while ((transitionalActionContent.Count() + middlePadding.Count()) < (bottomLength - transitionalBottomMark.Count() - bottomEnding.Count()))
                    middlePadding += transitionalMidInfix;
                //for (int i = 0; i <= (longestLineCount) - ((transitionalActionContent.Count()-transitionalMidPrefix.Count())); i++)
                  //  middlePadding += transitionalMidInfix;

                /*
                if ((middlePadding.Count() + transitionalMidPrefix.Count()) < (transitionalBottomPrefix.Count() + bottomPadding.Count()))
                    for (int i = middlePadding.Count(); i <= (transitionalBottomPrefix.Count() - transitionalActionContent.Count() - 2); i++)
                        middlePadding += transitionalMidInfix;
                */

                /*
                if ((middlePadding.Count() == 0))
                {
                    topPadding += transitionalTopInfix;
                    middlePadding += transitionalMidInfix;
                    bottomPadding += transitionalBottomInfix;
                }
                */

                concatenatedMid += transitionalActionContent + middlePadding + transitionalMidSuffix;
            }

            //topPadding
            while ((transitionalTopPrefix.Count() + topPadding.Count() - transitionalTopSuffix.Count()) < (bottomLength - transitionalBottomMark.Count() - bottomEnding.Count()))
                topPadding += transitionalTopInfix;
            /*
            for (int i = 0; i <= 1 + (transitionalMidPrefix.Count() + longestLineCount); i++)
                topPadding += transitionalTopInfix;*/



            concatenatedTop = transitionalTopPrefix + topPadding + transitionalTopSuffix;
            concatenatedBottom = transitionalBottomPrefix + bottomPadding + transitionalBottomMark + bottomEnding + bottomEndingSuffix;

            return concatenatedTop + concatenatedMid + concatenatedBottom;
        }

        private string ConcatenateDescriptors(List<string> roomDescriptors)
        {
            string createdRoomDescription = "";
            foreach (string roomDescriptor in roomDescriptors)
            {
                createdRoomDescription += (roomDescriptor + " ");
            }
            return createdRoomDescription;
        }

        private void WriteToUserInterface(List<ExploreMenuTextComponent> listToWrite)
        {
            foreach (ExploreMenuTextComponent entryToWrite in listToWrite)
            {
                UserInterfaceUtilities.WriteDialogue(entryToWrite.Content, entryToWrite.WriteSpeed);
            }
        }

        private string FormatChoices(List<IInvokableInteraction> possibleInteractions)
        {
            string createdChoiceList = "";
            int choiceIndex = 0;

            //Prefix
            createdChoiceList += ("\n ╔════════════════════════════════════════════════════════════════════════════════════════-=════-=═=-=--=-=-- - - -");

            //Main choice list
            while (choiceIndex < (possibleInteractions.Count))
            {
                createdChoiceList += ($"\n ╠ {choiceIndex + choiceIndexOffset} » {possibleInteractions.ElementAt(choiceIndex).InteractionTitle}");
                choiceIndex++;
            }

            //Suffix
            createdChoiceList += ("\n ╠═════════════════════════════════════════════════════════════════════════════════");

            return createdChoiceList;
        }

        private string FormatSubMenuChoices(List<GameObject> listedObjects)
        {

            string formattedObjectList = "";
            int choiceIndex = 0;

            while (choiceIndex < (listedObjects.Count))
                formattedObjectList += ($"\n ╠ {choiceIndex++ + choiceIndexOffset} » {listedObjects.ElementAt(choiceIndex).Properties.GetPropertyValue("Name")} - {listedObjects.ElementAt(choiceIndex).Properties.GetPropertyValue("ID")}\n");

            while (choiceIndex < (subMenuSystemChoices.Count))
                formattedObjectList += ($"\n ╠ {choiceIndex++ + choiceIndexOffset} » {subMenuSystemChoices.ElementAt(choiceIndex).InteractionTitle}\n");

            return formattedObjectList;
        }

        private void ReturnToMainMenu(IUserInterface currentInterface, PropertyContainer sourceContainer)
        {
            changeInterfaceStyleCallback(new MainMenu());
            exitExploreMenu = true;
        }
    }
}
