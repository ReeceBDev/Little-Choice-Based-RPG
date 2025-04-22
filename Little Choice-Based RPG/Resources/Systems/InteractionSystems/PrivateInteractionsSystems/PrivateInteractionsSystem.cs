using Little_Choice_Based_RPG.Resources.PropertyContainerEventArgs;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PrivateInteractionsSystems.PrivateInteractionsExtensions;
using Little_Choice_Based_RPG.Types.EntityProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PrivateInteractionsSystems
{
    /// <summary> Implements player-specific logic.</summary>
    public class PrivateInteractionsSystem : PropertyLogic
    {
        protected override void InitialiseNewSubscriber(PropertyContainer sourceContainer, PropertyHandler sourceProperties)
        {
            //Give the subscriber a PrivateInteractions list.
            if (!sourceContainer.Extensions.ContainsExtension("PrivateInteractions")) //If a PrivateInteractions isnt already assigned
                sourceContainer.Extensions.AddExtension(new PrivateInteractions());
        }

        protected override void OnObjectChanged(object sender, ObjectChangedEventArgs objectChangedData)
        {
        }
    }
}
