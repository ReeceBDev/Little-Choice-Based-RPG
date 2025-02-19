using Little_Choice_Based_RPG.Resources.Choices;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Little_Choice_Based_RPG.Resources.Choices.Choice;

namespace Little_Choice_Based_RPG.Resources.Entities.Physical.Plants
{
    public class Tree : InteractableObject
    {
        private protected bool isAudioBroken = true;
        public Tree(string setName, string newGenericDescriptor, string newInspectDescriptor, decimal setWeightInKG = 0m)
    : base(setName, newGenericDescriptor, newInspectDescriptor, setWeightInKG)
        {
            entityProperties.Add(new EntityProperty("isBurnt", false)); // temporarily burnt the trees
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
