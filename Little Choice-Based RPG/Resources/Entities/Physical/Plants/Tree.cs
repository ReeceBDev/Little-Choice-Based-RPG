using Little_Choice_Based_RPG.Resources.Choices;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual.Interactions;
using Little_Choice_Based_RPG.Types.EntityProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Little_Choice_Based_RPG.Resources.Choices.Choice;
using static Little_Choice_Based_RPG.Types.InteractDelegate.InteractDelegates.InteractDelegate;

namespace Little_Choice_Based_RPG.Resources.Entities.Physical.Plants
{
    public class Tree : InteractableObject
    {
        private protected bool isAudioBroken = true;
        public Tree(string setName, string newGenericDescriptor, string newInspectDescriptor, decimal setWeightInKG = 0m)
    : base(setName, newGenericDescriptor, newInspectDescriptor, setWeightInKG)
        {
            entityProperties.CreateProperty("IsBurnt", false); // temporarily burnt the trees
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
                InteractOnSourcePropertiesUsingTargetObject repairInteractDelegate = Repair;
                choices.Add(new Choice("Repair - Re-calibrate the helmets longitudinal wave sensor-array.", this, repairInteractDelegate));
            }

            if (isAudioBroken == false)
            {
                InteractOnSourceProperties breakInteractDelegate = Break;
                choices.Add(new Choice("Damage - Intentionally misalign the helmets longitudinal wave sensor-array", this, breakInteractDelegate));
            }
            return choices;
        }

        public void Repair(PropertyHandler setEntityProperties, GameObject requiredObject)
        {
            isAudioBroken = false;
            Console.WriteLine("You fixed the helmet, yayy");
        }
        public void Break(PropertyHandler setEntityProperties)
        {
            isAudioBroken = true;
            Console.WriteLine("its broken again oh nooo");
        }
    }
}
