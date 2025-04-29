using Little_Choice_Based_RPG.Resources.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.Navigation
{
    /// <summary> Coordinates representing a 3D space, along the X-Axis, Y-Axis and Z-Axis, respectively. </summary>
    public readonly record struct Coordinates(int X, int Y, int Z);
}
