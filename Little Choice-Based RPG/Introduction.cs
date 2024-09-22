/*using Little_Choice_Based_RPG.Frontend;
using Little_Choice_Based_RPG.Objects.Gear;
using Little_Choice_Based_RPG.World.Rooms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Little_Choice_Based_RPG
{
    public class Introduction
    {
        public static string currentDialogue = string.Empty;
        public static void Begin()
        {
            currentDialogue = UserInterface.WriteDialogue(@"You wake up with a start - you breath in sharply and sputter as heavy dust dries your mouth. 
In front of you is the cracked and charred sandstone ground of the Potsun Burran. It glitters with the debris of a thousand shredded spaceships.
The high-pitched drone you hear subsides in to a roar as you realise you are laying on the ground, face-first."
            );
        }
        public static void BeginSecond()
        {
            currentDialogue = UserInterface.WriteDialogue("Around you the ground is torn up as a hailstorm of sharded debris thunders across the landscape.");
        }
        public static void BeginThird()
        {
            if (player1.CanHear == true)
            {
                currentDialogue = UserInterface.WriteDialogue("A high pitched machinistic whirring approaches suddenly from your left.");
            }
            currentDialogue = UserInterface.WriteDialogue("An almighty chunk of sheet metal narrowly miss you by meters as it thuds in to the ground and splinters terribly.");

            if (player1.PlayerCanHear)
            {
                currentDialogue = UserInterface.WriteDialogue("Seconds later a rush of hot air from above shoves at you as a blistering cracking sound descendes upon you, as a part of the monolithic structures above is split in to two.");
            }
        }

        public Room NorthOfAtriiKaal => northOfAtriiKaal;
    }
}
*/