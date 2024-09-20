using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Choices
{
    internal static class ChoiceHandler
    {
        private static List<Choice> choices = new List<Choice>(); //These will be listed as options
        private static List<Choice> primedChoices = new List<Choice>(); //These will be run next
        public static void Add(Choice newChoice) //Adds choices which will be listed as options
        {
            choices.Add(newChoice);
        }
        public static void AddAsPrimed(Choice newChoice) //Adds choices which will be run ASAP
        {
            primedChoices.Add(newChoice);
        }
        public static void Prime(int choiceIndex) //Readies a listed choice by adding it to the primedChoices
        {
            if (choiceIndex > choices.Count | choiceIndex < 0)
                throw new ArgumentException("The choice index chosen can't be found within choices.");

            primedChoices.Add(choices[choiceIndex]);
        }
        public static void InvokePrimed() //Runs all selected choices at once
        {
            foreach (var choice in primedChoices)
            {
                choice.OnExecute?.Invoke();
            }

            ClearAll();
        }
        // Todo: (challenges from knortic :<)
        /* Execute several choices at once
         * Add remove function
        */
        public static void ClearAll()
        {
            choices.Clear();
        }

        public static List<Choice> Choices => choices;
    }
}
