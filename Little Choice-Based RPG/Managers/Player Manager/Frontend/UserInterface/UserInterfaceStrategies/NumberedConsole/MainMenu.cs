using Little_Choice_Based_RPG.Types.Interactions.InteractDelegate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface.UserInterfaceStyles
{
    public class MainMenu : IUserInterface
    {
        public void RunMenu()
        {
            Console.WriteLine("Main Menu.");
            UserInterfaceUtilities.Pause();

            /*
            while (!exitMainMenu)
            {
                if (firstTimeSinceTransition)
                    InitialiseMainTextEntries(listedInteractions);

                DrawUserInterface();

                int userInput = AwaitUserInput(listedInteractions.Count);
                InvokeInteraction(userInput, listedInteractions);
            }
            */
        }
    }
}
