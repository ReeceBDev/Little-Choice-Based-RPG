using Little_Choice_Based_RPG.Objects.Entities.Players;
using Little_Choice_Based_RPG.Objects.Gear.Armour.Helmets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Generation
{
    internal class Generation
    {
        public Player GenerateCharacter(uint spawnRoom = Rooms.Introduction.NorthOfAtriiKaal.ID)
        {
            Player player1 = new Player(spawnRoom); 
            return player1;
        }
        

        public void GeneratePhysicalObject()
        {
            DavodianMkIHelmet helmet1 = new DavodianMkI();
        }
    }
}
