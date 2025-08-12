using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using Little_Choice_Based_RPG.Types.Interactions;
using Little_Choice_Based_RPG.External.EndpointServices;
using Little_Choice_Based_RPG.Managers.PlayerControl;
using LCBRPG_User_Console.ConsoleUtilities;
using LCBRPG_User_Console.Types;
using LCBRPG_User_Console.MenuResource;
using LCBRPG_User_Console.Types.DisplayDataEntries;
using LCBRPG_User_Console.Types.ActualElements;

namespace LCBRPG_User_Console.ConsoleMenus
{
    internal class ExploreMenu : NumberedConsoleMenu
    {
        public ExploreMenu(LocalPlayerSession setPlayerSession, InteractionCache setInteractionCache, HistoryLogCache setHistoryLogCache) 
            : base(setPlayerSession, setInteractionCache, setHistoryLogCache)
        {
        }

        protected override List<InteractionDisplayData> InitialiseSystemChoices()
        {
            List<InteractionDisplayData> newSystemChoices = base.InitialiseSystemChoices();

            //Add a choice which opens the player's inventory
            newSystemChoices.Add(SystemChoices.SwitchToInventoryMenu);

            return newSystemChoices;
        }

        protected override DisplayDataList InitialiseMenuElements(List<InteractionDisplayData> displayedInteractions)
        {
            DisplayDataList newElements = base.InitialiseMenuElements(displayedInteractions);

            newElements.ChangePriority(2, ElementIdentities.TransitionalAction, this);
            newElements.UpsertElement(3, new ExploreDescriptionElement(ElementIdentities.CurrentDescription, PlayerSession), this);
            newElements.UpsertElement(4, new HistoryLogElement(ElementIdentities.HistoryLog, historyLogCacheResource), this);
            newElements.ChangePriority(5, ElementIdentities.AvailableChoices, this);

            return newElements;
         }
    }
}
