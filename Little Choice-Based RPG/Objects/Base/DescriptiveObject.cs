using Little_Choice_Based_RPG.Types;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
    internal class DescriptiveObject : GameObject
    {
        private protected ObjectDescription descriptor;

        public DescriptiveObject(string setName, string newGenericDescriptor, string newNativeDescriptor, string newInspectDescriptor) 
            : base(setName)
        {
            descriptor.generic.Value = newGenericDescriptor;
            descriptor.native.Value = (newNativeDescriptor == string.Empty) ? newGenericDescriptor : newNativeDescriptor;

            if (newInspectDescriptor == string.Empty)
                descriptor.inspect.Value = newGenericDescriptor;
            else
                descriptor.inspect.Value = $"You inspect this further. Albeit, for all the wisdom you gather, it is unfortunately still just a {base.Name}, to the limits of your knowledge.";
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
