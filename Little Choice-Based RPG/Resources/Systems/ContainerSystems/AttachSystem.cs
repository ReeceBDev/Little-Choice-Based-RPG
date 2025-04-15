using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems.ContainerSystems
{
    /// <summary> Allows objects to connect to other objects within their room/container. Will follow them around, since they are attached. Requires WeightbearingLogic</summary>
    public class AttachSystem : PropertyLogic
    {
        WeightbearingLogic weightBearingLogicInstantiation = WeightbearingLogic.Instance;
    }
}
