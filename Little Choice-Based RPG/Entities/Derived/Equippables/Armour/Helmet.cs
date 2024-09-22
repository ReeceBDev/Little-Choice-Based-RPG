using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Little_Choice_Based_RPG.Objects.Base;

namespace Little_Choice_Based_RPG.Entities.Derived.Equippables.Armour
{
    internal class Helmet : EquippableObject
    {
        private static string defaultGenericDescriptor = "Folorn, a mudcaked helmet rots here in its own scrap, long since abandoned.";
        private static string defaultName = "Rusting Helmet";
        private static decimal defaultWeightInKG = 1.8m;
        public Helmet(Vector2 setPosition) : base(defaultName, defaultGenericDescriptor, setPosition, setWeightInKG: defaultWeightInKG)
        {
        }
        public Helmet(string setName, string newGenericDescriptor, Vector2 setPosition, string newInspectDescriptor = "",
    string newNativeDescriptor = "", decimal setWeightInKG = 1.8m, string equipDescriptor = "", string unequipDescriptor = "")
    : base(setName, newGenericDescriptor, setPosition, newInspectDescriptor, newNativeDescriptor, setWeightInKG)
        {

        }
    }
}
