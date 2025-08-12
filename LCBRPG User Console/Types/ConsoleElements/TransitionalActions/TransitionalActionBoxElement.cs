using LCBRPG_User_Console.ConsoleUtilities;
using LCBRPG_User_Console.Types.DisplayDataEntries;
using Little_Choice_Based_RPG.External.EndpointServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LCBRPG_User_Console.Types.ActualElements.TransitionalActions
{
    internal class TransitionalActionBoxElement : ElementLogic
    {
        protected const string defaultTransitionalAction =
            $"A tsunami of a thousand glass-like reflections tear open reality with a roar.\n" +
            $"When they close, you are left standing in their place.";

        private LocalPlayerSession playerSession;


        public TransitionalActionBoxElement(ElementIdentities setUniqueIdentity, LocalPlayerSession currentPlayerSession) : base(setUniqueIdentity)
        {
            playerSession = currentPlayerSession;
        }

        protected override string GenerateContent()
        {
            int longestLineCount = 0;

            const int topPaddingInset = 1;
            const int bottomPaddingInset = 8;
            const int bottomLength = 84;

            string concatenatedTop = "";
            string concatenatedMid = "";
            string concatenatedBottom = "";

            string transitionalTopPrefix = "\n ╟";
            string transitionalTopInfix = "─";
            string transitionalTopSuffix = "╖";

            string transitionalMidPrefix = "\n ╠├ ";
            string transitionalMidInfix = " ";
            string transitionalMidSuffix = "┤╣";

            string transitionalBottomPrefix = "\n ╠══════════════════╤";
            string transitionalBottomInfix = "═";
            string transitionalBottomMark = "╩";
            string transitionalBottomSuffix = "═";

            string topPadding = "";
            string middlePadding = "";
            string bottomPadding = "";
            string bottomEnding = "";
            string bottomEndingSuffix = "══════-=════-=═=-=--=-=-- - - -";


            List<string> transitionalActionLines = WritelineUtilities.SplitIntoLines(transitionalAction, transitionalMidPrefix, transitionalMidPrefix, transitionalMidPrefix, 84);

            foreach (string line in transitionalActionLines)
            {
                if (longestLineCount < line.Length)
                    longestLineCount = line.Length - transitionalMidPrefix.Length;
            }

            //bottomPadding
            for (int i = 0; i <= 9 + (longestLineCount - transitionalBottomPrefix.Length); i++)
                bottomPadding += transitionalBottomInfix;

            //bottomEnding
            for (int i = 0; i <= bottomLength - (transitionalBottomPrefix.Length + bottomPadding.Length + transitionalBottomMark.Length); i++)
                bottomEnding += transitionalBottomSuffix;

            foreach (string transitionalActionContent in transitionalActionLines)
            {
                middlePadding = "";

                //middlePadding
                while (transitionalActionContent.Length + middlePadding.Length < bottomLength - transitionalBottomMark.Length - bottomEnding.Length)
                    middlePadding += transitionalMidInfix;

                //concatenatedMid
                concatenatedMid += transitionalActionContent + middlePadding + transitionalMidSuffix;
            }

            //topPadding
            while (transitionalTopPrefix.Length + topPadding.Length - transitionalTopSuffix.Length < bottomLength - transitionalBottomMark.Length - bottomEnding.Length)
                topPadding += transitionalTopInfix;

            concatenatedTop = transitionalTopPrefix + topPadding + transitionalTopSuffix;
            concatenatedBottom = transitionalBottomPrefix + bottomPadding + transitionalBottomMark + bottomEnding + bottomEndingSuffix;

            return concatenatedTop + concatenatedMid + concatenatedBottom;
        }
    }
}
