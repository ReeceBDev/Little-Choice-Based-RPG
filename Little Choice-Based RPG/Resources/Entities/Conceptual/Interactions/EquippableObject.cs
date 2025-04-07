using Little_Choice_Based_RPG.Resources.Entities.Conceptual.Descriptive_Derivatives;
using Little_Choice_Based_RPG.Types;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Entities.Conceptual.Interactions
{
    public abstract class EquippableObject : InteractableObject
    {
        private readonly static Dictionary<string, PropertyType> requiredProperties = new Dictionary<string, PropertyType>()
        {
            {"Descriptor.IsEquipped", PropertyType.Boolean},
            {"Descriptor.Equip", PropertyType.String},
            {"Descriptor.Unequip", PropertyType.String},
        };

        private readonly static Dictionary<string, PropertyType> optionalProperties = new Dictionary<string, PropertyType>()
        {

        };

        private readonly static Dictionary<string, object> defaultProperties = new Dictionary<string, object>()
        {
            {"Descriptor.IsEquipped", false},
            {"Descriptor.Equip", "You equip this and feel much better prepared for the Potsun Burran and its challenges." },
            {"Descriptor.Unequip", "Unequipping it with due care, you free yourself up for something else in its place." }
        };

        static EquippableObject()
        {
            //Define new required and optional ValidProperties for this class
            DeclareNewPropertyTypes(requiredProperties);
            DeclareNewPropertyTypes(optionalProperties);
        }

        private protected EquippableObject(Dictionary<string, object>? derivedProperties = null)
            : base(SetLocalProperties(derivedProperties ??= new Dictionary<string, object>()))
        {
            //Validate required properties have been set on entityProperties
            ValidateRequiredProperties(requiredProperties);
        }

        private static Dictionary<string, object> SetLocalProperties(Dictionary<string, object> derivedProperties)
        {
            //Apply default properties for this class to the current list of derivedProperties
            ApplyDefaultProperties(derivedProperties, defaultProperties);

            return derivedProperties; //Return is required to give (base) the derived list.
        }

        /*
        public virtual void HandleEquipChoices()
        {
            if (this.currentlyInUse)
                ChoiceHandler.Add(new Choice($"Equip the {this.Name}.", Equip));
            else
                ChoiceHandler.Add(new Choice($"Unequip the {this.Name}.", Unequip));
        }
        */

        public void Equip()
        {
            Console.WriteLine(Properties.GetPropertyValue("Descriptor.Equip"));
        }
        public void Unequip()
        {
            Console.WriteLine(Properties.GetPropertyValue("Descriptor.Unequip"));
        }
    }
}