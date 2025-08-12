using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Resources.Systems.InformationalSystems.Descriptor;
using Little_Choice_Based_RPG.Managers.PlayerControl;
using LCBRPG_User_Console.ConsoleUtilities;
using LCBRPG_User_Console.Types;
using LCBRPG_User_Console.Types.DisplayDataEntries;
using LCBRPG_User_Console.MenuResource;

namespace LCBRPG_User_Console.ConsoleMenus
{
    internal class InspectMenu : NumberedConsoleMenu
    {
        private ulong inspectee;

        private List<string> currentTextEntries = new();
        private List<string> currentChoiceEntries = new();
        private List<InteractionDisplayData> currentTargetInteractions = new();
        private List<InteractionDisplayData> currentSystemInteractions = new();


        public InspectMenu(LocalPlayerSession setPlayerSession, InteractionCache setInteractionCache, HistoryLogCache setHistoryLogCache, uint setTargetInspecteeID) 
            : base(setPlayerSession, setInteractionCache, setHistoryLogCache)
        {
            inspectee = setTargetInspecteeID;
        }

        protected override List<InteractionDisplayData> ConcatenateInteractions()
        {
            List<InteractionDisplayData> allInteractions = base.ConcatenateInteractions();

            //Public Interactions from the target itself
            allInteractions.AddRange(InteractionRetriever.GetInteractions(inspectee));

            //Private interactions from the player, about the target
            allInteractions.AddRange(InteractionRetriever.GetPrivateInteractions(PlayerSession.CurrentPlayer, inspectee));

            return allInteractions;
        }

        protected override List<InteractionDisplayData> InitialiseSystemChoices()
        {
            List<InteractionDisplayData> newSystemChoices = base.InitialiseSystemChoices();

            //Add a choice which returns to the player's inventory
            newSystemChoices.Add(SystemChoices.ReturnToInventoryMenu);

            //Add a choice to switch back to the exploration menu.
            newSystemChoices.Add(SystemChoices.SwitchToExploreMenu);

            return newSystemChoices;
        }

        protected override DisplayDataList InitialiseMenuElements(List<InteractionDisplayData> displayedInteractions)
        {
            DisplayDataList newElements = base.InitialiseMenuElements(displayedInteractions);

            //Add an element containing inspect data for the target GameObject.
            newElements.UpsertElement(2, ElementIdentities.TargetData, GenerateTargetInspectData(), this);
            newElements.ChangePriority(3, ElementIdentities.TransitionalAction, this);

            return newElements;
        }

    }
}
