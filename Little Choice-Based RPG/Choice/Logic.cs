using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Little_Choice_Based_RPG.Choice;

namespace Little_Choice_Based_RPG.Choice
{
    public static class Logic
    {
        public static bool HandleChoice()
        {
            ListChoices();
            Console.Write("\n\t\t>>\t");
            bool result = ExecuteChoice(ReadChoice());

            if (!result) { return false; }
            PlayerInterface.Pause();
            return true;
        }

        static void ListChoices()
        {
            if (Player.IsHelmetDamaged)
            {
                PlayerInterface.WriteDialogue("A. Smack the side of your helmet."); // lets you hear
            }
            if (Player.IsPlayerKnockedDown)
            {
                if (Player.IsHelmetDamaged)
                { Console.WriteLine(""); }
                PlayerInterface.WriteDialogue("B. Pick yourself up off of the floor."); // lets you see, lets you move
            }
        }
        static string ReadChoice()
        {
            //Sanitises input
            string playerInput = Console.ReadLine();
            if (playerInput == null)
            {
                Console.WriteLine("\nPlease enter your choice.");
                return null;
            }
            else if (!playerInput.All(char.IsLetterOrDigit))
            {
                Console.WriteLine("\nPlease only use letters or numbers to make your choice.");
                return null;
            }
            else
            {
                return playerInput;
            }
        }

        static bool ExecuteChoice(string playerInput)
        {
            if (playerInput == "A")
            {
                Player.IsHelmetDamaged = false;
                Player.PlayerCanHear = true;
                Outcomes.ChoiceA();
                return true;
            }
            else if (playerInput == "B")
            {
                Player.IsPlayerKnockedDown = false;
                Player.PlayerCanMove = true;
                Player.PlayerCanSee = true;
                Outcomes.ChoiceB();
                return true;
            }
            return false;
        }
    }
}