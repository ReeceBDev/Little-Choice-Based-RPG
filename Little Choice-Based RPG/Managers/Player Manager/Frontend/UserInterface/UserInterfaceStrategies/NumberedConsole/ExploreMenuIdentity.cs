using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface.UserInterfaceStrategies.NumberedConsole
{
    public enum ExploreMenuIdentity
    {
        None, //Default, used by StyleLine
        TopStatusBar,
        TransitionalAction,
        CurrentDescription,
        HistoryLog,
        AvailableChoices,

        //Submenu
        SubMenuTitle,
        SubMenuOptions,
    }
}
