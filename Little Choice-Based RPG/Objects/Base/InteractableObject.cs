using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Objects.Base
{
    internal abstract class InteractableObject : DescriptiveObject
    {

        InteractableObject(string setName, string newGenericDescriptor, string newNativeDescriptor, string newInspectDescriptor) : base(setName, newGenericDescriptor, newNativeDescriptor, newInspectDescriptor)
        {
        }

        public abstract void Interact();
    }
}
