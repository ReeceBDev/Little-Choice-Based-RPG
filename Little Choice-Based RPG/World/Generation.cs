using Little_Choice_Based_RPG.Entities.Derived.Equippables.Armour.Helmets;
using Little_Choice_Based_RPG.Entities.Derived.Living.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Generation
{
    internal class Generation
    {
        public Player GenerateCharacter(uint spawnRoom = Introduction.NorthOfAtriiKaal.ID)
        {
            Player player1 = new Player(0, 0); 
            return player1;
        }
        

        public void GeneratePhysicalObject()
        {
            DavodianMkIHelmet helmet1 = new DavodianMkI();
        }
    }
}
