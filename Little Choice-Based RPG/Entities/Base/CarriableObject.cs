using Little_Choice_Based_RPG.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Objects.Base
{
    internal class CarriableObject : InteractableObject
    {
        private protected string pickupDescriptor;
        private protected string putDownDescriptor;
        private protected uint weightInKG = 0;
        private protected bool beingCarried = false;
        public CarriableObject(string setName, string newGenericDescriptor, string newNativeDescriptor, string newInspectDescriptor) : base(setName, newGenericDescriptor, newNativeDescriptor, newInspectDescriptor)
        {
            pickupDescriptor = ($"You reach out your fist and take the {this.Name} in to your posession.");
            putDownDescriptor = ($"You grab the {this.Name} and place it down.");
        }

        public override void Interact()
        {
            if (this.beingCarried) 
                PutDown(); 
            else 
                Pickup();
        }

        public virtual void HandleCarryChoices()
        {
            if (this.beingCarried)
                ChoiceHandler.Add(new Choice($"Put down the {this.Name}.", Pickup));
            else
                ChoiceHandler.Add(new Choice($"Take the {this.Name}.", PutDown));
        }
        public virtual void Pickup()
        {
            this.beingCarried = false;
            Console.WriteLine(pickupDescriptor);
        }

        public virtual void PutDown()
        {
            this.beingCarried = true;
            Console.WriteLine(putDownDescriptor);
        }
    }
}
