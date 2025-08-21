using LCBRPG_User_Console.ConsoleUtilities;
using LCBRPG_User_Console.MenuResource;
using LCBRPG_User_Console.Types;
using LCBRPG_User_Console.Types.ConsoleElements;
using LCBRPG_User_Console.Types.ConsoleElements.ChoiceDisplays;
using LCBRPG_User_Console.Types.DisplayData;
using Little_Choice_Based_RPG.External.Types;
using Little_Choice_Based_RPG.External.Types.Services;
using Little_Choice_Based_RPG.Managers.PlayerControl;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory.InventoryExtensions;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.DoubleParameterDelegates;
using System.Collections.Generic;

namespace LCBRPG_User_Console.ConsoleMenus
{
    internal class InventoryMenu : NumberedConsoleMenu
    {
        ulong systemInteractionID = 0;

        public InventoryMenu(LocalPlayerSession setPlayerSession, InteractionCache setInteractionCache, HistoryLogCache setHistoryLogCache,
            ChangeInterfaceCallback changeMenuCallback) : base(setPlayerSession, setInteractionCache, setHistoryLogCache, changeMenuCallback)
        {
        }

        protected override List<InteractionDisplayData> ConcatenateInteractions()
        {
            List<InteractionDisplayData> allInteractions = new(); //Don't inherit from base as most interactions need to be skipped here.

            //Get interacts that enter the inspect menu for every item on the current player
            allInteractions.AddRange(GeneratePlayerInventoryChoices());
            allInteractions.AddRange(InitialiseSystemChoices());

            return allInteractions;
        }

        protected override List<InteractionDisplayData> InitialiseSystemChoices()
        {
            List<InteractionDisplayData> newSystemChoices = base.InitialiseSystemChoices();

            //Add a choice to switch back to the exploration menu.
            newSystemChoices.Add(new InteractionDisplayData(++systemInteractionID, "Close your inventory.", InteractionRole.System.ToString(), 
                PlayerSession.PlayerStatusServiceInstance.GetPlayerID()));

            seededSystemInteractions.Add(
                systemInteractionID, //Must be the same as its respective display data above
                () => SystemChoices.SwitchMenuLogic(
                    this,
                    ChangeInterfaceCallbackInstance,
                    new ExploreMenu(PlayerSession, interactionCacheResource, historyLogCacheResource, ChangeInterfaceCallbackInstance))
                );

            return newSystemChoices;
        }

        protected override DisplayDataList InitialiseMenuElements(List<InteractionDisplayData> displayedInteractions)
        {
            DisplayDataList newElements = base.InitialiseMenuElements(displayedInteractions);

            //Add an element containing inspect data for the target GameObject.
            newElements.UpsertElement(3, new InventoryTitleElement(ElementIdentities.TargetData, PlayerSession), this);
            newElements.UpsertElement(6, new InventoryChoicesElement(ElementIdentities.AvailableChoices, ConcatenateInteractions(), interactionCacheResource), this);
            //Note: I should probably link refreshes of this InventoryChoicesElement to updates to the player's inventory.

            return newElements;
        }

        private List<InteractionDisplayData> GeneratePlayerInventoryChoices()
        {
             int weightPosition = 97;

            List<InteractionDisplayData> inventoryInspectDisplays = new();
            List<ItemDisplayData> inventoryContents = PlayerSession.PlayerInventoryServiceInstance.GetPlayerInventoryDisplayData();

            foreach (var item in inventoryContents)
            {

                string inventoryEntryName = $"Inspect closer...    =-=   {item.Name}";
                string inventoryEntryWeightPrefix = $"   =-=   Weight: ";
                string inventoryEntryWeightActual = item.WeightInKG.ToString();

                string completeInventoryEntry = 
                    inventoryEntryName + inventoryEntryWeightPrefix.PadLeft(weightPosition - inventoryEntryName.Length) + inventoryEntryWeightActual + "kg";

                InteractionDisplayData newInspecteeChoice = AddInspecteeChoice(item);

                inventoryInspectDisplays.Add(newInspecteeChoice);
            }

            return inventoryInspectDisplays;
        }

        private InteractionDisplayData AddInspecteeChoice(ItemDisplayData item)
        {
            InteractionDisplayData newInspecteeChoice;

            //Add a choice to inspect this item.
            newInspecteeChoice = new InteractionDisplayData(
                ++systemInteractionID,
                $"You take a much closer look at the {item.Name}.",
                SystemInteractionContext.InventoryEntry.ToString(), //This marks the entry as inventory specific so that it may find and sort its own interactions.
                item.ID
                );

            seededSystemInteractions.Add(
                systemInteractionID, //Must be the same as its respective display data above
                () => SystemChoices.SwitchMenuLogic(
                    this,
                    ChangeInterfaceCallbackInstance,
                    new InspectMenu(PlayerSession, interactionCacheResource, historyLogCacheResource, item.ID, ChangeInterfaceCallbackInstance))
                );

            return newInspecteeChoice;
        }
    }
}
