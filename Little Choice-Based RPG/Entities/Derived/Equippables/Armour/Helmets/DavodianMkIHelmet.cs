using Little_Choice_Based_RPG.Choices;
using Little_Choice_Based_RPG.Entities.Derived.Equippables.Armour;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Entities.Derived.Equippables.Armour.Helmets
{
    internal class DavodianMkIHelmet : Helmet
    {
        private protected bool isAudioBroken = true;

        public DavodianMkIHelmet(Vector2 setPosition) : base(setPosition)
        {
            base.Name.Value = "Davodian MkI Covered Faceplate";
            descriptor.generic.Value = "Discarded aside here is your original Davodian MkI, its dented gunmetal grey faceplate long lustreless";
            descriptor.inspect.Value = "Disfigured and mottled from years of abuse, the gunmetal grey protective sensors relay a live feed, protecting your vision and perceptible hearing.\r\nThe units bodywork has been long since reworked in patchwork copper shielding - under the seams near-charred components made up of scrapped debris poke through residual sand.";
            base.equippableDescriptor.equip.Value = "Dusty residue coats your hands as you heave the thick, heavy metal above you and press your forehead in.\r\nInterlocking clicks engage behind your neck, a gentle buzz as the metal comes online.";
            base.equippableDescriptor.unequip.Value = "Engaging the clasp at the rear, the locks reluctantly scrape their disengaging clicks and the full weight of the visor bears down on your head.\r\nSandpaper lining scratches the sides of your face when you tilt your head forwards and force the faceplate off.";
        }

        public override void Interact()
        {
            HandleFixChoices();
        }
        public void HandleFixChoices()
        {
            if (isAudioBroken == true)
            {
                ChoiceHandler.Add(new Choice("Fix my helmet.", FixHelmet));
            }
        }
        public void FixHelmet()
        {
            isAudioBroken = false;
            Console.WriteLine("you fixed the helmet! yay!");
        }
        public void BreakHelmet()
        {
            isAudioBroken = true;
        }
    }
}