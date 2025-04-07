using Little_Choice_Based_RPG.Types.EntityProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources
{
    public interface IPropertyContainer
    {
        public PropertyHandler Properties { get; set; }
    }
}
