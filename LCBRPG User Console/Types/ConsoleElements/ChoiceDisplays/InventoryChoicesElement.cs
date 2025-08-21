using LCBRPG_User_Console.MenuResource;
using LCBRPG_User_Console.Types.ConsoleElements;
using LCBRPG_User_Console.Types.DisplayData;
using Little_Choice_Based_RPG.External.EndpointServices;
using Little_Choice_Based_RPG.External.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCBRPG_User_Console.Types.ConsoleElements.ChoiceDisplays
{
    internal class InventoryChoicesElement : NearbyChoicesElement
    {
        public InventoryChoicesElement(ElementIdentities setUniqueIdentity, List<InteractionDisplayData> initialInteractions, InteractionCache setInteractionCache) 
            : base(setUniqueIdentity, initialInteractions, setInteractionCache)
        {
        }

        protected override string FormatChoices(List<InteractionDisplayData> targetInteractions)
        {
            const int maximumInventoryLength = 30;

            List<InteractionDisplayData> orderedInteractions = new(); //Written listings, in order
            List<InteractionDisplayData> inventoryListings = new();
            List<InteractionDisplayData> interactionListings = new();
            List<string> paddedLines = new();

            string createdChoiceList = string.Empty;

            int choiceIndex = 0;
            int inventoryIndex = 0;
            int interactionIndex = 0;


            //Split inventory listings.
            foreach (var displayData in targetInteractions)
            {
                if (displayData.PresentationContext.Equals(SystemInteractionContext.InventoryEntry.ToString()))
                    inventoryListings.Add(displayData);
            }

            //Split other interactions.
            foreach (var interaction in targetInteractions)
            {
                if (!interaction.PresentationContext.Equals(SystemInteractionContext.InventoryEntry.ToString()))
                    interactionListings.Add(interaction);
            }

            //Pad the remaining lines.
            for (int totalLength = 0; totalLength < maximumInventoryLength; totalLength++)
                paddedLines.Add("\n ╠               -=-    ---- Empty ----    -=-       -=-    ---- Empty ----    -=-");


            //Write Prefix
            createdChoiceList += "\n ╔════════════════════════════════════════════════════════════════════════════════════════-=════-=═=-=--=-=-- - - -";

            //Write inventory listings.
            while (inventoryIndex < inventoryListings.Count)
            {
                createdChoiceList += $"\n ╠ {choiceIndex + choiceIndexOffset} » {inventoryListings.ElementAt(inventoryIndex).InteractionTitle}";
                orderedInteractions.Add(inventoryListings.ElementAt(inventoryIndex));
                inventoryIndex++;

                choiceIndex++;
            }

            //Write padded lines.
            foreach (var line in paddedLines)
                createdChoiceList += line;


            //Write other interaction listings.
            while (interactionIndex < interactionListings.Count)
            {
                createdChoiceList += $"\n ╠ {choiceIndex + choiceIndexOffset} » {interactionListings.ElementAt(interactionIndex).InteractionTitle}";
                orderedInteractions.Add(interactionListings.ElementAt(interactionIndex));
                interactionIndex++;

                choiceIndex++;
            }

            //Write Suffix
            createdChoiceList += "\n ╠═════════════════════════════════════════════════════════════════════════════════" + "══════-=════-=═=-=--=-=-- - - -";

            return createdChoiceList;
        }
    }
}
