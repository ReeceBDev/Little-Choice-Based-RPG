using LCBRPG_User_Console.ConsoleUtilities;
using LCBRPG_User_Console.MenuResource;
using LCBRPG_User_Console.Types.ConsoleElements;
using LCBRPG_User_Console.Types.DisplayData;
using Little_Choice_Based_RPG.External.EndpointServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LCBRPG_User_Console.Types.ConsoleElements.TransitionalActions
{
    internal class TransitionalActionBlockElement : ElementLogic
    {
        protected const string defaultTransitionalAction =
            $"A tsunami of a thousand glass-like reflections tear open reality with a roar.\n" +
            $"When they close, you are left standing in their place.";

        private HistoryLogCache localHistoryLog;


        public TransitionalActionBlockElement(ElementIdentities setUniqueIdentity, HistoryLogCache setHistoryLogCache) : base(setUniqueIdentity)
        {
            localHistoryLog = setHistoryLogCache;
        }

        protected override string GenerateContent()
        {
            string boxedTransitionalAction;
            string transitionalAction = null;

            string designPrefix = "\n ╔════════════════════════════════════════════════════════════════════════════════════════-=════-=═=-=--=-=-- - - -";
            string designBody = string.Empty;
            string designSuffix = "\n ╚═════════════════════════════════════════════════════════════════════════════════" + "══════-=════-=═=-=--=-=-- - - -";

            List<string> formattedTransitionalAction;
            HistoryLogDisplayData? recentHistoryLogData = localHistoryLog.PeekCache();

            transitionalAction = recentHistoryLogData?.Body.ToString() ?? defaultTransitionalAction;

            formattedTransitionalAction = WritelineUtilities.SplitIntoLines(transitionalAction, "\n ╠├ ", "\n ╠├ ", "\n ╠├ ");

            foreach (string line in formattedTransitionalAction)
                designBody += line;


            boxedTransitionalAction = designPrefix + designBody + designSuffix;

            return boxedTransitionalAction;
        }
    }
}
