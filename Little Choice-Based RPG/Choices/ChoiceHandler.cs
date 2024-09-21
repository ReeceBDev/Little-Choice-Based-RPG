using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Choices
{
    internal static class ChoiceHandler
    {
        private static List<Choice> listedChoices = new List<Choice>(); //These will be listed as options
        private static List<Choice> primedChoices = new List<Choice>(); //These will be run next
        public static void Add(Choice newChoice) //Adds choices which will be listed as options
        {
            listedChoices.Add(newChoice);
        }
        public static void AddPrimed(Choice newChoice) //Adds choices which will be run ASAP
        {
            primedChoices.Add(newChoice);
        }
        public static void Prime(int choiceIndex) //Readies a listed choice by adding it to the primedChoices
        {
            if (choiceIndex > listedChoices.Count | choiceIndex < 0)
                throw new ArgumentException("The choice index chosen can't be found within choices.");

            primedChoices.Add(listedChoices[choiceIndex]);
        }
        public static void InvokePrimed() //Runs all selected choices at once
        {
            foreach (var choice in primedChoices)
            {
                choice.OnExecute?.Invoke();
            }

            ClearPrimed();
        }
        public static void RemoveListed(int index) => listedChoices.RemoveAt(index);
        public static void RemoveListed(Choice removedChoice) => listedChoices.Remove(removedChoice);
        public static void RemovePrimed(int index) => primedChoices.RemoveAt(index);
        public static void RemovePrimed(Choice removedChoice) => primedChoices.Remove(removedChoice);
        public static void ClearListed() => listedChoices.Clear();
        public static void ClearPrimed() => primedChoices.Clear();
        public static void ClearAll()
        {
            ClearListed();
            ClearPrimed();
        }

        public static List<Choice> Choices => listedChoices;
    }
}
