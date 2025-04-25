using Little_Choice_Based_RPG.Resources.PropertyContainerEventArgs;
using Little_Choice_Based_RPG.Resources.Systems;
using Little_Choice_Based_RPG.Types.EntityProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Attach
{
    /// <summary> Allows objects to connect to other objects within their room/container. Will follow them around, since they are attached. Requires WeightbearingLogic</summary>
    public class AttachSystem : PropertyLogic
    {

        protected override void InitialiseNewSubscriber(PropertyContainer sourceContainer, PropertyHandler sourceProperties)
        {

        }

        protected override void OnObjectChanged(object sender, ObjectChangedEventArgs objectChangedData)
        {

        }
    }
}
