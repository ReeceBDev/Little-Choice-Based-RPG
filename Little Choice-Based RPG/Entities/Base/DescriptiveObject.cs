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
    internal struct ObjectDescription
    {
        public SanitizedString generic;
        public SanitizedString native;
        public SanitizedString inspect;
        public SanitizedString distant;
    }
    internal class DescriptiveObject : GameObject
    {
        private protected ObjectDescription descriptor;

        public DescriptiveObject(string setName, string newGenericDescriptor, Vector2 setPosition, string newInspectDescriptor = "", 
            string newNativeDescriptor = "", decimal setWeightInKG = 0m)
            : base(setName, setWeightInKG, setPosition)
        {
            descriptor.generic.Value = newGenericDescriptor;
            descriptor.native.Value = newNativeDescriptor == "" ? newGenericDescriptor : newNativeDescriptor;

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
