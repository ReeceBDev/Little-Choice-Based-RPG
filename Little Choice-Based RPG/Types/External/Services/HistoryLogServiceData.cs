namespace Little_Choice_Based_RPG.Types.External.Services
{

    /// <summary> Lightweight data for clients, representing a history log and its timestamp. This is delivered via services. </summary>
    public readonly record struct HistoryLogServiceData(string logContent, string timeStamp);
}
