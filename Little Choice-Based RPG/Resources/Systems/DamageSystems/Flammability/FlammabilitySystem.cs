using Little_Choice_Based_RPG.Types.PropertySystem.Archive;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using Little_Choice_Based_RPG.Types.PropertySystem.Systems;
using Little_Choice_Based_RPG.Types.TypedEventArgs.PropertyContainerEventArgs;

namespace Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Flammability
{
    internal sealed class FlammabilitySystem : PropertySystem
    {
        /// <summary> Provide logic for co-ordinating property changes with their relevant methods. </summary>
        protected override void OnObjectChanged(object sender, ObjectChangedEventArgs objectChangedData)
        {

        }

        /// <summary> Provide an initiale </summary>
        public override void InitialiseNewSubscriber(IPropertyContainer sourceContainer, PropertyStore sourceProperties)
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
