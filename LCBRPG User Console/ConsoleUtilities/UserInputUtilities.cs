        using LCBRPG_User_Console.Types;
using LCBRPG_User_Console.Types.DisplayData;
using Little_Choice_Based_RPG.External.EndpointServices;
using Little_Choice_Based_RPG.External.Types;
using Microsoft.VisualBasic;

namespace LCBRPG_User_Console.ConsoleUtilities
{
    internal static class UserInputUtilities
    {
        private const int choiceIndexOffset = 1;

        public static void HandleUserInput(UserInputService inputService, List<InteractionDisplayData> availableInteractions, 
            DrawMenuCallbackDelegate drawUserInterfaceDelegate, DisplayDataList callbackArgs, int inputLockCounter, List<string> allowedCommands,
            Dictionary<ulong, Action> seededSystemInteractions)
        {
            AwaitUserInputLoop(availableInteractions, drawUserInterfaceDelegate, callbackArgs, inputLockCounter, inputService, allowedCommands, seededSystemInteractions);
        }

        private static void AwaitUserInputLoop(List<InteractionDisplayData> availableInteractions, DrawMenuCallbackDelegate drawUserInterfaceDelegate, 
            DisplayDataList callbackArgs, int inputLockCounter, UserInputService inputService, List<string> allowedCommands, 
            Dictionary<ulong, Action> seededSystemInteractions)
        {
            string sanitisedInput;

            object? validatedInput = null; //Invalid input

            string errorMessage = "Unknown user input error in MenuInputLogic.AwaitUserInput().";
            bool errorMessageAvailable = false;

            //Loop until an interaction is selected.
            while (validatedInput is null)
            {
                //Send error message or write normal input.
                if (errorMessageAvailable == false)
                    WriteUserEntry();
                else
                    WriteUserError(errorMessage, drawUserInterfaceDelegate, callbackArgs);

                //Reset error message flag.
                errorMessageAvailable = false;

                if (!ValidateInput(Console.ReadLine().Trim(), availableInteractions, inputLockCounter, inputService, allowedCommands, seededSystemInteractions, 
                    out validatedInput, out errorMessage))
                    errorMessageAvailable = true;
            }
        }

        private static bool ValidateInput(string? userInput, List<InteractionDisplayData> availableInteractions, int inputLockCounter, 
            UserInputService inputService, List<string> allowedCommands, Dictionary<ulong, Action> seededSystemInteractions, 
            out object? validatedUserInput, out string? errorMessage)
        {
            const int userIndexOffset = -1;

            List<InteractionDisplayData> interactionFrozenCache = new List<InteractionDisplayData>(availableInteractions);
            ulong interactionID;
            int userSelection;

            errorMessage = null; //Do not add a default value here. The errorMessage under the Char.IsDigit check mustn't overwrite the out errorMessage of the if(UserCommands.TryCommand()).
            validatedUserInput = null;

            //Validate user input
            if (userInput == "" || userInput is null) //input must not be empty
            {
                errorMessage ??= "Please enter the number of your choice. Pressing enter will do nothing on its own!";
                return false;
            }

            if (!userInput.All(char.IsDigit)) //input may be a command
            {
                //Check if it was a valid command.
                if (TestCommandIsValid(userInput, allowedCommands))
                { 
                    //Fire the command if valid, then return as true.
                    inputService.HandleUserCommand(userInput);
                    return true;
                }

                errorMessage ??= "Please enter a number or a command and nothing else. Trying to enter other things won't have any use!";
                return false;
            }
            else //if input is a number, check for numbered choices
                userSelection = Convert.ToInt32(userInput) + userIndexOffset;

            if (!(0 <= userSelection && userSelection <= interactionFrozenCache.Count)) //input must match the range of choices.
            {
                errorMessage = "Please choose a number from the available choices. Trying a number not listed will have nothing to do!";
                return false;
            }

            //Fire the valid interaction selection, then return as true.
            interactionID = interactionFrozenCache[userSelection].InteractionID;

            //Check if system interaction or game interaction and fire accordingly.
            if (interactionFrozenCache[userSelection].PresentationContext.Equals(SystemInteractionContext.UserConsole.ToString())) //UserConsole interaction
            {
                if (!seededSystemInteractions.TryGetValue(interactionID, out var seededDelegate))
                    throw new Exception("Selection not found within seededSystemInteractions.");

                seededDelegate();
            }
            else //Game Interaction
            {
                inputService.HandleUserInteraction(interactionID);
            }

            return true;
        }

        private static void WriteUserEntry()
        {
            WritelineUtilities.WriteDialogue("\n ║\n ║\n ╚", 160);
            WritelineUtilities.WriteDialogue("═>> Choose an option: > ", 5);
        }

        private static void WriteUserError(string message, DrawMenuCallbackDelegate drawUserInterfaceDelegate, DisplayDataList callbackArgs)
        {
            Console.Clear();

            drawUserInterfaceDelegate(callbackArgs);

            WritelineUtilities.WriteDialogue("\n ╠", 160);
            WritelineUtilities.WriteDialogue(" Hang on: ", 0);
            WritelineUtilities.WriteDialogue("\n ╠", 160);
            WritelineUtilities.WriteDialogue($" {message}", 4);
            WritelineUtilities.WriteDialogue("\n ╚", 160);
            WritelineUtilities.WriteDialogue("═>> Choose an option: > ", 5);
        }        

        private static bool TestCommandIsValid(string userInput, List<string> allowedCommands)
        {
            foreach (string command in allowedCommands) {
                if (userInput
                        .Trim()
                        .ToUpper()
                        .StartsWith(command.ToUpper()))
                    return true;
            }

            return false;
        }

        public delegate void DrawMenuCallbackDelegate(DisplayDataList callbackArgs);
    }
}
