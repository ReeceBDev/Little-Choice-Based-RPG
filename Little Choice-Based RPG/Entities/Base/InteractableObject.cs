using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Objects.Base
{
    internal abstract class InteractableObject : DescriptiveObject
    {

        public InteractableObject(string setName, string newGenericDescriptor, Vector2 setPosition, string newInspectDescriptor = "",
            string newNativeDescriptor = "", decimal setWeightInKG = 0m)
            : base(setName, newGenericDescriptor, setPosition, newInspectDescriptor, newNativeDescriptor, setWeightInKG)
        {
        }

        public abstract void Interact();
    }
}
