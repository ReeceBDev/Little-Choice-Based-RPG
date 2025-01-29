using Little_Choice_Based_RPG.Resources.Entities.Base;
using Little_Choice_Based_RPG.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Objects.Base
{
    internal struct ObjectDescription()
    {
        public SanitizedString generic = new SanitizedString("");
        public SanitizedString native = new SanitizedString("");
        public SanitizedString inspect = new SanitizedString("");
        public SanitizedString distant = new SanitizedString("");
    }
    public class DescriptiveObject : GameObject
    {
        private protected ObjectDescription descriptor = new ObjectDescription();

        public DescriptiveObject(string setName, uint setPosition, string newGenericDescriptor, string newInspectDescriptor, decimal setWeightInKG = 0m)
        : base(setName, setPosition, setWeightInKG)
        {
            descriptor.generic.Value = newGenericDescriptor;

            if (newInspectDescriptor == "")
                descriptor.inspect.Value = $"You inspect this further. Albeit, for all the wisdom you gather, it is unfortunately still just a {Name}, to the limits of your knowledge.";
            else
                descriptor.inspect.Value = newInspectDescriptor;
        }

        private protected void _SetGenericDescriptor(string newGenericDescriptor) => descriptor.generic.Value = newGenericDescriptor;

        public string GenericDescriptor
        {
            get { return descriptor.generic.Value; }
            set { _SetGenericDescriptor(value); }
        }
        public string NativeDescriptor => descriptor.native.Value;
        public string InspectDescriptor => descriptor.inspect.Value;
    }
}
