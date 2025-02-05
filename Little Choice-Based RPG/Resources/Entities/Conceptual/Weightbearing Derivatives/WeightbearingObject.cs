using Little_Choice_Based_RPG.Resources.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Entities.Base.Weightbearing_Derivatives
{
    public class WeightbearingObject
    {
        public GameObject testobject;
        private protected decimal strength; // maximum weightbearing capacity
        private protected decimal totalWeightHeldInKG = 0;
        private protected WeightbearingObject(string setName, uint setPosition, decimal setWeightInKG, decimal setStrengthInKG)
        {
            testobject = new GameObject(setPosition);
            this.strength = setStrengthInKG;
        }

        public virtual void Carry(GameObject gameObject)
        {
            if (strength >= (totalWeightHeldInKG + gameObject.WeightInKG))
            {
                Attach(gameObject);
                totalWeightHeldInKG += gameObject.WeightInKG;
            }
            else
                throw new Exception("the object is too heavy to be picked up by this"); // I dont think this should be handled like this. Shouldnt it just try something else if its an NPC or say this to the player?
        }
        public void Drop(GameObject gameObject)
        {
            Unattach(gameObject);
            totalWeightHeldInKG -= gameObject.WeightInKG;
        }
    }
}
