using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface;
using Little_Choice_Based_RPG.Managers.World;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Types;
using Little_Choice_Based_RPG.Types.EntityProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Choices
{
    public class ChoiceHandler // List choices and read choices
    {
        public List<Choice> Choices = new List<Choice>(); //These will be listed as options

        public Player currentPlayer;
        public GameEnvironment currentEnvironment;

        public ChoiceHandler(Player setCurrentPlayer, GameEnvironment setCurrentEnvironment)
        {
            currentPlayer = setCurrentPlayer;
            currentEnvironment = setCurrentEnvironment;
        }
        // Look at the list of choices in available in a room. This could be used by Styles which may then check the ChoiecRole to see which they decide to display.
        // Either way, all the display-side and user interface side of this should be handled by the Styles or maybe even a userInterface resize thing,
        // rather than by this class - whos sole function is the listing and using of choices as a whole.

        public GameObject ChooseObject(EntityProperty entityPropertyFilter)
        {
            List<Choice> relevantChoices = new List<Choice>();

            uint currentRoomID = currentPlayer.Position;
            Room currentRoomPrinciple = currentEnvironment.GetRoomByID(currentRoomID);
            List<GameObject> allObjects = new List<GameObject>();
            allObjects = currentRoomPrinciple.GetRoomObjects();

            foreach (GameObject possibleObject in allObjects)
            {
                relevantChoices.Add(new Choice($"{possibleObject.Name}", possibleObject, null, null));
            }

            Choice chosenChoice = EventChoiceSubMenu(relevantChoices);
            return chosenChoice.Source;
        }

        public List<Choice> GetChoices()
        {
            List<Choice> allChoices = new List<Choice>();

            uint currentRoomID = currentPlayer.Position;
            Room currentRoomPrinciple = currentEnvironment.GetRoomByID(currentRoomID);
            List<GameObject> validObjects = new List<GameObject>();

            validObjects = currentRoomPrinciple.GetRoomObjects();

            foreach (GameObject currentObject in validObjects)
            {
                List<Choice> currentChoices = new List<Choice>(currentObject.GenerateChoices());

                foreach (Choice additionalChoice in currentChoices)
                {
                    allChoices.Add(additionalChoice);
                }
            }

            return allChoices;
        }

        public List<Choice> GetChoices(string filterByEntityPropertyName, object filterByEntityPropertyValue)
        {
            List<Choice> allChoices = new List<Choice>();

            uint currentRoomID = currentPlayer.Position;
            Room currentRoomPrinciple = currentEnvironment.GetRoomByID(currentRoomID);
            List<GameObject> validObjects = new List<GameObject>();

            validObjects = currentRoomPrinciple.GetRoomObjects(filterByEntityPropertyName, filterByEntityPropertyValue);

            foreach (GameObject currentObject in validObjects)
            {
                List<Choice> currentChoices = new List<Choice>(currentObject.GenerateChoices());

                foreach (Choice additionalChoice in currentChoices)
                {
                    allChoices.Add(additionalChoice);
                }
            }

            return allChoices;
        }

        public List<Choice> GetChoices(List<EntityProperty> entityPropertyFilter)
        {
            List<Choice> allChoices = new List<Choice>();

            uint currentRoomID = currentPlayer.Position;
            Room currentRoomPrinciple = currentEnvironment.GetRoomByID(currentRoomID);
            List<GameObject> validObjects = new List<GameObject>();

            validObjects = currentRoomPrinciple.GetRoomObjects(entityPropertyFilter);

            foreach (GameObject currentObject in validObjects)
            {
                List<Choice> currentChoices = new List<Choice>(currentObject.GenerateChoices());

                foreach (Choice additionalChoice in currentChoices)
                {
                    allChoices.Add(additionalChoice);
                }
            }

            return allChoices;
        }

        //This is the main execute for InteractDelegate
        public string ActivateInteract(Choice currentChoice)
        {
            if (currentChoice.InteractArguments != null)
                return HandleInteractParameters(currentChoice);
            else
                return currentChoice.InteractDelegate();
        }

        //If the InteractDelegate has requirements, handle the requirements here.
        private protected string HandleInteractParameters(Choice currentChoice)
        {
            if (currentChoice.InteractArguments != null)
            {
                if (TestForHealthPropertyOnly(currentChoice))
                    return HandleHealthPropertyOnly(currentChoice);
                /*if (TestForRoomArgumentAndHealthProperty(currentChoice))
                    return HandleRoomArgumentAndHealthProperty(currentChoice);
                */
                throw new Exception("Tried to HandleInteractParameters with an InteractArguments which didn't match any possible test.");
            }
            else throw new Exception("Tried to HandleInteractParameters with null InteractArguments?");
        }

        private protected bool TestForHealthPropertyOnly(Choice currentChoice)
        {
            EntityProperty testForEntityPropertyHasHealth = new EntityProperty("HasHealth", true);

            if (currentChoice.InteractArguments.Length == 1)
                if (currentChoice.InteractArguments.Contains(testForEntityPropertyHasHealth))
                    return true;

            return false;
        }

        private protected string HandleHealthPropertyOnly(Choice currentChoice)
        {
            List<GameObject> itemsWithHealth;
            EntityProperty entityPropertyFilter = new EntityProperty("HasHealth", true);

            GameObject targetObject = ChooseObject(entityPropertyFilter);

            return SendDamageToObject(targetObject);
        }

        private protected string SendDamageToObject(GameObject target)
        {
            target.TakeDamage(1);

            return "You damage the thing using the thing...";
        }
        private protected Choice EventChoiceSubMenu(List<Choice> possibleChoices)
        {
            // Do the event thing
            Console.WriteLine("Temporaily pretending to be an event...");
            Console.WriteLine("Here are your options:");

            uint foreachindex = 0;
            foreach(Choice choice in possibleChoices)
            {
                Console.WriteLine($"{++foreachindex}. {choice.Name}");
            }
            Console.WriteLine("\n Temporarily, continuing after the pause will choose 2.");

            UserInterfaceUtilities.Pause();
            Choice temporarySelection = possibleChoices[1]; //temporarily chooses index 1 by default (option 2)
            return temporarySelection;
        }
    }
}
