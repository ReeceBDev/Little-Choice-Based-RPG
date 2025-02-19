using Little_Choice_Based_RPG.Resources.Choices;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Equippables.Armour;
using Little_Choice_Based_RPG.Resources.Rooms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static Little_Choice_Based_RPG.Resources.Choices.Choice;

namespace Little_Choice_Based_RPG.Resources.Entities.Physical.Equippables.Armour.Helmets
{
    internal class DavodianMkIHelmet : Helmet
    {
        private protected bool isAudioBroken = true;
        public DavodianMkIHelmet(string setName, string newGenericDescriptor, string newInspectDescriptor, decimal setWeightInKG = 0m)
: base(setName, newGenericDescriptor, newInspectDescriptor, setWeightInKG)
        {
            Name.Value = "Davodian MkI Covered Faceplate";
            descriptor.generic.Value = "Discarded aside here is your original Davodian MkI, its dented gunmetal grey faceplate long lustreless";
            descriptor.inspect.Value = "Disfigured and mottled from years of abuse, the gunmetal grey protective sensors relay a live feed, protecting your vision and perceptible hearing.\r\nThe units bodywork has been long since reworked in patchwork copper shielding - under the seams near-charred components made up of scrapped debris poke through residual sand.";
            EquipDescriptor = "Dusty residue coats your hands as you heave the thick, heavy metal above you and press your forehead in.\r\nInterlocking clicks engage behind your neck, a gentle buzz as the metal comes online.";
            UnequipDescriptor = "Engaging the clasp at the rear, the locks reluctantly scrape their disengaging clicks and the full weight of the visor bears down on your head.\r\nSandpaper lining scratches the sides of your face when you tilt your head forwards and force the faceplate off.";
        }
        public override List<Choice> GenerateChoices()
        {
            List<Choice> choices = new List<Choice>();
            choices.AddRange(HandleFixChoices());
            return choices;
        }
        private List<Choice> HandleFixChoices()
        {
            List<Choice> choices = new List<Choice>();

            if (isAudioBroken == true)
            {
                Interact repair = new Interact(InteractArguments => Repair());
                choices.Add(new Choice("Repair - Re-calibrate the helmets longitudinal wave sensor-array.", this, repair));
            }

            if (isAudioBroken == false)
            {
                Interact breakAgain = new Interact(InteractArguments => Break());
                choices.Add(new Choice("Damage - Intentionally misalign the helmets longitudinal wave sensor-array", this, breakAgain));
            }

            // TEST
            EntityProperty damageTargetHasHealthRequirement = new EntityProperty("HasHealth", true);
            Object[] damageTargetRequirements = [damageTargetHasHealthRequirement];

            Interact shoot = new Interact(InteractArguments => Shoot(GameObjectArgument1));
            choices.Add(new Choice("Shoot - [Debug] Damage Something Else - Shoot Something With A Laser Beam (TEST)", this, shoot, damageTargetRequirements, ChoiceRole.MenuCompatible));

            return choices;
        }
        public string Repair()
        {
            isAudioBroken = false;
            return "you fixed the helmet! yay!";
        }
        public string Break()
        {
            isAudioBroken = true;
            return "its broken again oh nooo";
        }

        // TEST
        public string Shoot(GameObject targetObject)
        {
            return $"you use a laser mounted on the helmet to blast the {targetObject.Name}";
        }
    }
}