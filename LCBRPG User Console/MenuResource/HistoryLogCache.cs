using LCBRPG_User_Console.Types;
using Little_Choice_Based_RPG.External.Types.Services;
using Little_Choice_Based_RPG.External.Types.TypedEventArgs.HistoryLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCBRPG_User_Console.MenuResource
{
    internal class HistoryLogCache : IMenuResource
    {
        private LocalPlayerSession playerSession;
        private List<HistoryLogDisplayData> historyLogCache;

        public event EventHandler ResourceUpdated;
        public event EventHandler<HistoryLogDisplayData> LogAdded;

        public HistoryLogCache(LocalPlayerSession setPlayerSession)
        {
            playerSession = setPlayerSession;

            //Subscribe to historyLog service updates
            playerSession.HistoryLogServiceInstance.LogCacheRefreshRequired += OnHistoryLogRefreshRequired;
            playerSession.HistoryLogServiceInstance.LogAdded += OnHistoryLogAvailable;

            //Initialise
            RefreshCache();
        }

        protected virtual void OnHistoryLogRefreshRequired(object? sender, EventArgs e)
        {
            RefreshCache();
        }

        protected virtual void OnHistoryLogAvailable(object? sender, HistoryLogServiceAdditionEventArgs e)
        {
            HistoryLogServiceData historyLog = e.NewLogEntry;

            //Transmute the historyLog from the other assemblies data type into the local data type for displaying that data
            HistoryLogDisplayData displayData = new HistoryLogDisplayData(historyLog.logContent, historyLog.timeStamp);

            AddHistoryLog(displayData);
        }

        protected virtual void InvokeResourceUpdated()
        {
            ResourceUpdated?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void InvokeLogAdded(HistoryLogDisplayData e)
        {
            LogAdded?.Invoke(this, e);
        }

        private void RefreshCache()
        {
            //Fetch historyLog cache. 
            HistoryLogServiceData[] getHistoryLogCacheResults = playerSession.HistoryLogServiceInstance.RequestLogHistory();

            historyLogCache = Array
                .ConvertAll(getHistoryLogCacheResults, i => new HistoryLogDisplayData(i.logContent, i.timeStamp))
                .ToList();

            //Notify
            InvokeResourceUpdated();
        }

        private void AddHistoryLog(HistoryLogDisplayData i)
        {
            //Add historyLog by its display data
            historyLogCache.Add(i);

            //Notify
            InvokeLogAdded(i);
            InvokeResourceUpdated();
        }
    }
}