using Little_Choice_Based_RPG.Managers.PlayerControl;
using Little_Choice_Based_RPG.Types.External.ConsoleElements;

namespace Little_Choice_Based_RPG.External.ConsoleEndpoint.ConsoleFunctionalities
{
    public static class UserInputElement
    {
        public static int? AwaitUserInput(int numberOfInteractions, DrawMenuCallbackDelegate drawUserInterfaceDelegate, ConsoleElementList callbackArgs, PlayerController currentPlayerController, string? newUserMessage = null)
        {
            int? userInput = null; //Invalid input
            string userMessage = "Unknown user input error in MenuInputLogic.AwaitUserInput().";

            bool sendMessage = newUserMessage is null ? false : true;

            if (newUserMessage is not null)
                userMessage = newUserMessage;

            const int minimumInputValue = 0;
            int maximumInputValue = numberOfInteractions;

            //Loop until a choice is selected.
            while (userInput is null)
            {
                //Send error message or write normal input.
                if (sendMessage == false)
                    WriteUserEntry();
                else
                    WriteUserError(userMessage, drawUserInterfaceDelegate, callbackArgs);

                //Reset error message flag.
                sendMessage = false;

                //Await user input
                if (!TryUserInput(Console.ReadLine().Trim(), minimumInputValue, maximumInputValue, currentPlayerController, out userInput, out userMessage))
                    sendMessage = true;
            }

            return userInput;
        }

        private static bool TryUserInput(string? userInput, int minimumInputValue, int maximumInputValue, PlayerController currentPlayerController, out int? validInput, out string? errorMessage)
        {
            const int userIndexOffset = -1;

            errorMessage = null; //Do not add a default value here. The errorMessage under the Char.IsDigit check musn't overwrite the out errorMessage of the if(UserCommands.TryCommand()).
            validInput = null;
            int? userSelection = null;

            //Validate user input
            if (userInput == "" || userInput is null) //input must not be empty
            {
                errorMessage ??= "Please enter the number of your choice. Pressing enter will do nothing on its own!";
                return false;
            }

            if (!userInput.All(char.IsDigit)) //input may be a command
            {
                //Check if it was a valid command.
                if (UserCommands.TryCommand(userInput, currentPlayerController, out errorMessage))
                    return true;

                errorMessage ??= "Please enter a number or a command and nothing else. Trying to enter other things won't have any use!";
                return false;
            }
            else //if input is a number, check for numbered choices
                userSelection = Convert.ToInt32(userInput) + userIndexOffset;

            if (!(minimumInputValue <= userSelection && userSelection <= maximumInputValue)) //input must match the range of choices.
            {
                errorMessage = "Please choose a number from the available choices. Trying a number not listed will have nothing to do!";
                return false;
            }

            validInput = userSelection;
            return validInput is not null ? true : throw new Exception("Tried to return true but validInput was still null! Was userSelection still null?? That shouldn't have happened!");
        }

        private static void WriteUserEntry()
        {
            ConsoleUtilities.WriteDialogue("\n ║\n ║\n ╚", 160);
            ConsoleUtilities.WriteDialogue("═>> Choose an option: > ", 5);
        }

        private static void WriteUserError(string message, DrawMenuCallbackDelegate drawUserInterfaceDelegate, ConsoleElementList callbackArgs)
        {
            Console.Clear();

            drawUserInterfaceDelegate(callbackArgs);

            ConsoleUtilities.WriteDialogue("\n ╠", 160);
            ConsoleUtilities.WriteDialogue(" Hang on: ", 0);
            ConsoleUtilities.WriteDialogue("\n ╠", 160);
            ConsoleUtilities.WriteDialogue($" {message}", 4);
            ConsoleUtilities.WriteDialogue("\n ╚", 160);
            ConsoleUtilities.WriteDialogue("═>> Choose an option: > ", 5);
        }        

        public delegate void DrawMenuCallbackDelegate(ConsoleElementList targetElements);
    }
}
