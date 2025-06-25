using Little_Choice_Based_RPG.Types.EntityProperties;

namespace Little_Choice_Based_RPG.Types.TypedEventArgs
{

    /// <summary> Event arguments for the user to choose a selection, with an optional filter.
    /// Also contains a description for the user to see what the selection is about. </summary>
    public class FilterableRequestEventArgs : EventArgs
    {
        public FilterableRequestEventArgs(string setDescription, List<EntityProperty>? filtersList = null)
        {
            RequestDescription = setDescription;

            if (filtersList != null)
                filters = filtersList;
        }

        public List<EntityProperty>? filters { get; set; } = null;
        public string RequestDescription { get; set; }
    }
}
