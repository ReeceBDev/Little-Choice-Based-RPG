using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface;
using Little_Choice_Based_RPG.Types.EntityProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems.Damage.Break
{
    public interface IBreakable
    {
        void Break(IUserInterface mutexHolder);
    }
}
