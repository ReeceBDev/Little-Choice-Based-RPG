namespace Little_Choice_Based_RPG.External.ConsoleEndpoint.ConsoleMenus
{
    // Outlines the possible interface styles to be used by the ConsoleEndpoint
    public interface IConsoleMenu
    {
        static uint uniqueIdentifier;
        public void RunMenu();

        public bool ExitMenu { get; set; }
        /*
        public IInvokableInteraction RequestUserChoosesInteraction(List<IInvokableInteraction> possibleInteractionChoices);
        public GameObject RequestUserChoosesGameObject(List<EntityProperty>? propertyFilter = null);
        */


        /*
                 //string lastActionDescription = player.LastActionDescriptor.Value;
                    //string listChoices = ListChoices();
        private protected string lastActionDescription;
        private protected string listChoices;

        //int choiceindex = Convert.ToInt32(userInput);



        lastActionDescription = CurrentChoiceHandler.Choices.ElementAt(choiceindex).OnExecute?.Invoke();

            if (lastActionDescription == null)
                return;
            Console.WriteLine(lastActionDescription);


        }


        public string lastActionDescription = "";

        public ChoiceHandler CurrentChoiceHandler { get; init; } = new ChoiceHandler();
        */
    }
}
