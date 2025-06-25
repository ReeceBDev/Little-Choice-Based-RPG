namespace Little_Choice_Based_RPG.Types.TypedEventArgs
{
    /// <summary> Event arguments for requesting a decision from the user out of an array of strings, lablled with a description string.
    /// Also contains a cancellation delegate to cancel the original request. </summary>
    public class UserDecisionEventArgs : EventArgs
    {
        public UserDecisionEventArgs(string setDescription, string[] setPossibilities)
        {
            RequestDescription = setDescription;
            Possiblities = setPossibilities;
        }

        public string RequestDescription { get; set; }
        public string[] Possiblities { get; set; }
    }
}
