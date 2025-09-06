using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using Little_Choice_Based_RPG.Types.PropertySystem.Systems;
using Little_Choice_Based_RPG.Types.TypedEventArgs.PropertyContainerEventArgs;

namespace Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Gear
{
    /// <summary> Creates GameObject slots that can be filled with specific types for each slot definition.</summary>
    internal sealed class GearSystem : PropertySystem
    {
        /// <summary> Provide logic for co-ordinating property changes with their relevant methods. </summary>
        protected override void OnObjectChanged(object sender, ObjectChangedEventArgs objectChangedData)
        {

        }

        /// <summary> Provide an initiale </summary>
        public override void InitialiseNewSubscriber(IPropertyContainer sourceContainer, PropertyStore sourceProperties)
        {

        }
    }
}
