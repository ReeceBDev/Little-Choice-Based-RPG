using LCBRPG_User_Console.ConsoleMenus;
using LCBRPG_User_Console.Types.ActualElements;

namespace LCBRPG_User_Console.Types.DisplayDataEntries
{
    internal struct ElementDisplayData
    {
        public ElementDisplayData(uint setPriority, IElement setElement, uint setWriteSpeed, IConsoleMenu currentMenu)
        {
            Element = setElement;
            WriteSpeed = setWriteSpeed;
            Priority = setPriority;
        }

        public IElement Element { get; init; }
        public uint WriteSpeed { get; init; }
        public uint Priority { get; init; }
    }
}
