﻿using Little_Choice_Based_RPG.Resources.Entities;
using Little_Choice_Based_RPG.Resources.PropertyContainerEventArgs;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PrivateInteractionsSystems.PrivateInteractionsExtensions;
using Little_Choice_Based_RPG.Types.EntityProperties;

namespace Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PrivateInteractionsSystems
{
    /// <summary> Implements the logic for interactions only meant to be seen by the player. This isn't currently enforced in any way.</summary>
    public class PublicInteractionsSystem : PropertyLogic
    {
        protected override void InitialiseNewSubscriber(PropertyContainer sourceContainer, PropertyHandler sourceProperties)
        {
            //Give the subscriber a PrivateInteractions list.
            if (!sourceContainer.Extensions.Contains("PrivateInteractions")) //If a PrivateInteractions isnt already assigned
                sourceContainer.Extensions.AddExtension(new PrivateInteractions());
        }

        protected override void OnObjectChanged(object sender, ObjectChangedEventArgs objectChangedData)
        {
            //Not needed in this class yet! :) 
        }
    }
}
