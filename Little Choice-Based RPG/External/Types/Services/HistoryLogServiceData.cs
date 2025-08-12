namespace Little_Choice_Based_RPG.External.Types.Services
{

    /// <summary> Lightweight data for clients, representing a history log and its timestamp. This is delivered via services. </summary>
    public readonly record struct HistoryLogServiceData(string logContent, ulong timeStamp);
}
