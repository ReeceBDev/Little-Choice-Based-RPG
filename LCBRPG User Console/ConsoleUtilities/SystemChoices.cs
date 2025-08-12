using LCBRPG_User_Console.ConsoleMenus;
using Little_Choice_Based_RPG.External.ConsoleEndpoint.ConsoleMenus;
using Little_Choice_Based_RPG.External.Types;
using Little_Choice_Based_RPG.Managers.PlayerControl;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.SingleParameterDelegates;
using static Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.SingleParameterDelegates.SystemInteractionUsingCurrentPlayerController;

namespace LCBRPG_User_Console.ConsoleUtilities
{
    internal static class SystemChoices
    {
        static SystemChoices()
        {
            //Initialise ReturnToMainMenu
            SystemInteractionUsingCurrentPlayerControllerDelegate returnToMainMenu = new SystemInteractionUsingCurrentPlayerControllerDelegate(SwitchToMainMenuLogic);
            ReturnToMainMenu = new SystemInteractionUsingCurrentPlayerController(returnToMainMenu, "Go back to main menu.", "Returning to main menu...", InteractionRole.System);

            //Initialise SwitchToInventoryMenu
            SystemInteractionUsingCurrentPlayerControllerDelegate openInventoryMenu = new SystemInteractionUsingCurrentPlayerControllerDelegate(SwitchToInventoryMenuLogic);
            SwitchToInventoryMenu = new SystemInteractionUsingCurrentPlayerController(openInventoryMenu, "Open your inventory.", "Opening your inventory...", InteractionRole.System);

            //Initialise ReturnToInventoryMenu
            SystemInteractionUsingCurrentPlayerControllerDelegate returnToInventoryMenu = new SystemInteractionUsingCurrentPlayerControllerDelegate(SwitchToInventoryMenuLogic);
            ReturnToInventoryMenu = new SystemInteractionUsingCurrentPlayerController(returnToInventoryMenu, "Return to your inventory.", "Going back into your inventory...", InteractionRole.System);

            //Initialise LaunchExploreMenu
            SystemInteractionUsingCurrentPlayerControllerDelegate launchExploreMenu = new SystemInteractionUsingCurrentPlayerControllerDelegate(SwitchToExploreMenuLogic);
            LaunchExploreMenu = new SystemInteractionUsingCurrentPlayerController(launchExploreMenu, "-=- START THE GAME -=-", "Starting the game...", InteractionRole.System);

            //Initialise SwitchToExploreMenu
            SystemInteractionUsingCurrentPlayerControllerDelegate switchToExploreMenu = new SystemInteractionUsingCurrentPlayerControllerDelegate(SwitchToExploreMenuLogic);
            SwitchToExploreMenu = new SystemInteractionUsingCurrentPlayerController(switchToExploreMenu, "Close your inventory.", "Closing your inventory...", InteractionRole.System);
        }


        private static void SwitchToMainMenuLogic(PlayerController currentPlayerController)
        {
            ConsoleEndpoint currentInterfaceHandler = currentPlayerController.CurrentConsoleEndpoint;
            IConsoleMenu currentMenu = currentInterfaceHandler.CurrentMenu;

            currentInterfaceHandler.ChangeInterface(new MainMenu(currentPlayerController));
            currentMenu.ExitMenu = true;
        }

        private static void SwitchToExploreMenuLogic(PlayerController currentPlayerController)
        {
            ConsoleEndpoint currentInterfaceHandler = currentPlayerController.CurrentConsoleEndpoint;
            IConsoleMenu currentMenu = currentInterfaceHandler.CurrentMenu;

            currentInterfaceHandler.ChangeInterface(new ExploreMenu(currentPlayerController));
            currentMenu.ExitMenu = true;
        }

        private static void SwitchToInventoryMenuLogic(PlayerController currentPlayerController)
        {
            ConsoleEndpoint currentInterfaceHandler = currentPlayerController.CurrentConsoleEndpoint;
            IConsoleMenu currentMenu = currentInterfaceHandler.CurrentMenu;

            currentInterfaceHandler.ChangeInterface(new InventoryMenu(currentPlayerController));
            currentMenu.ExitMenu = true;
        }

        public static void SwitchToInspectMenuLogic(GameObject targetGameObject, PlayerController currentPlayerController)
        {
            ConsoleEndpoint currentInterfaceHandler = currentPlayerController.CurrentConsoleEndpoint;
            IConsoleMenu currentMenu = currentInterfaceHandler.CurrentMenu;

            currentInterfaceHandler.ChangeInterface(new InspectMenu(currentPlayerController, targetGameObject));
            currentMenu.ExitMenu = true;
        }

        /*
        public static void AbortOpenContainerSubMenuLogic(PlayerController currentPlayerController)
        {
            IUserInterface currentMenu = currentPlayerController.CurrentConsoleEndpoint.CurrentMenu;

            currentMenu.openContainerSubMenuAborted = true;
        }
        */


        public static IInvokableInteraction ReturnToMainMenu { get; private set; }
        public static IInvokableInteraction SwitchToInventoryMenu { get; private set; }
        public static IInvokableInteraction ReturnToInventoryMenu { get; private set; }
        public static IInvokableInteraction LaunchExploreMenu { get; private set; }
        public static IInvokableInteraction SwitchToExploreMenu { get; private set; }
    }
}
