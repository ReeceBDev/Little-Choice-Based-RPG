using Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Break;
using Little_Choice_Based_RPG.Resources.Systems.RoomSystems;
using Little_Choice_Based_RPG.Resources.Systems.RoomSystems.DirectionExtensions;
using Little_Choice_Based_RPG.Types.PropertySystem.Components;
using Little_Choice_Based_RPG.Types.PropertySystem.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Components.SystemComponents
{
    internal class DirectionsComponent : IPropertyComponent, ISystemComponent
    {
        /// <summary> Properties used by the Travel interaction, which provides the ability for entities to move between interconnected inventories.</summary>
        public TravelClass Travel { get; } = new();


        /// <summary> Initialises DirectionSystem on this entity. Allows use of navigable directions and point to point navigation portals. </summary>
        public TransparentSystemReference? UsesSystem { get; set => PropertyHandler.ReplaceSystemReference(ref field, value, new DirectionSystem()); }

        /// <summary> Provides the entity a list of point to point navigation portals to nearby entities. Usually used by rooms. </summary>
        public TransparentProperty<RoomConnections>? RoomConnectionsList { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }


        public class TravelClass
        {
            /// <summary> The title of the Travel interaction as displayed to the player. </summary>
            public TransparentProperty<string>? Title { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }

            /// <summary> Describes how this entity sees itself when travelling is invoked. </summary>
            public TransparentProperty<string>? Description { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }
        }
    }
}
