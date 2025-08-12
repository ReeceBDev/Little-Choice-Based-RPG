using Little_Choice_Based_RPG.External.Types;
using Little_Choice_Based_RPG.Managers.PlayerControl;
using Little_Choice_Based_RPG.Resources.Rooms;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Little_Choice_Based_RPG.Managers.Server
{
    /// <summary> Maintains a list of live sessions. Authetnication tokens are mapped to PlayerControllers. Session keys are simple ulongs and compatible with external services. 
    /// This allows for the mapping of a basic data type to in-game player data. </summary>
    internal static class SessionManager
    {
        //Maybe player sessions also have their playerlog held here on the game-side. That way, if a player disconnects and reconnects in short span, their session is not lost.
        //Sessions should obviously expire, but I'd like for them to stick around a little bit.
        //That way if a player crashes, accidentally closes, or anything else, they can just resume exactly where they left off.
        //This could also maybe hold their current screen, and any other transient data, which could get re-constructed.
        //I like it, not a bad idea.
        //Alternatively, that's what cookies are for! But they dont work on the console thing, like I found out!

        private static Dictionary<ulong, PlayerController> activeSessions = new(); //Authentication Key, PlayerController
        private static Object lockMonitor = new();

        /// <summary> Generates an entirely new player session, mapped to a new player controller. Returns the session token.
        /// Persistence is not implemented yet, therefore there are no references to existing player data, nor disposals, yet. </summary>
        public static ulong NewSession(PlayerController playerData)
        {
            lock (lockMonitor)
            {
                ulong? sessionToken = null;
                bool sessionAdded = false;

                while (!sessionAdded)
                {
                    sessionToken = GenerateSessionToken(); //Regenerate until there are no collisions
                    sessionAdded = activeSessions.TryAdd((ulong)sessionToken, playerData);
                }


                if (sessionToken is null)
                    throw new Exception("The session token was null, and therefore somehow not set, despite the above while loop!");

                return (ulong)sessionToken;
            }
        }

        public static PlayerController GetPlayerController(ulong authenticationToken)
        {
            lock (lockMonitor)
            {
                activeSessions.TryGetValue(authenticationToken, out PlayerController relatedController);
                //I need to add error handling

                return relatedController;
            }
        }

        private static ulong GenerateSessionToken()
        {
            ulong sessionToken;
            Span<byte> tokenBytes = stackalloc byte[8]; //Allocate bytes in a span for the generator to use. 8 bytes to make up 64 bits, for the ulong.

            RandomNumberGenerator.Fill(tokenBytes); //Randomises all the bytes (only works on bytes, unfortunately) //RandomNumberGenerator is from cryptography. 
            sessionToken = BitConverter.ToUInt64(tokenBytes); //Convert from span of bytes to ulong

            return (sessionToken);
        }
    }
}
