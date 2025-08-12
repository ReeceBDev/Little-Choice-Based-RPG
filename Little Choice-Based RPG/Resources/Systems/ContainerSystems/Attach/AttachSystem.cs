using Little_Choice_Based_RPG.Resources.Entities;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.TypedEventArgs.PropertyContainerEventArgs;

namespace Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Attach
{
    /// <summary> Allows objects to connect to other objects within their room/container. Will follow them around, since they are attached. Requires WeightbearingLogic</summary>
    internal class AttachSystem : PropertyLogic
    {

        protected override void InitialiseNewSubscriber(PropertyContainer sourceContainer, PropertyHandler sourceProperties)
        {

        }

        protected override void OnObjectChanged(object sender, ObjectChangedEventArgs objectChangedData)
        {

        }
    }
}
