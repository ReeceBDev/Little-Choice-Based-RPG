using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.PropertyExtensions.PropertyExtensionEventArgs
{
    public record struct PropertyExtensionChangedArgs(string UniqueIdentifier, object SubjectChanged);
}
