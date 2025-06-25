using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory;
using Little_Choice_Based_RPG.Types.EntityProperties;

namespace Little_Choice_Based_RPG.Resources.Entities.Conceptual
{
    public abstract class GameObject : PropertyContainer
    {
        private readonly static Dictionary<string, PropertyType> requiredProperties = new Dictionary<string, PropertyType>()
        {
            {"WeightInKG", PropertyType.Decimal}
        };

        private readonly static Dictionary<string, PropertyType> optionalProperties = new Dictionary<string, PropertyType>()
        {

        };

        private readonly static Dictionary<string, object> defaultProperties = new Dictionary<string, object>()
        {
        };

        static GameObject()
        {
            //Define new required and optional ValidProperties for this class
            DeclareNewPropertyTypes(requiredProperties);
            DeclareNewPropertyTypes(optionalProperties);
        }

        private protected GameObject(Dictionary<string, object>? derivedProperties = null)
            : base(SetLocalProperties(derivedProperties ??= new Dictionary<string, object>()))
        {
            //Validate required properties have been set on entityProperties
            ValidateRequiredProperties(requiredProperties);

            //Validate that pickup and putdown descriptors have been set if it falls under the current max-strength.
            InventoryPropertyValidation.ValidateInventoryDescriptors(this);
        }

        private static Dictionary<string, object> SetLocalProperties(Dictionary<string, object> derivedProperties)
        {
            //Apply default properties for this class to the current list of derivedProperties
            ApplyDefaultProperties(derivedProperties, defaultProperties);

            return derivedProperties; //Return is required to give (base) the derived list.
        }

        /*
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
        */
    }
}
