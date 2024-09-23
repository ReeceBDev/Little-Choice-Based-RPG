using Little_Choice_Based_RPG.Objects.Base;
using Little_Choice_Based_RPG.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Choices
{
    internal class Choice
    {
        private protected static uint globalCounter;
        public Func<string> OnExecute;

        public Choice(string setName, Func<string> onExecuteCallback)
        {
            ID = ++globalCounter;
            Name.Value = setName;
            if (onExecuteCallback != null)
                OnExecute = onExecuteCallback;
        }

        public uint ID { get; init; } = 0U; // 0 is an null, Invalid ID
        public SanitizedString Name { get; private protected set; } = new SanitizedString(string.Empty);
    }
}
