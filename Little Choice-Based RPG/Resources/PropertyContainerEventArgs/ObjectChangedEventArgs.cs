using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.EventArgs
{
    public record struct ObjectChangedEventArgs(PropertyContainer SourceContainer, string IdentifierChanged, object ValueChanged);
}
