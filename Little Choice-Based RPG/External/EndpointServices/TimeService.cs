using Little_Choice_Based_RPG.Managers.Server;

namespace Little_Choice_Based_RPG.External.EndpointServices
{
    /// <summary> Exposes all time-related functions, both server-side and in the game world. </summary>
    public class TimeService
    {
        public static string GetServerTime() => SystemTimeController.GetSystemTime();
    }
}
