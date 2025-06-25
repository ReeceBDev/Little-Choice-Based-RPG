using Little_Choice_Based_RPG.External.ConsoleEndpoint.ConsoleMenus;

namespace Little_Choice_Based_RPG.Types.External.ConsoleElements
{
    /// <summary> Provides a list of Menu Elements with custom functionality. </summary>
    public class ConsoleElementList
    {
        public void UpsertElement(uint priority, ConsoleElementIdentity identity, string content, IConsoleMenu sourceMenu)
        {
            //If the identity already exists, remove it first
            foreach (var target in elements.ToList())
            {
                if (target.UniqueIdentity == identity)
                    elements.Remove(target);
                break;
            }

            //Add the new element
            elements.Add(new ConsoleElement(priority, identity, content, 0, sourceMenu));
        }

        public void ChangePriority(uint newPriority, ConsoleElementIdentity identity, IConsoleMenu sourceMenu)
        {
            bool identityFound = false;
            
            //If the identity already exists, replace it
            foreach (var target in elements.ToList())
            {
                if (target.UniqueIdentity == identity)
                {
                    elements.Remove(target);
                    elements.Add(new ConsoleElement(newPriority, identity, target.Content, 0, sourceMenu));

                    identityFound = true;
                    break;
                }
            }

            if (!identityFound)
                throw new Exception("Could not change the priority. The target identity {identity} was not found!");
        }

        public List<ConsoleElement> elements { get; private set; } = new();
    }
}