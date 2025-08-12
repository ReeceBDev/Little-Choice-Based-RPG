using LCBRPG_User_Console.ConsoleUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCBRPG_User_Console.Types.ActualElements
{
    internal class InspecteeDataElement : ElementLogic
    {
        protected override string GenerateContent()
        {
            List<string> inspectData = new();
            List<string> formattedData = new();
            string concatenatedInspectData = string.Empty;
            string concatenatedFormattedData = string.Empty;

            //Add each data element to the inspectData
            inspectData.Add((string)inspectee.Properties.GetPropertyValue("Name"));
            inspectData.Add((string)inspectee.Properties.GetPropertyValue("Type"));
            inspectData.Add(DescriptorProcessor.GetDescriptor(inspectee, "Descriptor.Inspect.Current"));
            inspectData.Add("Stats:");

            //Format every property except descriptors:
            foreach (EntityProperty property in inspectee.Properties.EntityProperties.Where(i => !i.PropertyName.ToString().StartsWith("Descriptor.")))
                inspectData.Add($" - {property.PropertyName}   --   {property.PropertyValue.ToString()}");

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
