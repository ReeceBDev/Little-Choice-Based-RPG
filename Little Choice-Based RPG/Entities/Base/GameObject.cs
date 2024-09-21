using Little_Choice_Based_RPG.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Objects.Base
{
    internal class GameObject
    {
        private protected static uint globalCounter;

        private protected GameObject(string setName)
        {
            ID = ++globalCounter;
            Name.Value = setName;
            Position = new Vector2(0f, 0f);
        }
        private protected GameObject(string setName, Vector2 setPosition) : this(setName)
            => Position = setPosition;

        private protected void SetName(string newName)
        {
            Name.Value = newName;
        }

        private protected virtual void SetPosition(Vector2 newPosition) => Position = newPosition;

        public uint ID { get; init; } = 0U; // 0 is an null, Invalid ID
        public SanitizedString Name { get; set; } = new SanitizedString(string.Empty);
        public Vector2 Position { get; set; } = new Vector2(0f, 0f);
    }
}
