using Little_Choice_Based_RPG.Entities.Derived.Living.Players;
using Little_Choice_Based_RPG.Managers.World;
using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Managers.Player.Frontend.Description
{
    public class DescriptionHandler
    {
        private protected SanitizedString contextualDescription;
        private protected SanitizedString baseDescription;

        public string GetLastActionDescription(Player player)
        {
            return player.LastActionDescriptor.Value;
        }
        public string GetRoomDescriptors(GameEnvironment currentEnvironment, uint roomToDescribe)
        {
            Room foundRoom = currentEnvironment.FindRoomByID(roomToDescribe);

            if (foundRoom == null)
                throw new ArgumentNullException("Room not found in given environment.");

            return foundRoom.Descriptors.initial;
        }
    }.











}
