using Little_Choice_Based_RPG.Types.PropertySystem.Archive;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using Little_Choice_Based_RPG.Types.PropertySystem.Systems;
using Little_Choice_Based_RPG.Types.TypedEventArgs.PropertyContainerEventArgs;

namespace Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PrivateInteractionsSystems
{
    /// <summary> Implements the logic for interactions only meant to be seen by the player. This isn't currently enforced in any way.</summary>
    internal sealed class PublicInteractionsSystem : PropertySystem
    {
        public override void InitialiseNewSubscriber(IPropertyContainer sourceContainer, PropertyStore sourceProperties)
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
