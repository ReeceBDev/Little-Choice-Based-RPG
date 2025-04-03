using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Types;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.Interactions.InteractDelegate;
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
    //public record struct EntityProperty(string Key, object Value);
    public class GameObject : PropertyContainer
    {
        public List<IInvokableInteraction> InteractionChoices = new List<IInvokableInteraction>();

        private readonly static Dictionary<string, PropertyType> requiredProperties = new Dictionary<string, PropertyType>()
        {
            { "Name", PropertyType.String },
            { "WeightInKG", PropertyType.Decimal}
        };

        private readonly static Dictionary<string, PropertyType> optionalProperties = new Dictionary<string, PropertyType>()
        {

        };

        private readonly static Dictionary<string, object> defaultProperties = new Dictionary<string, object>()
        {
            {"Name", "Default Generic GameObject Name Test"},
            {"Weight", 1.0m},
        };

        static GameObject()
        {
            //Define new required and optional ValidProperties for this class
            DeclareNewPropertyTypes(requiredProperties);
            DeclareNewPropertyTypes(optionalProperties);
        }

        private protected GameObject(PropertyHandler? derivedProperties = null) 
            : base(SetLocalProperties(derivedProperties ??= new PropertyHandler()))
        {
            //Validate required properties have been set on entityProperties
            ValidateRequiredProperties(requiredProperties);
        }
        private static PropertyHandler SetLocalProperties(PropertyHandler derivedProperties)
        {
            //Apply default properties for this class to the current list of derivedProperties
            ApplyDefaultProperties(derivedProperties, defaultProperties);

            return derivedProperties; //Return is required to give (base) the derived list.
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
        public HashSet<GameObject> AttachedObjects { get; private protected set; } = [];
    }
}
