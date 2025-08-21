using LCBRPG_User_Console.ConsoleMenus;
using LCBRPG_User_Console.Types.ConsoleElements;

namespace LCBRPG_User_Console.Types.DisplayData
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
