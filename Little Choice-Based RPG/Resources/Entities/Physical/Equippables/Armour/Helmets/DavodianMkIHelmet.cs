using Little_Choice_Based_RPG.Resources.Choices;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Equippables.Armour;
using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Resources.Systems.Damage.Break;
using Little_Choice_Based_RPG.Types.EntityProperty;
using Little_Choice_Based_RPG.Types.InteractDelegate.InteractDelegates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static Little_Choice_Based_RPG.Resources.Choices.Choice;
using static Little_Choice_Based_RPG.Types.InteractDelegate.InteractDelegates.InteractDelegate;
using static Little_Choice_Based_RPG.Types.InteractDelegate.InteractDelegates.InteractWithNothing;
using static Little_Choice_Based_RPG.Types.InteractDelegate.InteractDelegates.InteractWithTargetObject;

namespace Little_Choice_Based_RPG.Resources.Entities.Physical.Equippables.Armour.Helmets
{
    internal class DavodianMkIHelmet : Helmet, IBreakable
    {
        private protected bool isAudioBroken = true;
        public DavodianMkIHelmet(string setName, string newGenericDescriptor, string newInspectDescriptor, decimal setWeightInKG = 0m)
: base(setName, newGenericDescriptor, newInspectDescriptor, setWeightInKG)
        {
            Name.Value = "Davodian MkI Covered Faceplate";
            descriptor.generic.original = "Discarded aside here is your original Davodian MkI, its dented gunmetal grey faceplate long lustreless";
            descriptor.inspect.original = "Disfigured and mottled from years of abuse, the gunmetal grey protective sensors relay a live feed, protecting your vision and perceptible hearing.\r\nThe units bodywork has been long since reworked in patchwork copper shielding - under the seams near-charred components made up of scrapped debris poke through residual sand.";
            EquipDescriptor = "Dusty residue coats your hands as you heave the thick, heavy metal above you and press your forehead in.\r\nInterlocking clicks engage behind your neck, a gentle buzz as the metal comes online.";
            UnequipDescriptor = "Engaging the clasp at the rear, the locks reluctantly scrape their disengaging clicks and the full weight of the visor bears down on your head.\r\nSandpaper lining scratches the sides of your face when you tilt your head forwards and force the faceplate off.";
        }
        public override List<Choice> GenerateChoices()
        {
            List<Choice> choices = new List<Choice>();
            choices.AddRange(HandleFixChoices());
            return choices;
        }

        private void HandleFixChoices()
        {
            List<Choice> choices = new List<Choice>();

            if (isAudioBroken == true)
            {
                InteractUsingTargetObject repairDelegate = Repair;
                InteractionChoices.Add(new InteractWithTargetObject(repairDelegate, "Repair - Re-calibrate the helmets longitudinal wave sensor-array.", "You fixed the helmet, yayy"));
            }

            if (isAudioBroken == false)
            {
                InteractUsingNothing breakDelegate = Break;
                InteractionChoices.Add(new InteractWithNothing(breakDelegate, "Damage - Intentionally misalign the helmets longitudinal wave sensor-array", "its broken again oh nooo"));

                foreach (IInvokableInteraction interaction in InteractionChoices)
                {
                    interaction.Invoke();
                }
            }
        }

        public void Repair(GameObject requiredObject)
        {
            isAudioBroken = false;
            Console.WriteLine("You fixed the helmet, yayy");
        }
        public void Break()
        {
            isAudioBroken = true;
            Console.WriteLine("its broken again oh nooo");
        }
    }
}