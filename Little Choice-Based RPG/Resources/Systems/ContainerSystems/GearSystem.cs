using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Systems.SystemEventBus;
using Little_Choice_Based_RPG.Types.EntityProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems.ContainerSystems
{
    /// <summary> Creates GameObject slots that can be filled with specific types for each slot definition. Requires WeightbearingCommon</summary>
    public class GearSystem : PropertyLogic
    {
        //This class requires WeightbearingCommon.
        WeightbearingLogic weightBearingLogicInstantiation = WeightbearingLogic.Instance;

        static GearSystem()
        {
            PropertyValidation.CreateValidProperty("HasGearSlots", PropertyType.Boolean);
            PropertyValidation.CreateValidProperty("Gear.Slot.Helmet.ID", PropertyType.UInt32);
        }

        /// <summary> Provide logic for co-ordinating property changes with their relevant methods. </summary>
        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedData)
        {

        }

        /// <summary> Provide an initiale </summary>
        protected override void InitialiseNewSubscriber(PropertyContainer sourceContainer, PropertyHandler sourceProperties)
        {

        }
    }
}
