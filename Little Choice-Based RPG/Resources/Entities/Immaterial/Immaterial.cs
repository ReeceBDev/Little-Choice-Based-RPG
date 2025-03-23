using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Entities.Immaterial
{
    public class Immaterial : GameObject
    {
        public Immaterial(string setName) : base(setName)
        {
            entityProperties.CreateProperty("isImmaterial", true);
        }
    }
}
