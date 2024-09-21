using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Little_Choice_Based_RPG.Frontend;

namespace Little_Choice_Based_RPG.OldCode
{
    public class Outcomes
    {
        public static void ChoiceA()
        {
            UserInterface.WriteDialogue(@"
You deliberately thud your helmet to try to re-align the speakers.

The roar grows in intensity as distinguishable sounds become sharp and jagged.
The riptide of two-tone thruster engines above you sputter hot air violently as behemoth silhouettes loom above you, vast machines engaged in mortal combat.
You feel buffeted by fear and instinct as the speakers kick in with a whip-like crack.");
        }

        public static void ChoiceB()
        {
            UserInterface.WriteDialogue(@"Bringing your elbows close to your chest, you strain to stand as rubble rolls off you.

You are kneeling in the dirt as the ground shakes violently and you feel hot oil streaking down the back of your neck.
Dust roils and grit is thrown across you as you right yourself upwards enough to see a vast and littered landscape, overcast by the shadows of the vicious dance above you.
You are in danger here and at the mercy of chance.");
        }
    }
}
