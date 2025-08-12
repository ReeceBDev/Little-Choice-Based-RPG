using LCBRPG_User_Console.Types.DisplayDataEntries;
using Little_Choice_Based_RPG.External.EndpointServices;
using Little_Choice_Based_RPG.External.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCBRPG_User_Console.Types.ActualElements.ChoiceDisplays
{
    internal class UserChoicesElement : ElementLogic
    {
        private const int choiceIndexOffset = 1;
        private LocalPlayerSession playerSession;

        public UserChoicesElement(ElementIdentities setUniqueIdentity, LocalPlayerSession currentPlayerSession) : base(setUniqueIdentity)
        {
            playerSession = currentPlayerSession;
        }

        protected override string GenerateContent()
        {
            return FormatChoices();
        }

        private string FormatChoices(List<InteractionDisplayData> targetInteractions, out List<InteractionDisplayData> sortedInteractions)
        {
            List<InteractionDisplayData> reorderedInteractions = new();
            string createdChoiceList = "";
            int choiceIndex = 0;

            //Prefix
            createdChoiceList += "\n ╔════════════════════════════════════════════════════════════════════════════════════════-=════-=═=-=--=-=-- - - -";

            //Main choice list
            while (choiceIndex < targetInteractions.Count)
            {
                createdChoiceList += $"\n ╠ {choiceIndex + choiceIndexOffset} » {targetInteractions.ElementAt(choiceIndex).InteractionTitle}";
                reorderedInteractions.Add(targetInteractions.ElementAt(choiceIndex));
                choiceIndex++;
            }

            //Suffix
            createdChoiceList += "\n ╠═════════════════════════════════════════════════════════════════════════════════" + "══════-=════-=═=-=--=-=-- - - -";

            sortedInteractions = reorderedInteractions;
            return createdChoiceList;
        }
    }
}
