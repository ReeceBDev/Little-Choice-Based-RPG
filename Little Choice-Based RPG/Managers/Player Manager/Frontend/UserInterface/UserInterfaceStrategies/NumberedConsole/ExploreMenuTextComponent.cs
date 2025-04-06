using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface.UserInterfaceStrategies.NumberedConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface.UserInterfaceStrategies.NumberedConsole
{
    public struct ExploreMenuTextComponent
    {
        static List<ExploreMenuIdentity> usedIdentifiers = new List<ExploreMenuIdentity>();
        public ExploreMenuTextComponent(ExploreMenuIdentity setIdentifier, string setContent, uint setWriteSpeed)
        {
            AddIdentifier(setIdentifier);
            Content = setContent;
            WriteSpeed = setWriteSpeed;
        }

        private void AddIdentifier(ExploreMenuIdentity prospectiveIdentifier)
        {
            if (!(prospectiveIdentifier == ExploreMenuIdentity.None))
                if (usedIdentifiers.Contains(prospectiveIdentifier))
                    throw new ArgumentException($"This ExploreMenuIdentifier of {prospectiveIdentifier} is already in use! Only one of each unique Identifier is allowed, except None. Try setting to {ExploreMenuIdentity.None}, since they are unlimited.");

            usedIdentifiers.Add(prospectiveIdentifier);
            UniqueIdentity = prospectiveIdentifier;
        }

        public ExploreMenuIdentity UniqueIdentity { get; private set; } // Can only use one of each, across all instances UserInterfaceTextEntry, except None, which can be used forever.
        public string Content { get; set; }
        public uint WriteSpeed { get; set; }
    }
}
