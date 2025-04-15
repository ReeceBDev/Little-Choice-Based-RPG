using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.Extensions
{
    public interface IExtension
    {
        public string UniqueIdentifier { get; init; }
    }
}
