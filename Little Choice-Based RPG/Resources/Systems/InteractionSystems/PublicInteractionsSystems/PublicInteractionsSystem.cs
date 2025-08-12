using Little_Choice_Based_RPG.Resources.Entities;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.TypedEventArgs.PropertyContainerEventArgs;

namespace Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PublicInteractionsSystems
{
    /// <summary> Implements the logic for interactions only meant to be seen by the player. This isn't currently enforced in any way.</summary>
    internal class PublicInteractionsSystem : PropertyLogic
    {
        protected override void InitialiseNewSubscriber(PropertyContainer sourceContainer, PropertyHandler sourceProperties)
        {
            //Give the subscriber a PrivateInteractions list.
            if (!sourceContainer.Extensions.Contains("PuyblicInteractions")) //If a PrivateInteractions isnt already assigned
                sourceContainer.Extensions.AddExtension(new PublicInteractions());
        }

        protected override void OnObjectChanged(object sender, ObjectChangedEventArgs objectChangedData)
        {
            //Not needed in this class yet! :) 
        }
    }
}
