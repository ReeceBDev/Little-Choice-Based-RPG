using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface.UserInterfaceStrategies.NumberedConsole.NumberedConsoleMenus;
using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface.UserInterfaceStyles;
using Little_Choice_Based_RPG.Resources;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory.InventoryExtensions;
using Little_Choice_Based_RPG.Types.Interactions;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.DoubleParameterDelegates;
using Little_Choice_Based_RPG.Types.MenuElements;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using static Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface.UserInterfaceHandler;
using static Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.DoubleParameterDelegates.SystemInteractionUsingCurrentPlayerControllerAndGameObject;
using static Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.InteractionUsingNothing;

namespace Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface.UserInterfaceStrategies.NumberedConsole
{
    public class InventoryMenu : NumberedConsoleMenu
    {
        public InventoryMenu(PlayerController setPlayerController) : base(setPlayerController)
        {
        }

        protected override List<IInvokableInteraction> GetMenuInteractions()
        {
            List<IInvokableInteraction> allInteractions = base.GetMenuInteractions();

            //Get interacts that enter the inspect menu for every item on the current player
            allInteractions.AddRange(GeneratePlayerInventoryChoices());

            return allInteractions;
        }

        protected override List<IInvokableInteraction> InitialiseSystemChoices()
        {
            List<IInvokableInteraction> newSystemChoices = base.InitialiseSystemChoices();

            //Add a choice to switch back to the exploration menu.
            newSystemChoices.Add(SystemChoices.SwitchToExploreMenu);

            return newSystemChoices;
        }

        protected override MenuElementList GenerateMenuElements(out List<IInvokableInteraction> orderedInteractions)
        {
            MenuElementList newElements = base.GenerateMenuElements(out orderedInteractions);

            //Add an element containing inspect data for the target GameObject.
            newElements.UpsertElement(3, MenuElementIdentity.TargetData, GenerateInventoryTitle(), this);

            return newElements;
        }

        protected override string GenerateTopStatusBar()
        {
            string topStatusPrefix = " -══════════════════════════════════════════════════════════════════════════════════" + "══════-=════-=═=-=--=-=-- - - -";
            string topStatusInfix = "\n  ";
            string topStatusBar = 
                $"           -=-    {currentPlayerController.CurrentPlayer.Properties.GetPropertyValue("Name")}    -=-" +
                $"        -=-    {DateTime.UtcNow.AddYears(641)}    -=- ";

            string concatenatedStatusBar = topStatusPrefix + topStatusInfix + topStatusBar;

            return concatenatedStatusBar;
        }

        private string GenerateInventoryTitle()
        {
            string bottomStatus =
                "\n                -=-    INVENTORY    -=-        -=-    Current Weight Held: " +
                $"{(currentPlayerController.CurrentPlayer.Properties.HasExistingPropertyName("Weightbearing.WeightHeldInKG") ?
                currentPlayerController.CurrentPlayer.Properties.GetPropertyValue("Weightbearing.WeightHeldInKG") : "0")}kg " +
                $"/ {currentPlayerController.CurrentPlayer.Properties.GetPropertyValue("Weightbearing.StrengthInKG")}kg " +
                "    -=-               ";

            return bottomStatus;
        }

        protected override string FormatChoices(List<IInvokableInteraction> targetInteractions, out List<IInvokableInteraction> sortedInteractions)
        {
            const int maximumInventoryLength = 30;

            List<IInvokableInteraction> orderedInteractions = new(); //Written listings, in order
            List<IInvokableInteraction> inventoryListings = new();
            List<IInvokableInteraction> interactionListings = new();
            List<string> paddedLines = new();

            string createdChoiceList = string.Empty;

            int choiceIndex = 0;
            int inventoryIndex = 0;
            int interactionIndex = 0;


            //Split inventory listings.
            foreach (var interaction in targetInteractions)
            {
                if (interaction.InteractionContext.Equals(InteractionRole.SystemInventoryEntry))
                    inventoryListings.Add(interaction);
            }

            //Split other interactions.
            foreach (var interaction in targetInteractions)
            {
                if (!interaction.InteractionContext.Equals(InteractionRole.SystemInventoryEntry))
                    interactionListings.Add(interaction);
            }

            //Pad the remaining lines.
            for (int totalLength = 0; totalLength < maximumInventoryLength; totalLength++)
                paddedLines.Add("\n ╠               -=-    ---- Empty ----    -=-       -=-    ---- Empty ----    -=-");


            //Write Prefix
            createdChoiceList += ("\n ╔════════════════════════════════════════════════════════════════════════════════════════-=════-=═=-=--=-=-- - - -");

            //Write inventory listings.
            while (inventoryIndex < (inventoryListings.Count))
            {
                createdChoiceList += ($"\n ╠ {choiceIndex + choiceIndexOffset} » {inventoryListings.ElementAt(inventoryIndex).InteractionTitle}");
                orderedInteractions.Add(inventoryListings.ElementAt(inventoryIndex));
                inventoryIndex++;

                choiceIndex++;
            }

            //Write padded lines.
            foreach (var line in paddedLines)
                createdChoiceList += line;


            //Write other interaction listings.
            while (interactionIndex < (interactionListings.Count))
            {
                createdChoiceList += ($"\n ╠ {choiceIndex + choiceIndexOffset} » {interactionListings.ElementAt(interactionIndex).InteractionTitle}");
                orderedInteractions.Add(interactionListings.ElementAt(interactionIndex));
                interactionIndex++;

                choiceIndex++;
            }

            //Write Suffix
            createdChoiceList += ("\n ╠═════════════════════════════════════════════════════════════════════════════════" + "══════-=════-=═=-=--=-=-- - - -");

            sortedInteractions = orderedInteractions;
            return createdChoiceList;
        }

        private List<IInvokableInteraction> GeneratePlayerInventoryChoices()
        {
             int weightPosition = 97;

            List<IInvokableInteraction> inventoryInspects = new();
            List<GameObject> inventoryContents = GetPlayerInventory();

            foreach (var item in inventoryContents)
            {

                string inventoryEntryName = $"Inspect closer...    =-=   {item.Properties.GetPropertyValue("Name")}";
                string inventoryEntryWeightPrefix = $"   =-=   Weight: ";
                string inventoryEntryWeightActual = item.Properties.GetPropertyValue("WeightInKG").ToString();

                string completeInventoryEntry = inventoryEntryName + inventoryEntryWeightPrefix.PadLeft(weightPosition - inventoryEntryName.Length) + inventoryEntryWeightActual + "kg";

                SystemInteractionUsingCurrentPlayerControllerAndGameObjectDelegate switchToInspectMenu = 
                    new SystemInteractionUsingCurrentPlayerControllerAndGameObjectDelegate(SystemChoices.SwitchToInspectMenuLogic);

                IInvokableInteraction newInteraction =
                    new SystemInteractionUsingCurrentPlayerControllerAndGameObject
                    (
                        switchToInspectMenu,
                        completeInventoryEntry,
                        $"You take a much closer look at the {item.Properties.GetPropertyValue("Name")}.",
                        item,
                        InteractionRole.SystemInventoryEntry
                    );

                inventoryInspects.Add(newInteraction);
            }

            return inventoryInspects;
        }

        private List<GameObject> GetPlayerInventory()
        {
             List<GameObject> currentContents = new();

            ItemContainer playerInventory = (ItemContainer)currentPlayerController.CurrentPlayer.Extensions.Get("ItemContainer");
            List<GameObject> playerInventoryContents = playerInventory.Inventory;

            foreach (var entity in playerInventoryContents)
                currentContents.Add(entity);

            return currentContents;
        }
    }
}
