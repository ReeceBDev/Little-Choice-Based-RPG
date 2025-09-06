using Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Break;
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
    internal class PrivateInteractionsComponent : IPropertyComponent, ISystemComponent
    {
        /// <summary> Initialises PrivateInteractions on this entity. Stores interactions which may only be used by this entity, even about other objects. </summary>
        public TransparentSystemReference? UsesSystem { get; set => PropertyHandler.ReplaceSystemReference(ref field, value, new what do I put here lol()); }

        /// <summary> Stores interactions which may only be used by this entity, even about other objects, i.e. giving a player access to personal door keys.</summary>
        public TransparentProperty<PrivateInteractions>? PrivateInteractionsList { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }
    }
}