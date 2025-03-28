using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Types;
using Little_Choice_Based_RPG.Types.EntityProperty;
using Little_Choice_Based_RPG.Types.InteractDelegate;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static Little_Choice_Based_RPG.Resources.Choices.ChoiceHandler;

namespace Little_Choice_Based_RPG.Resources.Choices
{
    public enum ChoiceRole
    {
        None,
        Default,
        MenuCompatible,
    }

    public class Choice
    {
        private protected static uint globalCounter;

        public Choice(string setName, GameObject setSource, Delegate setInteractDelegate, List<DelegateParameter>? setInteractArguments = null, Enum setChoiceRole = null)
        {
            ID = ++globalCounter;
            Name.Value = setName;

            Source = setSource; 
            InteractDelegate = setInteractDelegate;
            
            if (setInteractArguments == null)
                InteractArguments = null;
            else
                InteractArguments = setInteractArguments;

            if (setChoiceRole == null)
                Role = ChoiceRole.None;
        }

        public uint ID { get; init; } = 0U; // 0 is an null, Invalid ID
        public SanitizedString Name { get; private protected set; } = new SanitizedString(string.Empty);
        public GameObject Source { get; private set; }
        public Delegate InteractDelegate { get; private set; }
        public List<DelegateParameter>? InteractArguments { get; private set; }
        public ChoiceRole Role { get; private set; }
    }
}
