using Little_Choice_Based_RPG.External.ConsoleEndpoint.ConsoleMenus;

namespace Little_Choice_Based_RPG.Types.External.ConsoleElements
{
    public class ConsoleElement
    {
        public ConsoleElement(uint setPriority, ConsoleElementIdentity setIdentifier, string setContent, uint setWriteSpeed, IConsoleMenu currentMenu)
        {
            UniqueIdentity = setIdentifier;
            Content = setContent;
            WriteSpeed = setWriteSpeed;
            Priority = setPriority;
        }

        public ConsoleElementIdentity UniqueIdentity { get; init; } // Can only use one of each, across all instances UserInterfaceTextEntry, except None, which can be used forever.
        public string Content { get; init; }
        public uint WriteSpeed { get; init; }
        public uint Priority { get; init; }
    }
}
