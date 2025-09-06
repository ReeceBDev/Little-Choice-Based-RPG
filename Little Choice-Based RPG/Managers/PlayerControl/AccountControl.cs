using Little_Choice_Based_RPG.Managers.Server;
using Little_Choice_Based_RPG.Managers.World;
using Little_Choice_Based_RPG.Resources.Entities.Rooms;

namespace Little_Choice_Based_RPG.Managers.PlayerControl
{
    /// <summary> Handles account information and authentication, such as logging in to an existing account, or creating a guest account. </summary>
    internal static class AccountControl
    {
        /// <summary> Creates a temporary one-off player instance which is not stored persistently and returns their Session Token. </summary>
        public static ulong NewGuestSession()
        {
            //Generate a new player at the default spawn location
            GameEnvironment currentEnvironment = GameWorld.FindEnvironmentByID(1);
            Room spawnRoom = currentEnvironment.RoomRegistry.GetRoom(0, 0, 0); //Select the room to spawn the player in
            PlayerController player1 = new PlayerController(spawnRoom, currentEnvironment); //Create the player in that room

            //Assign them a session
            ulong sessionToken = SessionManager.NewSession(player1);

            return sessionToken;
        }
    }
}
