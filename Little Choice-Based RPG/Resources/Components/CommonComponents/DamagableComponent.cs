using Little_Choice_Based_RPG.Types.PropertySystem.Components;
using Little_Choice_Based_RPG.Types.PropertySystem.Properties;

namespace Little_Choice_Based_RPG.Resources.Components.CommonComponents
{
    internal class DamagableComponent : IPropertyComponent {
        /// <summary> Used to indicate something is broken yet still relatively intact. For example, this might be used to indicate something is repairable. </summary>
        public TransparentProperty<bool>? IsBroken { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }

        /// <summary> Indicates when an entity is entirely destroyed and likely without repair. It may also be used to indicate a vastly degraded appearance.
        /// It could also be used to indicate an entity has fallen down to constituent parts or rubble. </summary>
        public TransparentProperty<bool>? IsDestroyed { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }
    }
}
