using Little_Choice_Based_RPG.Types.Interactions.InteractDelegate;
using Little_Choice_Based_RPG.Types.PropertyExtensions.ExtensionEventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.PropertyExtensions.Extensions
{
    public class PrivateInteractions : IExtension
    {
        public List<IInvokableInteraction> PrivateInteractionsList { get; set; }

        public event EventHandler<ExtensionChangedArgs> ExtensionChanged; //No need to invoke this yet, it has no practical purpose (yet).
        public string UniqueIdentifier { get; init; } = "PrivateInteractions";

    }
}
