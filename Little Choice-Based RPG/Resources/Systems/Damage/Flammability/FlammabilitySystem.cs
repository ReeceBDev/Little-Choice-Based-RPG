using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types.EntityProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems.Damage.Flammability
{
    public class FlammabilitySystem : ComponentSystem
    {
        private GameObject parentObject;
        static FlammabilitySystem()
        {
            //Component
            PropertyValidation.CreateValidProperty("Component.Flammable", PropertyType.Boolean);

            //Logic
            PropertyValidation.CreateValidProperty("IsFlammable", PropertyType.Boolean);
            PropertyValidation.CreateValidProperty("Flammability.IsBurning", PropertyType.Boolean);
            PropertyValidation.CreateValidProperty("Flammability.IsBurnt", PropertyType.Boolean);
        }

        public FlammabilitySystem(GameObject instantiatingObject)
        {
            parentObject = instantiatingObject;
        }

        public void SetAflame()
        {
            // This class needs to be event driven :)
        }
        public void Extinguish()
        {

        }
    }
}
