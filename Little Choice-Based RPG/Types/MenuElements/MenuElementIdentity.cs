using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.MenuElements
{
    public enum MenuElementIdentity
    {
        None, //Default, used by StyleLine
        TopStatusBar,
        TransitionalAction,
        CurrentDescription,
        HistoryLog,
        AvailableChoices,
        TargetData,

        //Submenus
        RequestSubMenuTitle,
        RequestSubMenuOptions,
        OpenContainerSubMenuTitle,
        OpenContainerSubMenuOptions
    }
}
