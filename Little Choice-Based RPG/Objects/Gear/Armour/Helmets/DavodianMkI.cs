using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Objects.Gear.Armour.Helmets
{
    internal class DavodianMkI : Helmet
    {
        private protected bool isAudioBroken = false;
        public DavodianMkI()
        {
            base.name = "Davodian MkI Covered Faceplate";
            base.descriptor.generic = "Discarded aside here is your original Davodian MkI, its dented gunmetal grey faceplate long lustreless";
            base.descriptor.inspect = "Disfigured and mottled from years of abuse, the gunmetal grey protective sensors relay a live feed, protecting your vision and perceptible hearing.\r\nThe units bodywork has been long since reworked in patchwork copper shielding - under the seams near-charred components made up of scrapped debris poke through residual sand.";
            base.gearDescriptor.equip = "Dusty residue coats your hands as you heave the thick, heavy metal above you and press your forehead in.\r\nInterlocking clicks engage behind your neck, a gentle buzz as the metal comes online.";
            base.gearDescriptor.unequip = "Engaging the clasp at the rear, the locks reluctantly scrape their disengaging clicks and the full weight of the visor bears down on your head.\r\nSandpaper lining scratches the sides of your face when you tilt your head forwards and force the faceplate off.";
        }
    }
}