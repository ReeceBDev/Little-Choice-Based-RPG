﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface.UserInterfaceStyles
{
    public class MainMenuStyle : IUserInterface
    {
        public string OutputMainBody()
        {
            string userInterfaceStyle = string.Join("\n",
                          $" ===========-===== WELCOME TO THE GAME ===== ----========-========-= --..-- .",
                          @"{1. Start G\name\n\n\\\\\\\\\\\n\n\n\n }",
                          $" ====-===- =--=-=--_-----_--= =- -_ ._",
                          $" ===========",
                          $">>> ");

            return userInterfaceStyle;
        }
    }
}
