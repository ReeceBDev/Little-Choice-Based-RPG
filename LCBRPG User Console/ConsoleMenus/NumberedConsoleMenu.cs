using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory.InventoryExtensions;
using Little_Choice_Based_RPG.Managers.PlayerControl;
using LCBRPG_User_Console.ConsoleUtilities;
using LCBRPG_User_Console.Types;
using Little_Choice_Based_RPG.External.Types.Services;
using Little_Choice_Based_RPG.External.Types;
using LCBRPG_User_Console.MenuResource;
using LCBRPG_User_Console.Types.DisplayDataEntries;
using LCBRPG_User_Console.Types.ActualElements.StatusBars;
using LCBRPG_User_Console.Types.ActualElements.TransitionalActions;
using LCBRPG_User_Console.Types.ActualElements;
using System.Threading.Tasks;
using static LCBRPG_User_Console.ConsoleUtilities.UserInputUtilities;

namespace LCBRPG_User_Console.ConsoleMenus
{
    /// <summary> Outlines a menu which provides numbered choices to the player. This menu uses the console. </summary>
    internal abstract class NumberedConsoleMenu : IConsoleMenu
    {
        protected InteractionCache interactionCacheResource;
        protected HistoryLogCache historyLogCacheResource;
        protected List<string> allowedCommands;

        protected DrawMenuCallbackDelegate DrawMenuElementsCallback;

        private DisplayDataList menuElements;
        private List<InteractionDisplayData> concatenatedInteractions;

        private int inputLockCounter = 0;

        /// <summary> Being set to true indicates that the menu is ready to be left and deleted. 
        /// Do not set to true unless there is a new menu ready to be used in its place! </summary>
        public bool ExitMenu { get; set; } = false;
        protected LocalPlayerSession PlayerSession { get; }

        protected NumberedConsoleMenu(LocalPlayerSession setPlayerSession, InteractionCache setInteractionCache, HistoryLogCache setHistoryLogCache)
        {
            PlayerSession = setPlayerSession;
            interactionCacheResource = setInteractionCache;
            historyLogCacheResource = setHistoryLogCache;
        }

        /// <summary> Provides the main logic for this menu and keeps the player here until ready to leave. </summary>
        public void RunMenu()
        {
            concatenatedInteractions = ConcatenateInteractions();
            menuElements = InitialiseMenuElements(concatenatedInteractions);
            
            SubscribeToElements(menuElements);

            while (!ExitMenu)
            {
                DrawMenuElements(menuElements);
                HandleUserInput(concatenatedInteractions, inputLockCounter);
            }
        }

        /// <summary> Creates the choices which will be selectable by the player. </summary>
        protected virtual List<InteractionDisplayData> ConcatenateInteractions()
        {
            List<InteractionDisplayData> allInteractions = new();

            allInteractions.AddRange(interactionCacheResource.GetCache());
            allInteractions.AddRange(InitialiseSystemChoices());

            return allInteractions;
        }

        /// <summary> Initialises system choices for this menu. For example, the 'Return to Main Menu' interaction is a system choice. </summary>
        protected virtual List<InteractionDisplayData> InitialiseSystemChoices()
        {
            List<InteractionDisplayData> newSystemChoices = new();

            //Add a choice which returns to the main menu.
            newSystemChoices.Add(SystemChoices.ReturnToMainMenu);

            return newSystemChoices;
        }
        protected virtual DisplayDataList InitialiseMenuElements(List<InteractionDisplayData> displayedInteractions)
        {
            DisplayDataList newElements = new();

            newElements.UpsertElement(1, new StatusBarExploreElement(ElementIdentities.TopStatusBar, PlayerSession), this);
            newElements.UpsertElement(2, new TransitionalActionBlockElement(ElementIdentities.TransitionalAction, PlayerSession), this);
            newElements.UpsertElement(6, new UserInputElement(ElementIdentities.AvailableChoices, displayedInteractions, interactionCacheResource), this);

            return newElements;
        }

        protected virtual void OnElementUpdating(object? sender, EventArgs e)
        {
            if (sender is UserInputElement)
                inputLockCounter++; //Freeze user input, stacks multiple times for multiple updates
        }
        protected virtual void OnElementUpdated(object? sender, EventArgs e)
        {
            if (sender is IElement element)
            {
                //Initialise
                if (sender is UserInputElement) //Initialise specifically for UserInputElements
                    concatenatedInteractions = ConcatenateInteractions(); //Refresh interactions

                //Update this element, if it exists in menuElements
                ElementDisplayData targetElement = menuElements.elements.Find(i => i.Element.ElementIdentity == element.ElementIdentity);

                if (!targetElement.Equals(default(ElementDisplayData))) //If it exists in menuElements
                {
                    //Refresh element
                    targetElement.Element.RefreshContent();

                    //Redraw menu
                    DrawMenuElements(menuElements);
                }

                //Finalise
                if (sender is UserInputElement) //Finalise specifically for UserInputElements
                    inputLockCounter--; //Thaw user input by one stack
            }
        }

        private void HandleUserInput(List<InteractionDisplayData> interactions, int inputLockCounter)
        {
            UserInputUtilities.HandleUserInput(PlayerSession.UserInputServiceInstance, interactions, DrawMenuElementsCallback, menuElements, inputLockCounter, 
                allowedCommands);
        }

        /// <summary> Draw the User Interface. This writes all assigned text entries to the interface </summary>
        private void DrawMenuElements(DisplayDataList elementsToDraw)
        {
            DisplayDataList elementsSnapshot = new(elementsToDraw);
            DisplayDataList writtenElements = new();

            Console.Clear();
            Console.WriteLine("\x1b[3J"); //Escape code from stackoverflow! It clears the whole console buffer :) *Magic*

            //Iterate through priorities incrementally, writing each element for each, until all elements are written.
            for (uint selectedPriority = 0; writtenElements.elements.Count < elementsSnapshot.elements.Count; selectedPriority++)
            {
                //Order elements by priority
                foreach (ElementDisplayData entry in elementsSnapshot.elements)
                    if (entry.Priority == selectedPriority)
                        writtenElements.elements.Add(entry);
            }

            //Write elements in order
            foreach (ElementDisplayData entryToWrite in writtenElements.elements)
                WritelineUtilities.WriteDialogue(entryToWrite.Element.GetCachedContent(), entryToWrite.WriteSpeed);
        }

        private void SubscribeToElements(DisplayDataList targetElements)
        {
            foreach (var element in targetElements.elements)
            {
                element.Element.ElementUpdating += OnElementUpdating;
                element.Element.ElementUpdated += OnElementUpdated;
            }
        }
    }
}
