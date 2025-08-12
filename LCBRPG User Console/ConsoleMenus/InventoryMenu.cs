using LCBRPG_User_Console.ConsoleUtilities;
using LCBRPG_User_Console.MenuResource;
using LCBRPG_User_Console.Types;
using LCBRPG_User_Console.Types.ActualElements;
using LCBRPG_User_Console.Types.DisplayDataEntries;
using Little_Choice_Based_RPG.External.Types;
using Little_Choice_Based_RPG.Managers.PlayerControl;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory.InventoryExtensions;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.DoubleParameterDelegates;

namespace LCBRPG_User_Console.ConsoleMenus
{
    internal class InventoryMenu : NumberedConsoleMenu
    {
        public InventoryMenu(LocalPlayerSession setPlayerSession, InteractionCache setInteractionCache, HistoryLogCache setHistoryLogCache) 
            : base(setPlayerSession, setInteractionCache, setHistoryLogCache)
        {
        }

        protected override List<InteractionDisplayData> ConcatenateInteractions()
        {
            List<InteractionDisplayData> allInteractions = base.ConcatenateInteractions();

            //Get interacts that enter the inspect menu for every item on the current player
            allInteractions.AddRange(GeneratePlayerInventoryChoices());

            return allInteractions;
        }

        protected override List<InteractionDisplayData> InitialiseSystemChoices()
        {
            List<InteractionDisplayData> newSystemChoices = base.InitialiseSystemChoices();

            //Add a choice to switch back to the exploration menu.
            newSystemChoices.Add(SystemChoices.SwitchToExploreMenu);

            return newSystemChoices;
        }

        protected override DisplayDataList InitialiseMenuElements(List<InteractionDisplayData> displayedInteractions)
        {
            DisplayDataList newElements = base.InitialiseMenuElements(displayedInteractions);

            //Add an element containing inspect data for the target GameObject.
            newElements.UpsertElement(3, new InventoryTitleElement(ElementIdentities.TargetData, PlayerSession), this);

            return newElements;
        }

        private List<InteractionDisplayData> GeneratePlayerInventoryChoices()
        {
             int weightPosition = 97;

            List<InteractionDisplayData> inventoryInspects = new();
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

            ItemContainer playerInventory = (ItemContainer)PlayerSession.CurrentPlayer.Extensions.Get("ItemContainer");
            List<GameObject> playerInventoryContents = playerInventory.Inventory;

            foreach (var entity in playerInventoryContents)
                currentContents.Add(entity);

            return currentContents;
        }
    }
}
