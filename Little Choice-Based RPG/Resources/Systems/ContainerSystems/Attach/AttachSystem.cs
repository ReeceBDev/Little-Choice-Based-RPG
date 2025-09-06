using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using Little_Choice_Based_RPG.Types.PropertySystem.Systems;
using Little_Choice_Based_RPG.Types.TypedEventArgs.PropertyContainerEventArgs;

namespace Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Attach
{
    /// <summary> Allows objects to connect to other objects within their room/container. Will follow them around, since they are attached. Requires WeightbearingLogic</summary>
    internal sealed class AttachSystem : PropertySystem
    {

        public override void InitialiseNewSubscriber(IPropertyContainer sourceContainer, PropertyStore sourceProperties)
        {

        }

        protected override void OnObjectChanged(object sender, ObjectChangedEventArgs objectChangedData)
        {

        }
    }
}
