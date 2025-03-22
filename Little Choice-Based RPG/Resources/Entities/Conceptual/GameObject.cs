using Little_Choice_Based_RPG.Resources.Choices;
using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Types;
using Little_Choice_Based_RPG.Types.EntityProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Entities.Conceptual
{
    public record struct EntityProperty(string Key, object Value);
    public class GameObject
    {
        //private PropertyHandler entityProperties;

        public List<EntityProperty> entityProperties = new List<EntityProperty> (); // Comment out once new entityProperties is in.

        private protected static uint globalCounter;

        private protected GameObject(string setName, decimal setWeightInKG = 0m)
        {
            ID = ++globalCounter;
            Name.Value = setName;
            WeightInKG = setWeightInKG;
        }

        public virtual List<Choice> GenerateChoices()
        {
            List<Choice> choices = new List<Choice>();
            // Handle additional choices here.
            // I.e. choices.Add(HandleAttachChoices());
            return choices;
        }

        private protected void Attach(GameObject attachee) // Cojoin together with another object
        {
            AttachedObjects.Add(attachee);
            attachee.Attach(this);
        }
        private protected void Unattach(GameObject attachee) // Unattach self from another object and unattach the other object from self
        {
            AttachedObjects.Remove(attachee);
            attachee.Unattach(this);
        }

        public string TakeDamage(uint healthToLose)
        {
            return $"you lose {healthToLose}";
        }

        public uint ID { get; init; } = 0U; // 0 is an null, Invalid ID
        public SanitizedString Name { get; set; } = new SanitizedString("i'm an error");
        public HashSet<GameObject> AttachedObjects { get; private protected set; } = [];
        public decimal WeightInKG { get; private protected set; }
    }
}
