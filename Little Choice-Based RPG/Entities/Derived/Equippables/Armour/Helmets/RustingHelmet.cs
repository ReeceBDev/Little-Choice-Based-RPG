using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Entities.Derived.Equippables.Armour.Helmets
{
    internal class RustingHelmet : Helmet
    {
        public RustingHelmet(Vector2 setPosition) : base(setPosition)
        {
            base.Name.Value = "Rusting Helmet";
            base.descriptor.generic.Value = "Folorn, a mudcaked helmet rots here in its own scrap, long since abandoned.";
            descriptor.inspect.Value = ""
        }

        public override void Interact()
        {
            
        }
    }
}
