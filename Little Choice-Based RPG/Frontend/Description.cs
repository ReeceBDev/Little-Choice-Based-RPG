using Little_Choice_Based_RPG.Entities.Derived.Living.Players;
using Little_Choice_Based_RPG.Types;
using Little_Choice_Based_RPG.World.Managers;
using Little_Choice_Based_RPG.World.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Frontend
{
    public class Description
    {
        private protected SanitizedString contextualDescription;
        private protected SanitizedString baseDescription;

        public string GetDescription(Player player)
        {
            return player.PlayerDescriptor.Value;
        }
        public string GetRoomDescriptor(GameEnvironment currentEnvironment, uint roomToDescribe)
        {
            Room foundRoom = currentEnvironment.FindRoomByID(roomToDescribe);

            if (foundRoom == null)
                throw new ArgumentNullException("Room not found in given environment.");

            return foundRoom.Descriptors.initial;
        }
    }
}
