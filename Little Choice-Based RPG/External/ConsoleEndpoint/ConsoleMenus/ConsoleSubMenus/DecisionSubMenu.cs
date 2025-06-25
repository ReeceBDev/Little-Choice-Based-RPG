using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;

namespace Little_Choice_Based_RPG.External.ConsoleEndpoint.ConsoleMenus.ConsoleSubMenus
{
    public static class DecisionSubMenu
    {
        /// <summary> Generates a Sub-Menu asking the player to choose an object from the room, which optionally matches property filters. </summary>
        private static void GenerateRequestSubMenu(IInvokableInteraction sender, string requirementDescription, IInvokableInteraction abortInteraction, List<EntityProperty>? setFilters = null)
        {
            /*
            //Reset submenu
            requestSubMenuSystemChoices.Clear();

            List<GameObject> possibleObjects =
                InventoryLogic.GetInventoryEntities(currentPlayerController.CurrentRoom, setFilters);
            //List<GameObject> possibleObjects = currentPlayerController.CurrentRoom.GetRoomObjects(setFilters);
            List<GameObject> listedObjects = new List<GameObject>();

            requestSubMenuSystemChoices.Add(abortInteraction);

            //Update the submenu text cache, or initiliase it if its not yet been set.
            if (requestSubMenuText.Count() == 0)
            {
                List<MenuElement> listToWrite = InitialiseRequestSubMenuTextEntries(mainBodyText, requirementDescription, listedObjects);
                this.requestSubMenuText = listToWrite;
            }
            else
            {
                //Update the cached output with the new content.
                MenuElement targetComponent = GetTextEntryByIdentifier(MenuElementIdentity.RequestSubMenuTitle);
                int targetIndex = requestSubMenuText.IndexOf(targetComponent);

                this.requestSubMenuText[targetIndex].Content = SetRequestSubMenuHeader(requirementDescription);

                targetComponent = GetTextEntryByIdentifier(MenuElementIdentity.RequestSubMenuOptions);
                targetIndex = requestSubMenuText.IndexOf(targetComponent);

                this.requestSubMenuText[targetIndex].Content = FormatRequestSubMenuChoices(listedObjects);
            }

            //Draw the SubMenu
            this.requestSubMenuActive = true;
            DrawUserInterface();

            //Await the user's answer.
            int userInput = MenuInputLogic.AwaitUserInput(listedObjects.Count + requestSubMenuSystemChoices.Count, drawUserInterfaceDelegate);

            if (userInput <= (choiceIndexOffset + (listedObjects.Count - 1)))
            {
                GameObject chosenObject = listedObjects.ElementAt(userInput - choiceIndexOffset);

                sender.GiveRequiredParameter(chosenObject, currentPlayerController);

                //Reset the additional SubMenu choice
                requestSubMenuSystemChoices.Remove(abortInteraction);
            }

            if (userInput > (choiceIndexOffset + (listedObjects.Count - 1)))
            {

                requestSubMenuSystemChoices.ElementAt(userInput - (listedObjects.Count + choiceIndexOffset));
                abortInteraction.AttemptInvoke(currentPlayerController);

                //Reset the additional SubMenu choice
                requestSubMenuSystemChoices.Remove(abortInteraction);
            }

            //Deactivate the SubMenu
            this.requestSubMenuActive = false;
        }

        public static List<MenuElement> InitialiseRequestSubMenuTextEntries(List<MenuElement> overwrittenInterfaceEntries, string requirementDescription, List<GameObject> listedObjects)
        {
            if (!(overwrittenInterfaceEntries.Count < 1))
                throw new Exception($"The required list of overwrittenInterfaceEntries does not contain enough lines to be used here. It might not have been initialised. There must be an existing user interface to draw the sub-menu over the top of!");

            List<MenuElement> subMenu = new List<MenuElement>(overwrittenInterfaceEntries.SkipLast(3));

            subMenu.Append(new MenuElement(MenuElementIdentity.RequestSubMenuTitle, SetRequestSubMenuHeader(requirementDescription), 0));
            subMenu.Append(new MenuElement(MenuElementIdentity.RequestSubMenuOptions, FormatRequestSubMenuChoices(listedObjects), 0));
            subMenu.Append(new MenuElement(MenuElementIdentity.None, FormatChoices(requestSubMenuSystemChoices), 0));

            subMenu.Append(new MenuElement(MenuElementIdentity.None, "═══════-=════-=═=-=--=-=-- - - -", 0));
            subMenu.Append(new MenuElement(MenuElementIdentity.None, " >> > > ", 0));

            return subMenu;
        }
        private static string SetRequestSubMenuHeader(string title)
        {
            string subMenuHeader = "";

            subMenuHeader += $"\n == +] ======][========][======= MAKE A SELECTION =======][======][=======. =- = - -[ =  - .";
            subMenuHeader += $"\n  . - = -- - - ===========-===========================--======-=---=-. ---=== =-----. -  - .";
            subMenuHeader += $"\n                          {title}";
            subMenuHeader += $"\n  .- =  --  - =-==--==--  --==--  --===-==- -=====-=-  -======-=---=-. ---=== =--=--. -  - .";

            return subMenuHeader;
            */
        }
    }
}
