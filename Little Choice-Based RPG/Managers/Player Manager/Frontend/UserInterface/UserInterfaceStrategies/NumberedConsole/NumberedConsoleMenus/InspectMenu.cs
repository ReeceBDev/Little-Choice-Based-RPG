using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface.UserInterfaceStyles;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.ImmaterialEntities.System;
using Little_Choice_Based_RPG.Resources;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using Little_Choice_Based_RPG.Types.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface.UserInterfaceHandler;
using static Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.InteractionUsingNothing;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.TripleParameterDelegates;
using static Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.TripleParameterDelegates.InteractionUsingGameObjectAndCurrentPlayerAndCurrentRoom;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PrivateInteractionsSystems.PrivateInteractionsExtensions;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory;
using Little_Choice_Based_RPG.Resources.Systems.InformationalSystems.Descriptor;
using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface.UserInterfaceStrategies.NumberedConsole.NumberedConsoleMenus;
using Little_Choice_Based_RPG.Types.MenuElements;

namespace Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface.UserInterfaceStrategies.NumberedConsole
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
            allInteractions.AddRange(InteractionController.GetInteractions(currentTargetInspectee));

            //Private interactions from the player, about the target
            allInteractions.AddRange(InteractionController.GetPrivateInteractions(currentPlayerController.CurrentPlayer, currentTargetInspectee));

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

        protected override MenuElementList GenerateMenuElements(out List<IInvokableInteraction> orderedInteractions)
        {
            MenuElementList newElements = base.GenerateMenuElements(out orderedInteractions);

            //Add an element containing inspect data for the target GameObject.
            newElements.UpsertElement(2, MenuElementIdentity.TargetData, GenerateTargetInspectData(), this);
            newElements.ChangePriority(3, MenuElementIdentity.TransitionalAction, this);

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
            inspectData.Add((string) DescriptorProcessor.GetDescriptor(currentTargetInspectee, "Descriptor.Inspect.Current"));
            inspectData.Add("Stats:");

            //Format every property except descriptors:
            foreach (EntityProperty property in currentTargetInspectee.Properties.EntityProperties.Where((i => !i.PropertyName.ToString().StartsWith("Descriptor."))))
                inspectData.Add($" - {property.PropertyName}   --   {property.PropertyValue.ToString()}");

            //Format the data elements
            foreach (var entry in inspectData)
                concatenatedInspectData += $"\n {entry}";

            formattedData = UserInterfaceUtilities.SplitIntoLines(concatenatedInspectData, "\n ╟ ", "\n ║ ", "\n ╟", 100);

            //Concatenate the lines into one string
            foreach (var entry in formattedData)
                concatenatedFormattedData += entry;

            return concatenatedFormattedData;
        }
    }
}
