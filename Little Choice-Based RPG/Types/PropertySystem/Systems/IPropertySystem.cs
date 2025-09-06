using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.PropertySystem.Systems
{
    /// <summary> A marker interface used to declare a class as a system which may utilise properties within the overarching ECS' property system. 
    /// Note: While usually an anti-pattern, in this case a marker interface is sensible as its being used for type constraints. </summary>
    internal interface IPropertySystem
    {
        public void InitialiseContainer(IPropertyContainer i); //TEMP TEST. MAY REMOVE LATER POTENTIALLY. PROBABLY KEEP THO ESP. IF DERIVED SYSTEMS CAN BE MADE STATIC!
    }
}
