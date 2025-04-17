using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.TypedEventArgs
{

    /// <summary> Event arguments for the user to choose a selection, with an optional filter.
    /// Also contains a description for the user to see and a cancellation delegate to cancel the original request. </summary>
    public class FilterableRequestEventArgs : EventArgs
    {
        public FilterableRequestEventArgs(string setDescription, IInvokableInteraction setCancelDelegate, List<EntityProperty>? filtersList = null)
        {
            RequestDescription = setDescription;
            CancellationDelegate = setCancelDelegate;

            if (filtersList != null)
                filters = filtersList;
        }

        public List<EntityProperty>? filters { get; set; } = null;
        public string RequestDescription { get; set; }
        public IInvokableInteraction CancellationDelegate { get; set; }
    }
}
