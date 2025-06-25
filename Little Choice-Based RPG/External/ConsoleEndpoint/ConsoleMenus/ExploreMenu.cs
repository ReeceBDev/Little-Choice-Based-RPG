using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using Little_Choice_Based_RPG.Types.Interactions;
using Little_Choice_Based_RPG.External.EndpointServices;
using Little_Choice_Based_RPG.Types.External.ConsoleElements;
using Little_Choice_Based_RPG.Managers.PlayerControl;
using Little_Choice_Based_RPG.External.ConsoleEndpoint.ConsoleFunctionalities;

namespace Little_Choice_Based_RPG.External.ConsoleEndpoint.ConsoleMenus
{
    public class ExploreMenu : NumberedConsoleMenu
    {
        private const int historyLineMaxCount = 20;

        private string transitionalAction = defaultTransitionalAction;

        public ExploreMenu(PlayerController setPlayerController) : base(setPlayerController)
        {
        }

        protected override List<IInvokableInteraction> GetMenuInteractions()
        {
            List<IInvokableInteraction> allInteractions = base.GetMenuInteractions();

            //Get explore interactions for this room
            allInteractions.AddRange(InteractionRetriever.GetInteractions(currentPlayerController.CurrentRoom, InteractionRole.Explore));

            //Get explore interactions from the player
            allInteractions.AddRange(InteractionRetriever.GetPrivateInteractions(currentPlayerController.CurrentPlayer, InteractionRole.Explore));

            //Get movement interactions for this room
            allInteractions.AddRange(InteractionRetriever.GetInteractions(currentPlayerController.CurrentRoom, InteractionRole.Navigation));

            //Get movement interactions from the player
            allInteractions.AddRange(InteractionRetriever.GetPrivateInteractions(currentPlayerController.CurrentPlayer, InteractionRole.Navigation));

            return allInteractions;
        }

        protected override List<IInvokableInteraction> InitialiseSystemChoices()
        {
            List<IInvokableInteraction> newSystemChoices = base.InitialiseSystemChoices();

            //Add a choice which opens the player's inventory
            newSystemChoices.Add(SystemChoices.SwitchToInventoryMenu);

            return newSystemChoices;
        }

        protected override ConsoleElementList GenerateMenuElements(out List<IInvokableInteraction> orderedInteractions)
        {
            ConsoleElementList newElements = base.GenerateMenuElements(out orderedInteractions);

            newElements.ChangePriority(2, ConsoleElementIdentity.TransitionalAction, this);
            newElements.UpsertElement(3, ConsoleElementIdentity.CurrentDescription, GetCurrentDescription(), this);
            newElements.UpsertElement(4, ConsoleElementIdentity.HistoryLog, currentPlayerController.CurrentHistoryLog.GetHistoryLog(historyLineMaxCount), this);
            newElements.ChangePriority(5, ConsoleElementIdentity.AvailableChoices, this);

            return newElements;
         }

        
        protected override string GenerateTopStatusBar()
        {
            string topStatusPrefix = " ╔══════════════════════════════════════════════════════════════════════════════════" + "══════-=════-=═=-=--=-=-- - - -";
            string topStatusInfix = "\n ║ ";
            string topStatusBar = $"{PlayerStatusService.GetPlayerLocation(currentPlayerController)}\t -=-\t Potsun Burran\t -=-\t Relative, {PlayerStatusService.GetPlayerID(currentPlayerController)}\t -=-\t {TimeService.GetServerTime()}";

            string concatenatedStatusBar = topStatusPrefix + topStatusInfix + topStatusBar;

            return concatenatedStatusBar;
        }

        protected override string GenerateTransitionalAction()
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


            List<string> transitionalActionLines = ConsoleUtilities.SplitIntoLines(transitionalAction, transitionalMidPrefix, transitionalMidPrefix, transitionalMidPrefix, 84);

            foreach (string line in transitionalActionLines)
            {
                if (longestLineCount < line.Length)
                    longestLineCount = line.Length - transitionalMidPrefix.Length;
            }

            //bottomPadding
            for (int i = 0; i <= 9 + (longestLineCount - transitionalBottomPrefix.Length); i++)
                bottomPadding += transitionalBottomInfix;

            //bottomEnding
            for (int i = 0; i <= bottomLength - (transitionalBottomPrefix.Length + bottomPadding.Length + transitionalBottomMark.Length); i++)
                bottomEnding += transitionalBottomSuffix;

            foreach (string transitionalActionContent in transitionalActionLines)
            {
                middlePadding = "";

                //middlePadding
                while (transitionalActionContent.Length + middlePadding.Length < bottomLength - transitionalBottomMark.Length - bottomEnding.Length)
                    middlePadding += transitionalMidInfix;

                //concatenatedMid
                concatenatedMid += transitionalActionContent + middlePadding + transitionalMidSuffix;
            }

            //topPadding
            while (transitionalTopPrefix.Length + topPadding.Length - transitionalTopSuffix.Length < bottomLength - transitionalBottomMark.Length - bottomEnding.Length)
                topPadding += transitionalTopInfix;

            concatenatedTop = transitionalTopPrefix + topPadding + transitionalTopSuffix;
            concatenatedBottom = transitionalBottomPrefix + bottomPadding + transitionalBottomMark + bottomEnding + bottomEndingSuffix;

            return concatenatedTop + concatenatedMid + concatenatedBottom;
        }

        private string GetCurrentDescription()
        {
            List<string> roomLines = ConsoleUtilities.SplitIntoLines(PlayerStatusService.GetPlayerLocationDescription(currentPlayerController), "\n ╟ ", "\n ║ ", "\n ╟ ");

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
    }
}
