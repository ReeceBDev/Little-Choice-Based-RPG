using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface.UserInterfaceStyles
{
    public class InteractionMenu : IUserInterface
    {
        public string OutputMainBody()
        {
            string userInterfaceStyle = string.Join("\n",
                          $" =You have opened your inventory INVENTORY SCREEN :: CURRENT WEIGHT 0/0 MAN -- .",
                          @"{1. next screen}",
                          $" =2. previous screen===-===- =--=-=--_-----_--= =- -_ ._",
                          $" ==3. use item=========",
                          $" ==4. inspect item=========",
                          $" ==5. drop item=========",
                          $" ==6. exit to main menu=========",
                          $">>> ");

            return userInterfaceStyle;
        }
    }
}
