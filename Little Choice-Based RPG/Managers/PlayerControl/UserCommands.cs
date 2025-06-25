using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory.InventoryExtensions;

namespace Little_Choice_Based_RPG.Managers.PlayerControl
{
    public class UserCommands
    {
        private static PlayerController playerController;
        public static bool TryCommand(string userInput, PlayerController setCurrentPlayerController, out string? errorMessage)
        {
            playerController = setCurrentPlayerController;
            string? possibleErrorMessage = null;

            errorMessage = null;


            //SAY - Broadcasts a message to other players in the room.
            if (Say(userInput, out possibleErrorMessage))
                return true;
            errorMessage ??= possibleErrorMessage;

            //ADVERT - Broadcasts a message to all players in the game, in-character.
            if (Advert(userInput, out possibleErrorMessage))
                return true;
            errorMessage ??= possibleErrorMessage;

            //OOC - Broadcasts a message to all players in the game, out of character.
            if (OOC(userInput, out possibleErrorMessage))
                return true;
            errorMessage ??= possibleErrorMessage;

            //HELP - Shows a list of valid commands.
            if (Help(userInput, out possibleErrorMessage))
                return true;
            errorMessage ??= possibleErrorMessage;

            //RAINBOW - Produces a rainbow piece of text, for fun.
            if (Rainbow(userInput, out possibleErrorMessage))
                return true;
            errorMessage ??= possibleErrorMessage;


            return false;
        }

        /// <summary> SAY - Broadcasts a message to other players in the room. </summary>
        private static bool Say(string userInput, out string? errorMessage)
        {
            errorMessage = null;

            if (userInput.ToUpper().StartsWith("SAY")) //say command
            {
                string? userMessage = userInput.Substring("SAY".Length).Trim();
                string completedSentence = null;
                string previousSentence = null;

                if (!string.IsNullOrWhiteSpace(userMessage))
                {
                    string playerName = (string)playerController.CurrentPlayer.Properties.GetPropertyValue("Name");

                    //Format the userMessage properly as a sentence.
                    if (userMessage.Any(char.IsAsciiLetter)) //Check whether it even contains letters, first.
                    {
                        //Ensure the userMessage ends in a full stop.
                        string formattedUserMessage = $"{(userMessage.EndsWith(".") ? $"{userMessage}" : $"{userMessage}.")}";


                        //Correctly format sentences, delimited by fullstops.
                        while (formattedUserMessage.Contains(".")) //Make sure to check here that the index even goes one higher... post whitespace (end trim), of course.
                        {
                            //Initialise completedSentence if null, so that it may work with += operators.
                            //Doing this here ensures that the fallback ??= operators for this variable work.
                            //(And we know completedSentence will be set here, anyway.)
                            completedSentence ??= string.Empty;

                            //Set the first letter as a capital, then add a full stop at the end if one is not present.
                            formattedUserMessage =
                                $"{(char.IsAsciiLetterLower(formattedUserMessage[0]) ? $"{formattedUserMessage[0].ToString().ToUpper()}" : $"{formattedUserMessage[0]}")}" +
                                $"{formattedUserMessage.Substring(1)}" +
                                $"{(formattedUserMessage.EndsWith(".") ? "" : ".")}";

                            //Get the next fullstop
                            int dotIndex = formattedUserMessage.IndexOf(".");

                            //Cache anything before the next fullstop, as a sentence.
                            previousSentence = formattedUserMessage.Remove(dotIndex);
                            completedSentence += previousSentence;

                            //Then delete up to and including the next full stop.
                            formattedUserMessage = formattedUserMessage.Remove(0, dotIndex + 1).Trim();
                            //Add the full stop in again, here, to avoid out of bounds.- but only onto the end result, to force continuity.
                            completedSentence += ".";

                            //If there is a letter or number next, add a space to the completed entries.
                            //A letter will capitalise on the next iteration.
                            if (!string.IsNullOrEmpty(formattedUserMessage))
                                if (char.IsAsciiLetterOrDigit(formattedUserMessage[0]))
                                    completedSentence += "\u0020";
                        }

                        completedSentence ??= formattedUserMessage;
                    }

                    completedSentence ??= userMessage;

                    //The above if statement should look something like the below after every iteration.
                    //FormattedMessage: Hello there. i am a bean.
                    //PreviousSentence: Hello there. I
                    //remainingMessage: _am a bean.

                    string newSay = $"{playerName} says \"{completedSentence}\"";
                    ((ItemContainer) playerController.CurrentRoom.Extensions.Get("ItemContainer")).NewLocalUserMessage(newSay);

                    return true;
                }

                errorMessage = "You tried to write a message using \"Say\", but you have to put something after the command!";
            }
            return false;
        }

        /// <summary> ADVERT - Broadcasts a message to all players in the game, in-character. </summary>
        private static bool Advert(string userInput, out string? errorMessage)
        {
            errorMessage = null;

            if (userInput.ToUpper().StartsWith("ADVERT")) //advert command
            {
                string? userMessage = userInput.Substring("ADVERT".Length);

                if (!string.IsNullOrWhiteSpace(userMessage))
                {
                    string playerName = (string)playerController.CurrentPlayer.Properties.GetPropertyValue("Name");
                    string newAdvert = $"§e[ADVERTISEMENT] {playerName}: {userMessage}";

                    ItemContainer.NewGlobalUserMessage(newAdvert);

                    return true;
                }

                errorMessage = "You tried to write a message using \"Advert\", but you have to put something after the command!";
            }
            return false;
        }

        /// <summary> OOC - Broadcasts a message to all players in the game, out of character. </summary>
        private static bool OOC(string userInput, out string? errorMessage)
        {
            errorMessage = null;

            if (userInput.ToUpper().StartsWith("OOC")) //say command
            {
                string? userMessage = userInput.Substring("OOC".Length);

                if (!string.IsNullOrWhiteSpace(userMessage))
                {
                    string playerName = (string)playerController.CurrentPlayer.Properties.GetPropertyValue("Name");
                    string newOOC = $"§7(OOC) {playerName} - {userMessage}";

                    ItemContainer.NewGlobalUserMessage(newOOC);

                    return true;
                }

                errorMessage = "You tried to write a message using \"Advert\", but you have to put something after the command!";
            }

            return false;
        }

        /// <summary> HELP - Shows a list of valid commands. </summary>
        private static bool Help(string userInput, out string? errorMessage)
        {
            errorMessage = null;

            if (userInput.ToUpper().Equals("HELP"))
            {
                ItemContainer.NewGlobalUserMessage(
                    "Valid Commands:" +
                    "\n - Say (\"Your message goes here\") - Broadcasts a message to other players in the room." +
                    "\n - Advert (\"Your message goes here\") - Broadcasts a message to all players in the game, in-character." +
                    "\n - OOC (\"Your message goes here\") - Broadcasts a message to all players in the game, out of character" +
                    "\n - RAINBOW - Produces a rainbow piece of text, for fun."
                    );

                return true;
            }

            return false;
        }

        /// <summary> RAINBOW - Produces a rainbow piece of text, for fun. </summary>
        private static bool Rainbow(string userInput, out string? errorMessage)
        {
            errorMessage = null;

            if (userInput.ToUpper().Equals("RAINBOW"))
            {
                errorMessage = "§9I §2A§eM §4A §aR§5A§dI§3N§cB§6O§bW";
                return true;
            }

            return false;
        }
    }
}
