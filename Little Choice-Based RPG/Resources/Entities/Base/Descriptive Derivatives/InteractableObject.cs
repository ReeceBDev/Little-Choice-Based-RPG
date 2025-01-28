using Little_Choice_Based_RPG.Resources.Choices;
using Little_Choice_Based_RPG.Types;
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

        public InteractableObject(Vector2 setPosition) : base(setPosition) { }
        public InteractableObject(string setName, string newGenericDescriptor, Vector2 setPosition, string newInspectDescriptor = "",
            string newNativeDescriptor = "", decimal setWeightInKG = 0m)
            : base(setName, newGenericDescriptor, setPosition, newInspectDescriptor, newNativeDescriptor, setWeightInKG)
        {
        }

        public abstract List<Choice> GenerateChoices();
    }
}
