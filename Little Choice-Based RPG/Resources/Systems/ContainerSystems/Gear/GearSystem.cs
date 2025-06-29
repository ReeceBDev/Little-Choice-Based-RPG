﻿using Little_Choice_Based_RPG.Resources.Entities;
using Little_Choice_Based_RPG.Resources.PropertyContainerEventArgs;
using Little_Choice_Based_RPG.Types.EntityProperties;

namespace Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Gear
{
    /// <summary> Creates GameObject slots that can be filled with specific types for each slot definition.</summary>
    public class GearSystem : PropertyLogic
    {
        static GearSystem()
        {
            PropertyValidation.CreateValidProperty("HasGearSlots", PropertyType.Boolean);
            PropertyValidation.CreateValidProperty("Gear.Slot.Helmet.ID", PropertyType.UInt32);
        }

        /// <summary> Provide logic for co-ordinating property changes with their relevant methods. </summary>
        protected override void OnObjectChanged(object sender, ObjectChangedEventArgs objectChangedData)
        {

        }

        /// <summary> Provide an initiale </summary>
        protected override void InitialiseNewSubscriber(PropertyContainer sourceContainer, PropertyHandler sourceProperties)
        {

        }
    }
}
