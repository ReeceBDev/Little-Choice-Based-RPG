using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Systems.Damage.Repair;
using Little_Choice_Based_RPG.Types.EntityProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems.Damage.Break
{
    public class BreakSystem : IBreakable
    {
        private GameObject parentObject;

        static BreakSystem()
        {
            //BreakSystem logic
            PropertyValidation.CreateValidProperty("IsBreakable", PropertyType.Boolean); //Activates all this class and all of these properties when true :)
            PropertyValidation.CreateValidProperty("IsBreakableByChoice", PropertyType.Boolean); //Lets players choose to break it by choice. 

            //Descriptors
            PropertyValidation.CreateValidProperty("Descriptor.Breaking", PropertyType.String); //Describes how it looks when it is breaking.
            PropertyValidation.CreateValidProperty("Descriptor.Choice.Break.Interact", PropertyType.String); //Describes the interact option presented to the player.
            PropertyValidation.CreateValidProperty("Descriptor.Choice.Breaking", PropertyType.String); //Describes the action of breaking it when a player uses the Break() choice.
            PropertyValidation.CreateValidProperty("Descriptor.Generic.Broken", PropertyType.String); //Broken at a distance
            PropertyValidation.CreateValidProperty("Descriptor.Inspect.Broken", PropertyType.String); //A closer look
        }

        /// <summary> Allows objects to break. Requires DamageCommon. </summary>
        public BreakSystem(GameObject instantiatingObject)
        {
            //Enforces IRepairable on the instantiating class
            if (!(instantiatingObject is IBreakable))
                throw new Exception($"{instantiatingObject.GetType()} does not implement IBreakable!");

            DamageCommon damageCommonInstantisation = DamageCommon.Instance;

            //Stores instantiating object for accessing its PropertyHandler.
            parentObject = instantiatingObject;
        }

        /// <summary> Sets IsBroken to true. </summary>
        public void Break()
        {
            //Guard clauses for the values in use.
            if (!parentObject.entityProperties.HasProperty("IsBreakable", true))
                throw new Exception("This object is not breakable! Tried to break an object where there is no EntityProperty of IsBreakable = true.");

            if (!parentObject.entityProperties.HasExistingPropertyName("Descriptor.Breaking"))
                throw new Exception("This object has no break description! Tried to break an object where there is no EntityProperty of Descriptor.Breaking.");

            if (!parentObject.entityProperties.HasExistingPropertyName("Descriptor.Generic.Broken"))
                throw new Exception("This object has no broken description! Tried to break an object where there is no EntityProperty of Descriptor.Generic.Broken.");

            //Main breaking logic.
            parentObject.entityProperties.UpsertProperty("IsBroken", true); //Property found in DamageCommon

            //Set generic descriptor.
            parentObject.entityProperties.UpdateProperty("Descriptor.Generic.Current", parentObject.entityProperties.GetPropertyValue("Descriptor.Generic.Broken"));

            //Set inspect descriptor or default to generic.
            if (!parentObject.entityProperties.HasExistingPropertyName("Descriptor.Inspect.Broken"))
                parentObject.entityProperties.UpdateProperty("Descriptor.Inspect.Current", parentObject.entityProperties.GetPropertyValue("Descriptor.Inspect.Broken"));
            else
                parentObject.entityProperties.UpdateProperty("Descriptor.Inspect.Current", parentObject.entityProperties.GetPropertyValue("Descriptor.Generic.Broken"));
        }
    }
}
