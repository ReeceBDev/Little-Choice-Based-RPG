using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface.UserInterfaceStrategies.NumberedConsole;
using Little_Choice_Based_RPG.Managers.World;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
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
        private bool firstTimeSinceTransition;
        private ExploreMenuTextEntry[] textEntries;
        private Stack<string> historyLog;
        private uint historyDisplayLength = 20;
        private string transitionalAction = "";
        private readonly string defaultTransitionalAction;
        private int choiceIndexOffset = 1;
        private List<IInvokableInteraction> relevantInteractions = new List<IInvokableInteraction>();

        private protected ChangeInterfaceStyleCallback changeInterfaceStyleCallback;
        public ExploreMenu(ChangeInterfaceStyleCallback changeInterfaceStyle, Player player, GameEnvironment currentEnvironment)
        {
            exitExploreMenu = false;
            firstTimeSinceTransition = true;

            currentPlayer = player;
            currentRoomID = player.Position;
            Room currentRoom = currentEnvironment.GetRoomByID(currentRoomID);

            systemChoices = InitialiseSystemChoices();
            subMenuSystemChoices = InitialiseSubMenuSystemChoices();

            defaultTransitionalAction = $"Welcome to the game {currentPlayer}!";
            transitionalAction = defaultTransitionalAction;
        }

        private List<IInvokableInteraction> InitialiseSystemChoices()
        {
            List<IInvokableInteraction> allCurrentChoices = new List<IInvokableInteraction>();

            InteractionUsingNothingDelegate returnToMainMenu = new InteractionUsingNothingDelegate(ReturnToMainMenu);
            allCurrentChoices.Add(new InteractionUsingNothing(returnToMainMenu, "Go back to main menu.", "Returning to main menu...", InteractionRole.System));

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

                if (firstTimeSinceTransition)
                    InitialiseMainTextEntries(listedInteractions);

                Console.Clear();
                WriteToUserInterface(textEntries);

                int userInput = AwaitUserInput(listedInteractions.Count);
                InvokeInteraction(userInput, listedInteractions);
            }
        }

        /// <summary> Generates a Sub-Menu asking the player to choose an object from the room, which optionally matches property filters. </summary>
        private void GenerateSubMenu(IInvokableInteraction sender, string requirementDescription, IInvokableInteraction abortInteraction, List<EntityProperty>? setFilters = null)
        {
            List<GameObject> possibleObjects = currentRoom.GetRoomObjects(setFilters);
            List<GameObject> listedObjects = new List<GameObject>();

            subMenuSystemChoices.Add(abortInteraction);

            //Write the Sub-Menu to the user interface.
            ExploreMenuTextEntry[] listToWrite = InitialiseSubMenuTextEntries(textEntries, requirementDescription, listedObjects);

            Console.Clear();
            WriteToUserInterface(listToWrite);

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
            

            throw new Exception("userInput didn't land into either if statements. That means it hit neither the system options, nor the object options.");
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
            bool isInputValid = false;
            int userSelection = -1;

            int minimumInputValue = choiceIndexOffset;
            int maximumInputValue = choiceIndexOffset + numberOfInteractions - 1;

            //Loop until a choice is selected.
            while (isInputValid == false)
            {
                userInput = Console.ReadLine();

                if (userInput.All(Char.IsDigit)) //input must be a number
                    userSelection = Convert.ToInt32(userInput);

                if (minimumInputValue <= userSelection && userSelection <= maximumInputValue) //input must match the range of choices.
                    isInputValid = true; //Break
            }

            return userSelection;
        }

        private void InvokeInteraction(int userInput, List<IInvokableInteraction> listedInteractions)
        {
            IInvokableInteraction selectedInteraction = listedInteractions.ElementAt(userInput - choiceIndexOffset);

            //Record the action in the log.
            AddNewHistoryLog(selectedInteraction.InteractDescriptor.Value);

            //Record the action in the TransitionalAction if the location around the player changed.
            if ((selectedInteraction.InteractionContext == InteractionRole.Navigation))
            {
                transitionalAction = selectedInteraction.InteractDescriptor.Value;
                UpdateTransitionalAction();
            }

            //Invoke!
            selectedInteraction.AttemptInvoke(this);
        }

        //Record the action in the TransitionalAction if the location around the player changed.
        private void OnEventRoomDescriptorChanged(Interaction trigger)
        {
            transitionalAction = trigger.InteractDescriptor.Value;
            UpdateTransitionalAction();
        }

        public void InitialiseMainTextEntries(List<IInvokableInteraction> listedInteractions)
        {
            textEntries = [];

            List<ExploreMenuTextEntry> exploreMenu = new List<ExploreMenuTextEntry>
            {
            new ExploreMenuTextEntry(ExploreMenuIdentity.TopStatusBar, GetTopStatusBar(), 0),
            new ExploreMenuTextEntry(ExploreMenuIdentity.None, GetStyleLine(1), 0),
            new ExploreMenuTextEntry(ExploreMenuIdentity.TransitionalAction, transitionalAction, 0),
            new ExploreMenuTextEntry(ExploreMenuIdentity.CurrentDescription, GetCurrentDescription(), 0),
            new ExploreMenuTextEntry(ExploreMenuIdentity.None, GetStyleLine(2), 0),
            new ExploreMenuTextEntry(ExploreMenuIdentity.HistoryLog, GetHistoryLog(), 0),
            new ExploreMenuTextEntry(ExploreMenuIdentity.None, GetStyleLine(3), 0),
            new ExploreMenuTextEntry(ExploreMenuIdentity.AvailableChoices, FormatChoices(listedInteractions), 0),
            new ExploreMenuTextEntry(ExploreMenuIdentity.None, GetStyleLine(4), 0),
            new ExploreMenuTextEntry(ExploreMenuIdentity.None, GetStyleLine(5), 0)
            };

            textEntries = exploreMenu.ToArray();
        }

        private ExploreMenuTextEntry[] InitialiseSubMenuTextEntries(ExploreMenuTextEntry[] overwrittenInterfaceEntries, string requirementDescription, List<GameObject> listedObjects)
        {

            if (!(overwrittenInterfaceEntries.Length < 1))
                throw new Exception($"The required list of overwrittenInterfaceEntries does not contain enough lines to be used here. It might not have been initialised. There must be an existing user interface to put the sub-menu over the top of!");

            IEnumerable<ExploreMenuTextEntry> subMenu = overwrittenInterfaceEntries.SkipLast(3);

            string subMenuFormatPart1 = $"  == +] ======][========][======= MAKE A SELECTION =======][======][=======. =- = - -[ =   - .";
            string subMenuFormatPart2 = $"  . - = -- - - ===========-===========================--======-=---=-. ---=== =-----. -  - .";
            string subMenuFormatPart3 = $"                          {requirementDescription}";

            subMenu.Append(new ExploreMenuTextEntry(ExploreMenuIdentity.None, subMenuFormatPart1, 0));
            subMenu.Append(new ExploreMenuTextEntry(ExploreMenuIdentity.None, subMenuFormatPart2, 0));
            subMenu.Append(new ExploreMenuTextEntry(ExploreMenuIdentity.None, subMenuFormatPart3, 0));
            subMenu.Append(new ExploreMenuTextEntry(ExploreMenuIdentity.None, subMenuFormatPart2, 0));

            subMenu.Append(new ExploreMenuTextEntry(ExploreMenuIdentity.None, FormatSubMenuChoices(listedObjects), 0));
            subMenu.Append(new ExploreMenuTextEntry(ExploreMenuIdentity.None, FormatChoices(subMenuSystemChoices), 0));

            subMenu.Append(new ExploreMenuTextEntry(ExploreMenuIdentity.None, GetStyleLine(4), 0));
            subMenu.Append(new ExploreMenuTextEntry(ExploreMenuIdentity.None, GetStyleLine(5), 0));

            return subMenu.ToArray();
        }

        private ExploreMenuTextEntry GetTextEntryByIdentifier(ExploreMenuIdentity targetIdentifier)
        {
            ExploreMenuTextEntry? targetEntry = null;

            foreach (ExploreMenuTextEntry entry in textEntries)
            {
                if (entry.UniqueIdentity == targetIdentifier)
                    return entry;
            }

            if (targetEntry == null)
                throw new ArgumentException($"The TopStatusBar didnt seem to exist. No UserInterfaceTextEntry within {textEntries} had the identifier {ExploreMenuIdentity.TopStatusBar}!");

            throw new Exception($"Critical failure! This method didn't complete! This shouldn't have happened. It looks like this method somehow didn't complete any other path, but it should have!");
        }

        private string GetTopStatusBar()
        {
            return $"{currentRoom.Name}\t -=-\t Potsun Burran\t -=-\t Relative, {currentRoomID}\t -=-\t {DateTime.UtcNow.AddYears(641)}";
        }

        private void UpdateTopStatusBar()
        {
            ExploreMenuTextEntry targetEntry = GetTextEntryByIdentifier(ExploreMenuIdentity.TopStatusBar);
            int targetIndex = Array.IndexOf(textEntries, targetEntry);

            textEntries[targetIndex] = new ExploreMenuTextEntry(ExploreMenuIdentity.TopStatusBar, GetTopStatusBar(), 40);
            WriteToUserInterface(textEntries);
        }

        private void UpdateTransitionalAction()
        {
            ExploreMenuTextEntry targetEntry = GetTextEntryByIdentifier(ExploreMenuIdentity.TransitionalAction);
            int targetIndex = Array.IndexOf(textEntries, targetEntry);

            textEntries[targetIndex] = new ExploreMenuTextEntry(ExploreMenuIdentity.TransitionalAction, transitionalAction, 40);
            WriteToUserInterface(textEntries);
        }

        private string GetCurrentDescription()
        {
            return ConcatenateDescriptors(currentRoom.GetRoomDescriptors());
        }

        private void UpdateCurrentDescription()
        {
            ExploreMenuTextEntry targetEntry = GetTextEntryByIdentifier(ExploreMenuIdentity.CurrentDescription);
            int targetIndex = Array.IndexOf(textEntries, targetEntry);

            textEntries[targetIndex] = new ExploreMenuTextEntry(ExploreMenuIdentity.CurrentDescription, GetCurrentDescription(), 40);
            WriteToUserInterface(textEntries);
        }

        private string GetHistoryLog() //Should display bottom to top :)
        {
            string concatenatedLog = "";
            Stack<string> holdingStack = new Stack<string>();

            uint iteration = 0;

            //Grabs entries and puts them at the bottom of a new stack.
            while (historyLog.Count > 0 && historyDisplayLength > iteration)
            {
                string poppedLog = historyLog.Pop();
                holdingStack.Push(poppedLog);

                concatenatedLog += $"\n<>-< {DateTime.UtcNow.AddYears(641)} >-< {poppedLog} >-<>";
            }

            // Returns entries back to their stack.
            while (holdingStack.Count > 0)
                historyLog.Push(holdingStack.Pop());

            // Insert whitespace to pad up until the maximum history length.
            while (historyDisplayLength > concatenatedLog.Length)
                concatenatedLog += "\n";

            return concatenatedLog;
        }

        private void UpdateHistoryLog()
        {
            ExploreMenuTextEntry targetEntry = GetTextEntryByIdentifier(ExploreMenuIdentity.HistoryLog);
            int targetIndex = Array.IndexOf(textEntries, targetEntry);

            textEntries[targetIndex] = new ExploreMenuTextEntry(ExploreMenuIdentity.HistoryLog, GetHistoryLog(), 40);
            WriteToUserInterface(textEntries);
        }

        private void AddNewHistoryLog(string newHistoryLog)
        {
            historyLog.Push(newHistoryLog);
            UpdateHistoryLog();
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

        private void UpdateAvailableChoices() //THIS WONT WORK, because choices come in different types and so on. This should probably just add new choices and remove old choices upon event call (of what?)
        {
            /*
            ExploreMenuTextEntry targetEntry = GetTextEntryByIdentifier(ExploreMenuIdentity.AvailableChoices);
            int targetIndex = Array.IndexOf(textEntries, targetEntry);

            textEntries[targetIndex] = new ExploreMenuTextEntry(ExploreMenuIdentity.AvailableChoices, GetAvailableChoices(), 40);
            WriteToUserInterface(textEntries);
            */
        }

        private string GetStyleLine(uint style)
        {
            switch (style)
            {
                case 1:
                    return " ===========-========== ----========-========-= --..-- .";
                case 2:
                    return " ====-====-===-=--=-=--_-----_--= =- -_ ._";
                case 3:
                    return " ===========";
                case 4:
                    return " ====-====-===-=--=-=-- - - -";
                case 5:
                    return " >> > > ";

                default:
                    throw new ArgumentException($"No switch case for the style matching {style}");
            }
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

        private void WriteToUserInterface(ExploreMenuTextEntry[] listToWrite)
        {
            foreach (ExploreMenuTextEntry entryToWrite in listToWrite)
            {
                if (entryToWrite.WriteSpeed == 0)
                    UserInterfaceUtilities.WriteDialogue(entryToWrite.Content, 0);
                else if (entryToWrite.WriteSpeed == 40)
                    UserInterfaceUtilities.WriteDialogue(entryToWrite.Content, 40);
            }
        }

        private string FormatChoices(List<IInvokableInteraction> possibleInteractions)
        {
            string createdChoiceList = "";
            int choiceIndex = 0;

            while(choiceIndex < (possibleInteractions.Count))
                createdChoiceList += ($"{choiceIndex++ + choiceIndexOffset}. {possibleInteractions.ElementAt(choiceIndex).InteractionTitle}\n");

            return createdChoiceList;
        }

        private string FormatSubMenuChoices(List<GameObject> listedObjects)
        {

            string formattedObjectList = "";
            int choiceIndex = 0;

            while (choiceIndex < (listedObjects.Count))
                formattedObjectList += ($"{choiceIndex++ + choiceIndexOffset}. {listedObjects.ElementAt(choiceIndex).Name} - {listedObjects.ElementAt(choiceIndex).ID}\n");

            while (choiceIndex < (subMenuSystemChoices.Count))
                formattedObjectList += ($"{choiceIndex++ + choiceIndexOffset}. {subMenuSystemChoices.ElementAt(choiceIndex).InteractionTitle}\n");

            return formattedObjectList;
        }

        private void ReturnToMainMenu(IUserInterface currentInterface)
        {
            changeInterfaceStyleCallback(new MainMenu());
            exitExploreMenu = true;
        }
    }
}
