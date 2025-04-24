using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory;
using Little_Choice_Based_RPG.Resources.Systems.InformationalSystems.Descriptor;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates.InteractionUsingNothing;

namespace Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface.UserInterfaceStrategies.NumberedConsole.NumberedConsoleMenus.NumberedConsoleSubMenus
{
    public static class OpenContainerSubMenu
    {
        public static void GenerateOpenContainerSubMenu(GameObject targetContainer)
        {/* UNCOMMENT ME WHEN READY TO IMPLEMENT :)
            while (!openContainerSubMenuAborted)
            {
                //These are all of the choices, by the way
                List<IInvokableInteraction> containerPickups = InventoryRoomUpdateHandler.ReturnPlayerItemContainerContentsPickups(currentPlayerController.CurrentPlayer, targetContainer);
                containerPickups.Add(InventoryDelegation.GenerateMoveIntoGameObjectContainer(currentPlayerController.CurrentPlayer, targetContainer));

                //Add the close option
                openContainerSubMenuSystemChoices.Add(GenerateOpenContainerSubMenuCloseChoice(targetContainer));

                //Update the submenu text cache, or initiliase it if its not yet been set.
                if (openContainerSubMenuText.Count() == 0)
                {
                    List<MenuElement> listToWrite = InitialiseOpenContainerSubMenuTextEntries(targetContainer, containerPickups);
                    this.openContainerSubMenuText = listToWrite;
                }
                else
                {
                    //Update the cached output with the new content.
                    MenuElement targetComponent = GetTextEntryByIdentifier(MenuElementIdentity.OpenContainerSubMenuTitle);
                    int targetIndex = openContainerSubMenuText.IndexOf(targetComponent);

                    this.openContainerSubMenuText[targetIndex].Content = SetOpenContainerSubMenuHeader(targetContainer.Properties.GetPropertyValue("Name").ToString());

                    targetComponent = GetTextEntryByIdentifier(MenuElementIdentity.OpenContainerSubMenuTitle);
                    targetIndex = openContainerSubMenuText.IndexOf(targetComponent);

                    this.openContainerSubMenuText[targetIndex].Content = FormatOpenContainerSubMenuChoices(containerPickups);
                }

                //Draw the SubMenu
                this.openContainerSubMenuActive = true;
                DrawUserInterface();

                //Await the user's answer.
                int userInput = MenuInputLogic.AwaitUserInput(containerPickups.Count + openContainerSubMenuSystemChoices.Count, drawUserInterfaceDelegate);

                //Invoke the selected choice
                if (userInput <= containerPickups.Count)
                    InvokeInteraction(userInput, containerPickups);

                if (userInput >= containerPickups.Count)
                    InvokeInteraction(userInput - containerPickups.Count, openContainerSubMenuSystemChoices);
            }

            //Once aborted, reset
            openContainerSubMenuSystemChoices.Clear();
            openContainerSubMenuAborted = false;
            openContainerSubMenuActive = false;
        }

        public static List<MenuElement> InitialiseOpenContainerSubMenuTextEntries(GameObject targetContainer, List<IInvokableInteraction> containerPickups)
        {
            List<MenuElement> overwrittenInterfaceEntries = mainBodyText;

            if (!(overwrittenInterfaceEntries.Count < 1))
                throw new Exception($"The required list of overwrittenInterfaceEntries does not contain enough lines to be used here. It might not have been initialised. There must be an existing user interface to draw the sub-menu over the top of!");


            List<MenuElement> subMenu = new List<MenuElement>(overwrittenInterfaceEntries.SkipLast(3));

            subMenu.Append(new MenuElement(MenuElementIdentity.OpenContainerSubMenuTitle, SetOpenContainerSubMenuHeader(targetContainer.Properties.GetPropertyValue("Name").ToString()), 0));
            subMenu.Append(new MenuElement(MenuElementIdentity.OpenContainerSubMenuOptions, FormatOpenContainerSubMenuChoices(containerPickups), 0));

            subMenu.Append(new MenuElement(MenuElementIdentity.None, "═══════-=════-=═=-=--=-=-- - - -", 0));
            subMenu.Append(new MenuElement(MenuElementIdentity.None, " >> > > ", 0));

            return subMenu;
        }

        private static IInvokableInteraction GenerateOpenContainerSubMenuCloseChoice(GameObject targetContainer)
        {
            if (!targetContainer.Properties.HasExistingPropertyName("Descriptor.InventorySystem.Interaction.Close.Title"))
                throw new Exception($"Tried to create a close interaction for the currently open container, but {targetContainer} didn't have a property matching Descriptor.InventorySystem.Interaction.Close.Title!");

            if (!targetContainer.Properties.HasExistingPropertyName("Descriptor.InventorySystem.Interaction.Close.Invoking"))
                throw new Exception($"Tried to create a close interaction for the currently open container, but {targetContainer} didn't have a property matching Descriptor.InventorySystem.Interaction.Close.Invoking!");

            InteractionUsingNothingDelegate abortOpenContainerDelegate = AbortOpenContainerSubMenu;

            InteractionUsingNothing abortOpenContainer = new InteractionUsingNothing
                (
                    abortOpenContainerDelegate,
                    currentPlayerController.CurrentPlayer,
                    DescriptorLogic.GetDescriptor(targetContainer, "InventorySystem.Interaction.Close.Title"),
                    DescriptorLogic.GetDescriptor(targetContainer, "InventorySystem.Interaction.Close.Invoking")
                );

            return abortOpenContainer;
        }

        private static string SetOpenContainerSubMenuHeader(string title)
        {
            string subMenuHeader = "";

            subMenuHeader += $"\n == +] ======][========][======= {title} =======][======][=======. =- = - -[ =  - .";
            subMenuHeader += $"\n  . - = -- - - ===========-===========================--======-=---=-. ---=== =-----. -  - .";
            subMenuHeader += $"\n";
            subMenuHeader += $"\n  .- =  --  - =-==--==--  --==--  --===-==- -=====-=-  -======-=---=-. ---=== =--=--. -  - .";

            return subMenuHeader;
            */
        }
    }
}
