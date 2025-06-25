using Little_Choice_Based_RPG.Managers.PlayerControl;
using Little_Choice_Based_RPG.Types.External.Services;

namespace Little_Choice_Based_RPG.External.EndpointServices
{
    /// <summary> Provides endpoints with a history log from the server. The entire log may be requested at once by the endpoint. </summary>
    internal class HistoryLogService
    {
        public event EventHandler<HistoryLogServiceData> LogAdded; //Pushes a new history log to an endpoint.
        public event EventHandler LogCacheRefreshRequired; //Instructs endpoints to re-request their cache.

        public static List<HistoryLogServiceData> RequestLogHistory(PlayerController currentPlayerController, int logCount) 
            => currentPlayerController.CurrentHistoryLog.GetHistoryLog(logCount);
    }
}
