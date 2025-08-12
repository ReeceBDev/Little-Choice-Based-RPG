using Little_Choice_Based_RPG.Managers.PlayerControl;
using Little_Choice_Based_RPG.Managers.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.External.EndpointServices
{
    public sealed class SessionService
    {
        private ulong sessionTokenCache;

        /// <summary> Initialises a new guest session and returns its SessionToken. </summary>
        public ulong CreateGuestSession()
        {
            ulong sessionToken = AccountControl.NewGuestSession();

            sessionTokenCache = sessionToken;

            return sessionToken;
        }
    }
}
