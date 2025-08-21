using Little_Choice_Based_RPG.External.Types.Services;
using Little_Choice_Based_RPG.External.Types.TypedEventArgs.HistoryLog;
using Little_Choice_Based_RPG.Managers.PlayerControl;
using Little_Choice_Based_RPG.Managers.Server;

namespace Little_Choice_Based_RPG.External.EndpointServices
{
    /// <summary> Provides endpoints with a history log from the server. The entire log may be requested at once by the endpoint. </summary>
    public sealed class HistoryLogService
    {
        private PlayerController currentPlayerController;

        public HistoryLogService(ulong authenticationToken)
        {
            currentPlayerController = SessionManager.GetPlayerController(authenticationToken);
        }

        public HistoryLogServiceData[] RequestLogHistory(int? logCount = null) 
            => currentPlayerController.CurrentHistoryLog.GetHistoryLog(logCount);

        public event EventHandler<HistoryLogServiceAdditionEventArgs> LogAdded; //Pushes a new history log to an endpoint.
        public event EventHandler LogCacheRefreshRequired; //Instructs endpoints to re-request their cache.
    }
}
