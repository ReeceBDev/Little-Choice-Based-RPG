using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PrivateInteractionsSystems;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PublicInteractionsSystems;
using Little_Choice_Based_RPG.Types.PropertySystem.Components;
using Little_Choice_Based_RPG.Types.PropertySystem.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Components.SystemComponents
{
    internal class PublicInteractionsComponent : IPropertyComponent, ISystemComponent
    {
        /// <summary> Initialises PublicInteractions on this entity. Exposes interactibility about this entity, for outside use. Implicitly shared nearby. </summary>
        public TransparentSystemReference? UsesSystem { get; set => PropertyHandler.ReplaceSystemReference(ref field, value, new what do i put here lol()); }

        /// <summary> Stores interactions which expose interactions about this entity to others nearby, i.e. giving a door the ability to be opened by anyone. </summary>
        public TransparentProperty<PublicInteractions>? PublicInteractionsList { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }
    }
}