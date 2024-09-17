using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Objects
{
    internal struct ObjectDescription
    {
        public string generic;
        public string native;
        public string inspect;
    }
    internal class GenericObject
    {
        private protected static uint currentID = 0;
        private protected uint uniqueID;

        private protected string name;
        private protected ObjectDescription descriptor;

        //List<GameWorldActions> actions;

        public GenericObject()
        {
            uniqueID = ++currentID;
        }
        public GenericObject(string newName, string newGenericDescriptor, string newNativeDescriptor = null, string newInspectDescriptor = null)
        {
            uniqueID = ++currentID;

            name = newName;
            descriptor.generic = newGenericDescriptor;

            if (newNativeDescriptor == null)
                descriptor.native = newGenericDescriptor;
            else
                descriptor.native = newNativeDescriptor;

            if (newInspectDescriptor == null)
                descriptor.inspect = newGenericDescriptor;
            else
                descriptor.inspect = $"You inspect this further. Albeit, for all the wisdom you gather, it is unfortunately still just a {newName}, to the limits of your knowledge.";
        }
        
        public string Name => this.name;
        public uint ID => this.uniqueID;
        public string GenericDescriptor => this.descriptor.generic;
        public string NativeDescriptor => this.descriptor.native;
        public string InspectDescriptor => this.descriptor.inspect;
    }
}
