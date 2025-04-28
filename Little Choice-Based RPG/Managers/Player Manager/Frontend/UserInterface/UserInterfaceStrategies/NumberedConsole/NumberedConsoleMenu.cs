using Little_Choice_Based_RPG.Resources;
using Little_Choice_Based_RPG.Types.Interactions;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.InteractionUsingNothing;
using Little_Choice_Based_RPG.Types.MenuElements;
using Little_Choice_Based_RPG.Types;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory.InventoryExtensions;

namespace Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface.UserInterfaceStrategies.NumberedConsole.NumberedConsoleMenus
{
    /// <summary> Outlines a menu which provides numbered choices to the player. This menu uses the console. </summary>
    public abstract class NumberedConsoleMenu : IUserInterface
    {
        protected const int choiceIndexOffset = 1;
        protected const string defaultTransitionalAction =
            $"A tsunami of a thousand glass-like reflections tear open reality with a roar.\n" +
            $"When they close, you are left standing in their place.";

        protected PlayerController currentPlayerController;

        private List<IInvokableInteraction> systemChoices = new();
        private UserInputManager.DrawMenuCallbackDelegate drawUserInterfaceDelegate;

        private bool invokedWithinCurrentMenu = false;


        protected NumberedConsoleMenu(PlayerController setCurrentPlayerController)
        {
            currentPlayerController = setCurrentPlayerController;
            systemChoices = InitialiseSystemChoices();
            drawUserInterfaceDelegate = DrawUserInterface;

            //Subscribe to user messages
            ItemContainer.BroadcastGlobalUserMessage += OnGlobalUserMessage;
            currentPlayerController.ReceivedLocalUserMessage += OnLocalUserMessage;
        }

        /// <summary> Provides the main logic for this menu and keeps the player here until ready to leave. </summary>
        public void RunMenu()
        {
            while (!ExitMenu)
            {
                List<IInvokableInteraction> orderedInteractions = new();

                MenuElementList newTextElements = GenerateMenuElements(out orderedInteractions);

                DrawUserInterface(newTextElements);

                int? userInput = UserInputManager.AwaitUserInput(orderedInteractions.Count, drawUserInterfaceDelegate, newTextElements, currentPlayerController);

                //If userInput was null, assume a command was written instead!
                if (userInput is not null)
                    InvokeInteraction((int) userInput, orderedInteractions);
            }
        }


        /// <summary> Creates the system choices which will be selectable by the player. </summary>
        protected virtual List<IInvokableInteraction> GetMenuInteractions()
        {
            List<IInvokableInteraction> allInteractions = new List<IInvokableInteraction>();

            allInteractions.AddRange(systemChoices);

            return allInteractions;
        }


        /// <summary> Initialises system choices for this menu. For example, the 'Return to Main Menu' interaction is a system choice. </summary>
        protected virtual List<IInvokableInteraction> InitialiseSystemChoices()
        {
            List<IInvokableInteraction> newSystemChoices = new();

            //Add a choice which returns to the main menu.
            newSystemChoices.Add(SystemChoices.ReturnToMainMenu);

            return newSystemChoices;
        }

        protected virtual MenuElementList GenerateMenuElements(out List<IInvokableInteraction> orderedInteractions)
        {
            MenuElementList newElements = new();

            newElements.UpsertElement(1, MenuElementIdentity.TopStatusBar, GenerateTopStatusBar(), this);
            newElements.UpsertElement(2, MenuElementIdentity.TransitionalAction, GenerateTransitionalAction(), this);
            newElements.UpsertElement(6, MenuElementIdentity.AvailableChoices, FormatChoices(GetMenuInteractions(), out orderedInteractions), this);

            return newElements;
        }

        protected abstract string GenerateTopStatusBar();

        protected virtual string GenerateTransitionalAction()
        {
            string transitionalAction = null;

            string designPrefix = ("\n ╔════════════════════════════════════════════════════════════════════════════════════════-=════-=═=-=--=-=-- - - -");
            string designBody = string.Empty;
            string designSuffix = ("\n ╚═════════════════════════════════════════════════════════════════════════════════" + "══════-=════-=═=-=--=-=-- - - -");


            if (invokedWithinCurrentMenu)
                transitionalAction = currentPlayerController.CurrentHistoryLog.HistoryLog.Peek();

            transitionalAction ??= defaultTransitionalAction;
            List<string> reformattedAction = UserInterfaceUtilities.SplitIntoLines(transitionalAction, "\n ╠├ ", "\n ╠├ ", "\n ╠├ ");

            foreach (string line in reformattedAction)
                designBody += line;


            string completeDesign = designPrefix + designBody + designSuffix;

            return completeDesign;
        }

        protected virtual string FormatChoices(List<IInvokableInteraction> targetInteractions, out List<IInvokableInteraction> sortedInteractions)
        {
            List<IInvokableInteraction> reorderedInteractions = new();
            string createdChoiceList = "";
            int choiceIndex = 0;

            //Prefix
            createdChoiceList += ("\n ╔════════════════════════════════════════════════════════════════════════════════════════-=════-=═=-=--=-=-- - - -");

            //Main choice list
            while (choiceIndex < (targetInteractions.Count))
            {
                createdChoiceList += ($"\n ╠ {choiceIndex + choiceIndexOffset} » {targetInteractions.ElementAt(choiceIndex).InteractionTitle}");
                reorderedInteractions.Add(targetInteractions.ElementAt(choiceIndex));
                choiceIndex++;
            }

            //Suffix
            createdChoiceList += ("\n ╠═════════════════════════════════════════════════════════════════════════════════" + "══════-=════-=═=-=--=-=-- - - -");

            sortedInteractions = reorderedInteractions;
            return createdChoiceList;
        }


        /// <summary> Draw the User Interface. This writes all assigned text entries to the interface </summary>
        private void DrawUserInterface(MenuElementList currentElements)
        {
            MenuElementList writtenElements = new();

            Console.Clear();
            Console.WriteLine("\x1b[3J"); //Escape code from stackoverflow! It clears the whole console buffer :) *Magic*

            //Iterate through priorities until all elements are written.
            for (uint selectedPriority = 0; writtenElements.elements.Count < currentElements.elements.Count; selectedPriority++)
            {
                //Order elements by priority
                foreach (MenuElement entry in currentElements.elements)
                    if (entry.Priority == selectedPriority)
                        writtenElements.elements.Add(entry);
            }

            //Write elements in order
            foreach (MenuElement entryToWrite in writtenElements.elements)
                UserInterfaceUtilities.WriteDialogue(entryToWrite.Content, entryToWrite.WriteSpeed);
        }


        /// <summary> Executes an interaction based on the user's input. </summary>
        private void InvokeInteraction(int userInput, List<IInvokableInteraction> listedInteractions)
        {
            IInvokableInteraction selectedInteraction = listedInteractions.ElementAt(userInput);

            //Invoke!
            selectedInteraction.AttemptInvoke(currentPlayerController);

            //Record the action in the log.
            if (selectedInteraction.InteractionContext != InteractionRole.System)
                currentPlayerController.CurrentHistoryLog.AddNewHistoryLog(selectedInteraction.InteractDescriptor);

            invokedWithinCurrentMenu = true;
        }

        protected virtual void OnGlobalUserMessage(object? sender, string userMessage)
        {
            currentPlayerController.CurrentHistoryLog.AddNewHistoryLog(userMessage);

            RunMenu();
        }

        protected virtual void OnLocalUserMessage(object? sender, string userMessage)
        {
            currentPlayerController.CurrentHistoryLog.AddNewHistoryLog(userMessage);

            RunMenu();
        }

        /// <summary> Being set to true indicates that the menu is ready to be left and deleted. 
        /// Do not set to true unless there is a new menu ready to be used in its place! </summary>
        public bool ExitMenu { get; set; } = false;
    }
}
