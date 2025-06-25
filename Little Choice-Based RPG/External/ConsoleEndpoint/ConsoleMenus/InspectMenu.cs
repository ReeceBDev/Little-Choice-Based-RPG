using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Resources.Systems.InformationalSystems.Descriptor;
using Little_Choice_Based_RPG.Types.External.ConsoleElements;
using Little_Choice_Based_RPG.Managers.PlayerControl;
using Little_Choice_Based_RPG.External.ConsoleEndpoint.ConsoleFunctionalities;

namespace Little_Choice_Based_RPG.External.ConsoleEndpoint.ConsoleMenus
{
    public class InspectMenu : NumberedConsoleMenu
    {
        private GameObject currentTargetInspectee;

        private List<string> currentTextEntries = new();
        private List<string> currentChoiceEntries = new();
        private List<IInvokableInteraction> currentTargetInteractions = new();
        private List<IInvokableInteraction> currentSystemInteractions = new();


        public InspectMenu(PlayerController setPlayerController, GameObject setTargetInspectee) : base(setPlayerController)
        {
            currentTargetInspectee = setTargetInspectee;
        }

        protected override List<IInvokableInteraction> GetMenuInteractions()
        {
            List<IInvokableInteraction> allInteractions = base.GetMenuInteractions();

            //Public Interactions from the target itself
            allInteractions.AddRange(InteractionRetriever.GetInteractions(currentTargetInspectee));

            //Private interactions from the player, about the target
            allInteractions.AddRange(InteractionRetriever.GetPrivateInteractions(currentPlayerController.CurrentPlayer, currentTargetInspectee));

            return allInteractions;
        }

        protected override List<IInvokableInteraction> InitialiseSystemChoices()
        {
            List<IInvokableInteraction> newSystemChoices = base.InitialiseSystemChoices();

            //Add a choice which returns to the player's inventory
            newSystemChoices.Add(SystemChoices.ReturnToInventoryMenu);

            //Add a choice to switch back to the exploration menu.
            newSystemChoices.Add(SystemChoices.SwitchToExploreMenu);

            return newSystemChoices;
        }

        protected override ConsoleElementList GenerateMenuElements(out List<IInvokableInteraction> orderedInteractions)
        {
            ConsoleElementList newElements = base.GenerateMenuElements(out orderedInteractions);

            //Add an element containing inspect data for the target GameObject.
            newElements.UpsertElement(2, ConsoleElementIdentity.TargetData, GenerateTargetInspectData(), this);
            newElements.ChangePriority(3, ConsoleElementIdentity.TransitionalAction, this);

            return newElements;
        }

        protected override string GenerateTopStatusBar()
        {
            string topStatusPrefix = " ╔══════════════════════════════════════════════════════════════════════════════════" + "══════-=════-=═=-=--=-=-- - - -";
            string topStatusInfix = "\n ║ ";
            string topStatusBar = $"{currentTargetInspectee.Properties.GetPropertyValue("Name")}" +
                $"\t -=-\t INSPECT VIEW \t -=-\t -=════- \t -=- \t {DateTime.UtcNow.AddYears(641)}";

            string concatenatedStatusBar = topStatusPrefix + topStatusInfix + topStatusBar;

            return concatenatedStatusBar;
        }

        private string GenerateTargetInspectData()
        {
            List<string> inspectData = new();
            List<string> formattedData = new();
            string concatenatedInspectData = string.Empty;
            string concatenatedFormattedData = string.Empty;

            //Add each data element to the inspectData
            inspectData.Add((string) currentTargetInspectee.Properties.GetPropertyValue("Name"));
            inspectData.Add((string) currentTargetInspectee.Properties.GetPropertyValue("Type"));
            inspectData.Add( DescriptorProcessor.GetDescriptor(currentTargetInspectee, "Descriptor.Inspect.Current"));
            inspectData.Add("Stats:");

            //Format every property except descriptors:
            foreach (EntityProperty property in currentTargetInspectee.Properties.EntityProperties.Where(i => !i.PropertyName.ToString().StartsWith("Descriptor.")))
                inspectData.Add($" - {property.PropertyName}   --   {property.PropertyValue.ToString()}");

            //Format the data elements
            foreach (var entry in inspectData)
                concatenatedInspectData += $"\n {entry}";

            formattedData = ConsoleUtilities.SplitIntoLines(concatenatedInspectData, "\n ╟ ", "\n ║ ", "\n ╟", 100);

            //Concatenate the lines into one string
            foreach (var entry in formattedData)
                concatenatedFormattedData += entry;

            return concatenatedFormattedData;
        }
    }
}
