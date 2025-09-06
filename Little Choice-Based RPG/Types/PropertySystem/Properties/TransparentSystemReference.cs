using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using Little_Choice_Based_RPG.Types.PropertySystem.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.PropertySystem.Properties
{
    /// <summary> Transparently represents systems in use by boolean. Holds a system reference for later use, as long as it has been initialised. 
    /// Inherits from TransparentProperty - see its description for more details. </summary>
    internal class TransparentSystemReference(bool setIsActivated) : TransparentProperty<bool>(setIsActivated)
    {
        public IPropertySystem? SystemReference { get { return field ?? throw new Exception("Tried to retrieve a system reference before it was set by PropertyHandler! That shouldnt happen!"); } set; }
    }
}