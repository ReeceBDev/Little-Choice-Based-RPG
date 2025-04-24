using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.MenuElements
{
    public class MenuElement
    {
        public MenuElement(uint setPriority, MenuElementIdentity setIdentifier, string setContent, uint setWriteSpeed, IUserInterface currentMenu)
        {
            UniqueIdentity = setIdentifier;
            Content = setContent;
            WriteSpeed = setWriteSpeed;
            Priority = setPriority;
        }

        public MenuElementIdentity UniqueIdentity { get; init; } // Can only use one of each, across all instances UserInterfaceTextEntry, except None, which can be used forever.
        public string Content { get; init; }
        public uint WriteSpeed { get; init; }
        public uint Priority { get; init; }
    }
}
