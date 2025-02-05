using Little_Choice_Based_RPG.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Entities.Base
{
    public class GameObject
    {
        private protected static uint globalCounter;

        private protected GameObject(string setName, uint setPosition, decimal setWeightInKG = 0m)
        {
            ID = ++globalCounter;
            Name.Value = setName;
            Position = setPosition;
            WeightInKG = setWeightInKG;
        }

        private protected virtual void SetPosition(uint newPosition) => Position = newPosition;

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

        public uint ID { get; init; } = 0U; // 0 is an null, Invalid ID
        public SanitizedString Name { get; set; } = new SanitizedString("i'm an error");
        public uint Position { get; private protected set; }
        public HashSet<GameObject> AttachedObjects { get; private protected set; } = [];
        public decimal WeightInKG { get; private protected set; }
    }
}
