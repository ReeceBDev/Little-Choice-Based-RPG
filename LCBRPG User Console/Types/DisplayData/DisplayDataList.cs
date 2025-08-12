using LCBRPG_User_Console.ConsoleMenus;
using LCBRPG_User_Console.Types.ActualElements;

namespace LCBRPG_User_Console.Types.DisplayDataEntries
{
    /// <summary> Provides a list of Menu Elements with custom functionality. </summary>
    internal class DisplayDataList()
    {
        public DisplayDataList(DisplayDataList original) : this()
        {
            elements = original.elements.ToList();
        }

        public void UpsertElement(uint priority, IElement element, IConsoleMenu sourceMenu)
        {
            //If the element already exists, remove it first
            foreach (var target in elements.ToList())
            {
                if (target.Element.ElementIdentity == element.ElementIdentity)
                {
                    elements.Remove(target);
                    break;
                }
            }

            //Add the new element
            elements.Add(new ElementDisplayData(priority, element, 0, sourceMenu));
        }

        public void ChangePriority(uint newPriority, ElementIdentities identity, IConsoleMenu sourceMenu)
        {
            bool identityFound = false;
            
            //If the identity already exists, replace it
            foreach (var target in elements.ToList())
            {
                if (target.Element.ElementIdentity == identity)
                {
                    elements.Remove(target);
                    elements.Add(new ElementDisplayData(newPriority, target.Element, 0, sourceMenu));

                    identityFound = true;
                    break;
                }
            }

            if (!identityFound)
                throw new Exception("Could not change the priority. The target identity {identity} was not found!");
        }

        public List<ElementDisplayData> elements { get; private set; } = new();
    }
}