using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG
{
    public struct ObjectDescription
    {
        string generic;
        string initial;
    }
    internal class GenericObject
    {
        static uint currentID = 0;
        uint uniqueID;
        string name;
        string descriptor;
        // List<GameWorldActions> actions;

        public GenericObject(string setName, string setDescriptor)
        {
            uniqueID = ++currentID;
            name = setName;

            // how to add descriptions, again? try to understand it, and what is going on.
        }
    }
}
