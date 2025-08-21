using LCBRPG_User_Console.ConsoleUtilities;
using LCBRPG_User_Console.Types.DisplayData;
using Little_Choice_Based_RPG.External.EndpointServices;
using Little_Choice_Based_RPG.External.Types.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCBRPG_User_Console.Types.ConsoleElements
{
    internal class InspecteeDataElement : ElementLogic
    {
        private uint inspecteeID;

        private List<ItemDetails> inspecteeDetails;
        private PlayerInventoryService inventoryService;

        public InspecteeDataElement(ElementIdentities setUniqueIdentity, uint setInspecteeID, PlayerInventoryService setInventoryService) : base (setUniqueIdentity)
        {
            inspecteeID = setInspecteeID;
            inventoryService = setInventoryService;
        }

        protected override string GenerateContent()
        {
            inspecteeDetails ??= inventoryService.GetItemDetails(inspecteeID);

            List<string> inspectData = new();
            List<string> formattedData = new();
            string concatenatedInspectData = string.Empty;
            string concatenatedFormattedData = string.Empty;

            //Add each data element to the inspectData
            inspectData.Add(inspecteeDetails.Find(i => i.Property.Equals("Name")).Data);
            inspectData.Add(inspecteeDetails.Find(i => i.Property.Equals("Type")).Data);
            inspectData.Add(inspecteeDetails.Find(i => i.Property.Equals("Name")).Data);
            inspectData.Add(inventoryService.GetInspectDescriptor(inspecteeID));
            inspectData.Add("Stats:");

            //Format every property except descriptors:
            foreach (var detail in inspecteeDetails.Where(i => !i.Property.StartsWith("Descriptor.")))
                inspectData.Add($" - {detail.Property}   --   {detail.Data}");

            //Format the data elements
            foreach (var entry in inspectData)
                concatenatedInspectData += $"\n {entry}";

            formattedData = WritelineUtilities.SplitIntoLines(concatenatedInspectData, "\n ╟ ", "\n ║ ", "\n ╟", 100);

            //Concatenate the lines into one string
            foreach (var entry in formattedData)
                concatenatedFormattedData += entry;

            return concatenatedFormattedData;
        }
    }
}
