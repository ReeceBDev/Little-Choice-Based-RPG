using Little_Choice_Based_RPG.Resources.Entities;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.TypedEventArgs.PropertyContainerEventArgs;

namespace Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Flammability
{
    internal class FlammabilitySystem : PropertyLogic
    {
        static FlammabilitySystem()
        {
            //Logic
            PropertyValidation.CreateValidProperty("IsFlammable", PropertyType.Boolean);
            PropertyValidation.CreateValidProperty("Flammability.IsBurning", PropertyType.Boolean);
            PropertyValidation.CreateValidProperty("Flammability.IsBurnt", PropertyType.Boolean);
        }

        /// <summary> Provide logic for co-ordinating property changes with their relevant methods. </summary>
        protected override void OnObjectChanged(object sender, ObjectChangedEventArgs objectChangedData)
        {

        }

        /// <summary> Provide an initiale </summary>
        protected override void InitialiseNewSubscriber(PropertyContainer sourceContainer, PropertyHandler sourceProperties)
        {

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
