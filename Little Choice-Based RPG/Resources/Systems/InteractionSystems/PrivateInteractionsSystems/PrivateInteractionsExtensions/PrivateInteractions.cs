using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using Little_Choice_Based_RPG.Types.PropertyExtensions;
using Little_Choice_Based_RPG.Types.PropertyExtensions.PropertyExtensionEventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PrivateInteractionsSystems.PrivateInteractionsExtensions
{
    public class PrivateInteractions : IPropertyExtension
    {
        public List<IInvokableInteraction> PrivateInteractionsList { get; set; }

        public event EventHandler<PropertyExtensionChangedArgs> PropertyExtensionChanged; //No need to invoke this yet, it has no practical purpose (yet).
        public string UniqueIdentifier { get; init; } = "PrivateInteractions";

    }
}
