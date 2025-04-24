using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Managers.World;
using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Types;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.EntityProperties;

namespace Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface
{
    // Outlines the possible interface styles to be used by the UserInterfaceHandler
    public interface IUserInterface
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
