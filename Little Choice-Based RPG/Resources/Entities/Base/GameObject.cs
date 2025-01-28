using Little_Choice_Based_RPG.Entities.Derived;
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

        private protected GameObject(Vector2 setPosition)
        {
            ID = ++globalCounter;
            Position = setPosition;
        }
        private protected GameObject(string setName, decimal setWeightInKG = 0m)
        {
            ID = ++globalCounter;
            Name.Value = setName;
            WeightInKG = setWeightInKG;
            Position = new Vector2(0f, 0f);
        }
        private protected GameObject(string setName, decimal setWeightInKG, Vector2 setPosition) : this(setName, setWeightInKG)
        {
            Position = setPosition;
        }

        private protected void SetName(string newName)
        {
            Name.Value = newName;
        }

        private protected virtual void SetPosition(Vector2 newPosition) => Position = newPosition;

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
        public Vector2 Position { get; private protected set; } = new Vector2(0f, 0f);
        public HashSet<GameObject> AttachedObjects { get; private protected set; } = [];
        public decimal WeightInKG { get; private protected set; }
    }
}
