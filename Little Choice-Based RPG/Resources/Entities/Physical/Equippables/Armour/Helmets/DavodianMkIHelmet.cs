using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Equippables.Armour;
using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Resources.Systems.Damage.Break;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.Interactions.InteractDelegate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static Little_Choice_Based_RPG.Types.Interactions.InteractDelegate.Interaction;
using static Little_Choice_Based_RPG.Types.Interactions.InteractDelegate.InteractionUsingNothing;
using static Little_Choice_Based_RPG.Types.Interactions.InteractDelegate.InteractionUsingGameObject;
using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface;
using Little_Choice_Based_RPG.Resources.Systems.Damage.Repair;

namespace Little_Choice_Based_RPG.Resources.Entities.Physical.Equippables.Armour.Helmets
{
    internal class DavodianMkIHelmet : Helmet, IBreakable, IRepairable
    {
        private protected bool isAudioBroken = true;
        public DavodianMkIHelmet(string setName, string newGenericDescriptor, string newInspectDescriptor, decimal setWeightInKG = 0m)
: base(setName, newGenericDescriptor, newInspectDescriptor, setWeightInKG)
        {
            //If these need defining, define them on the BASE OBJECTS that this inherits from!!!!!!!!
            entityProperties.UpsertProperty("Name", "Davodian MkI Covered Faceplate");
            entityProperties.UpsertProperty("Descriptor.Generic.Original", "Discarded aside here is your original Davodian MkI, its dented gunmetal grey faceplate long lustreless.");
            entityProperties.UpsertProperty("Descriptor.Inspect.Original", "Disfigured and mottled from years of abuse, the gunmetal grey protective sensors relay a live feed, protecting your vision and perceptible hearing.\r\nThe units bodywork has been long since reworked in patchwork copper shielding - under the seams near-charred components made up of scrapped debris poke through residual sand.")
            //Move these to PROPERTIES :)
            /*
            Name.Value = "Davodian MkI Covered Faceplate";
            descriptor.generic.original = "Discarded aside here is your original Davodian MkI, its dented gunmetal grey faceplate long lustreless";
            descriptor.inspect.original = "Disfigured and mottled from years of abuse, the gunmetal grey protective sensors relay a live feed, protecting your vision and perceptible hearing.\r\nThe units bodywork has been long since reworked in patchwork copper shielding - under the seams near-charred components made up of scrapped debris poke through residual sand.";
            EquipDescriptor = "Dusty residue coats your hands as you heave the thick, heavy metal above you and press your forehead in.\r\nInterlocking clicks engage behind your neck, a gentle buzz as the metal comes online.";
            UnequipDescriptor = "Engaging the clasp at the rear, the locks reluctantly scrape their disengaging clicks and the full weight of the visor bears down on your head.\r\nSandpaper lining scratches the sides of your face when you tilt your head forwards and force the faceplate off.";
            */
            HandleFixChoices();
        }

        private void HandleFixChoices()
        {
            if (isAudioBroken == true)
            {
                InteractionUsingGameObjectDelegate repairDelegate = Repair;
                InteractionChoices.Add(new InteractionUsingGameObject(repairDelegate, "Repair - Re-calibrate the helmets longitudinal wave sensor-array.", "You fixed the helmet, yayy"));
            }

            if (isAudioBroken == false)
            {
                InteractionUsingNothingDelegate breakDelegate = Break;
                InteractionChoices.Add(new InteractionUsingNothing(breakDelegate, "Damage - Intentionally misalign the helmets longitudinal wave sensor-array", "its broken again oh nooo"));
            }
        }

        public void Repair(IUserInterface setMutexIdentity, GameObject requiredObject)
        {
            isAudioBroken = false;
            Console.WriteLine("You fixed the helmet, yayy");
        }
        public void Break(IUserInterface setMutexIdentity)
        {
            isAudioBroken = true;
            Console.WriteLine("its broken again oh nooo");
        }
    }
}