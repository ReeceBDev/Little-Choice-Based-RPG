using LCBRPG_User_Console.ConsoleMenus;
using LCBRPG_User_Console.MenuResource;
using LCBRPG_User_Console.Types;
using Little_Choice_Based_RPG.External.Types;
using Little_Choice_Based_RPG.Managers.PlayerControl;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.SingleParameterDelegates;
using System.Dynamic;

namespace LCBRPG_User_Console.ConsoleUtilities
{
    internal static class SystemChoices
    {
        private delegate void ChangeMenu(IConsoleMenu oldMenu, ChangeInterfaceCallback menuCallback, IConsoleMenu newMenu);
        private delegate void OpenInspectMenu(ConsoleEndpoint userEndpoint, LocalPlayerSession setPlayerSession, InteractionCache setInteractionCache, HistoryLogCache setHistoryLogCache, uint inspecteeID);
        private delegate void SeededDelegate(params object[] args); //Provides a variadic delegate.

        private static ulong interactionCounter; //Used to incrementally create IDs. Ensures IDs are never the same.

        public static void SwitchMenuLogic(IConsoleMenu oldMenu, ChangeInterfaceCallback menuCallback, IConsoleMenu newMenu)
        {
            menuCallback(newMenu);
            oldMenu.ExitMenu = true;
        }
    }
}
