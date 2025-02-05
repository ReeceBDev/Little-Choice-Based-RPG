using Little_Choice_Based_RPG.Resources.Choices;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Equippables.Armour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Entities.Physical.Equippables.Armour.Helmets
{
    internal class RustingHelmet : Helmet
    {
        public RustingHelmet(Vector2 setPosition) : base(setPosition)
        {
            Name.Value = "Rusting Helmet";
            descriptor.generic.Value = "Folorn, a mudcaked helmet rots here in its own scrap, long since abandoned.";
            descriptor.inspect.Value = "";
        }
        public override List<Choice> GenerateChoices()
        {
            throw new NotImplementedException();
        }
    }
}
