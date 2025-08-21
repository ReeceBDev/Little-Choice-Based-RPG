using LCBRPG_User_Console.ConsoleUtilities;
using LCBRPG_User_Console.MenuResource;
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
    internal class NearbyChoicesElement : ElementLogic
    {
        protected const int choiceIndexOffset = 1;

        protected List<InteractionDisplayData> playerInteractions;
        protected InteractionCache interactionAuthority;

        public NearbyChoicesElement(ElementIdentities setUniqueIdentity, List<InteractionDisplayData> initialInteractions, InteractionCache setInteractionAuthority) 
            : base(setUniqueIdentity)
        {
            playerInteractions = initialInteractions;
            interactionAuthority = setInteractionAuthority;

            //Subscribe to the interaction authority's updates so that this element knows to update, too
            interactionAuthority.ResourceUpdated += OnResourceUpdated;
        }


        public void UpdateInteractions(List<InteractionDisplayData> newInteractions) => playerInteractions = newInteractions;

        protected override string GenerateContent()
        {
            return FormatChoices(playerInteractions);
        }

        /// <summary> Returns a formatted string describing every available interaction, in a numbered list. </summary>
        protected virtual string FormatChoices(List<InteractionDisplayData> interactions)
        {
            string createdChoiceList = "";
            int choiceIndex = 0;

            //Prefix
            createdChoiceList += "\n ╔════════════════════════════════════════════════════════════════════════════════════════-=════-=═=-=--=-=-- - - -";

            //Main choice list
            while (choiceIndex < interactions.Count)
            {
                createdChoiceList += $"\n ╠ {choiceIndex + choiceIndexOffset} » {interactions.ElementAt(choiceIndex).InteractionTitle}";
                choiceIndex++;
            }

            //Suffix
            createdChoiceList += "\n ╠═════════════════════════════════════════════════════════════════════════════════" + "══════-=════-=═=-=--=-=-- - - -";

            return createdChoiceList;
        }

        /// <summary> When the authority updates, replicate changes to this element. </summary>
        protected virtual void OnResourceUpdated(object? sender, EventArgs e)
        {
            OnElementUpdating();

            UpdateInteractions(interactionAuthority.GetCache().ToList());
            RefreshContent();

            OnElementUpdated();
        }
    }
}