using LCBRPG_User_Console.Types.DisplayData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCBRPG_User_Console.Types.ConsoleElements
{
    internal interface IElement
    {
        public ElementIdentities ElementIdentity { get; init; }
        public abstract string GetCachedContent();

        public abstract void RefreshContent();

        public event EventHandler ElementUpdated;
        public event EventHandler ElementUpdating;
    }
}
