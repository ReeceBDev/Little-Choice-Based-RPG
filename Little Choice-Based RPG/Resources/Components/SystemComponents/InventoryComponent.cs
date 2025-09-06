using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory;
using Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Break;
using Little_Choice_Based_RPG.Types.PropertySystem.Components;
using Little_Choice_Based_RPG.Types.PropertySystem.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Components.SystemComponents
{
    internal class InventoryComponent : IPropertyComponent, ISystemComponent
    {
        /// <summary> Represents the flavour texts of interactions presented to the player under the InventorySystem. </summary>
        public InteractionsClass Interactions { get; } = new();


        /// <summary> Indicates whether an entity uses the InventorySystem. Examples might include: Players, Rooms, Chests, Vehicles. </summary>
        public TransparentSystemReference? UsesSystem { get; set => PropertyHandler.ReplaceSystemReference(ref field, value, new InventorySystem()); }

        /// <summary> Provides a list of entities currently held by an inventory under the InventorySystem. </summary>
        public TransparentProperty<List<GameObject>>? ItemContainer { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }


        public class InteractionsClass
        {
            /// <summary> The pickup title text for the Pickup interaction as shown to the player. </summary>
            public TransparentProperty<string>? PickupTitle { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }

            /// <summary> Represents what picking up an item looks like for the player performing the action. </summary>
            public TransparentProperty<string>? PickupInvoking { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }


            /// <summary> The Drop interaction's title description as shown to the player. </summary>
            public TransparentProperty<string>? DropTitle { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }

            /// <summary> Describes what an item looks like as its being dropped. </summary>
            public TransparentProperty<string>? DropInvoking { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }


            /// <summary> The Open interaction's title description as shown to the player. </summary>
            public TransparentProperty<string>? OpenTitle { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }

            /// <summary> Describes what an inventory looks like as its being opened. </summary>
            public TransparentProperty<string>? OpenInvoking { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }


            /// <summary> The title description shown to the player for the interaction responsible for transferring items between two different ambiguous inventories. </summary>
            public TransparentProperty<string>? LoadIntoInventoryTitle { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }

            /// <summary> Describes what it looks like as two items are transferred between eachother ambiguously. </summary>
            public TransparentProperty<string>? LoadIntoInventoryInvoking { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }
        }
    }
}