using LCBRPG_User_Console.ConsoleUtilities;
using LCBRPG_User_Console.MenuResource;
using LCBRPG_User_Console.Types;
using LCBRPG_User_Console.Types.ConsoleElements;
using LCBRPG_User_Console.Types.DisplayData;
using Little_Choice_Based_RPG.External.Types;
using Little_Choice_Based_RPG.Managers.PlayerControl;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Systems.InformationalSystems.Descriptor;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;

namespace LCBRPG_User_Console.ConsoleMenus
{
    internal class InspectMenu : NumberedConsoleMenu
    {
        private uint inspectee;

        private List<string> currentTextEntries = new();
        private List<string> currentChoiceEntries = new();
        private List<InteractionDisplayData> currentTargetInteractions = new();
        private List<InteractionDisplayData> currentSystemInteractions = new();


        public InspectMenu(LocalPlayerSession setPlayerSession, InteractionCache setInteractionCache, HistoryLogCache setHistoryLogCache, uint setTargetInspecteeID,
            ChangeInterfaceCallback changeMenuCallback) : base(setPlayerSession, setInteractionCache, setHistoryLogCache, changeMenuCallback)
        {
            inspectee = setTargetInspecteeID;
        }

        protected override List<InteractionDisplayData> ConcatenateInteractions()
        {
            List<InteractionDisplayData> allInteractions = base.ConcatenateInteractions();

            //Interactions from the player about the target
            allInteractions = allInteractions.Where(i => i.AssociatedObjectID == inspectee).ToList();

            return allInteractions;
        }

        protected override List<InteractionDisplayData> InitialiseSystemChoices()
        {
            List<InteractionDisplayData> newSystemChoices = base.InitialiseSystemChoices();
            ulong systemInteractionID = 0;

            //Add a choice which returns to the player's inventory
            newSystemChoices.Add(new InteractionDisplayData(++systemInteractionID, "Open your inventory.", InteractionRole.System.ToString(),
                PlayerSession.PlayerStatusServiceInstance.GetPlayerID()));

            seededSystemInteractions.Add(
                systemInteractionID, //Must be the same as its respective display data above
                () => SystemChoices.SwitchMenuLogic(
                    this,
                    ChangeInterfaceCallbackInstance,
                    new InventoryMenu(PlayerSession, interactionCacheResource, historyLogCacheResource, ChangeInterfaceCallbackInstance))
                );

            //Add a choice to switch back to the exploration menu.
            newSystemChoices.Add(new InteractionDisplayData(++systemInteractionID, "Return to the exploration screen.", InteractionRole.System.ToString(),
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
            newElements.UpsertElement(2,  new InspecteeDataElement(ElementIdentities.TargetData, inspectee, PlayerSession.PlayerInventoryServiceInstance), this);
            newElements.ChangePriority(3, ElementIdentities.TransitionalAction, this);

            return newElements;
        }
    }
}
