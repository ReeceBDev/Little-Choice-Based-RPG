using Little_Choice_Based_RPG.Types.EntityProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems.Damage.Break
{
    internal class BreakSystem : IBreakable
    {
        static BreakSystem()
        {
            //BreakSystem logic
            PropertyValidation.CreateValidProperty("IsBreakable", PropertyType.Boolean); //Activates all this class and all of these properties when true :)
            PropertyValidation.CreateValidProperty("IsBreakableByChoice", PropertyType.Boolean); //Lets players choose to break it by choice.
            PropertyValidation.CreateValidProperty("IsBroken", PropertyType.Boolean); 

            //Descriptors
            PropertyValidation.CreateValidProperty("Descriptor.Breaking", PropertyType.String); //Describes how it looks when it breaks.
            PropertyValidation.CreateValidProperty("Descriptor.Generic.Broken", PropertyType.String); //Broken at a distance
            PropertyValidation.CreateValidProperty("Descriptor.Inspect.Broken", PropertyType.String); //A closer look
            PropertyValidation.CreateValidProperty("Descriptor.Choice.Break", PropertyType.String); //Describes the action of breaking it when a player uses the Break() choice.
        }

        public void Break(PropertyHandler sourcePropertyHandler)
        {
            if (!sourcePropertyHandler.HasPropertyAndValue("IsBreakable", true))
                throw new Exception("This object is not breakable! Tried to break an object where there is no EntityProperty of IsBreakable = true.");

            if (!sourcePropertyHandler.HasProperty("Descriptor.Breaking"))
                throw new Exception("This object has no break description! Tried to break an object where there is no EntityProperty of Descriptor.Breaking.");

            if (!sourcePropertyHandler.HasProperty("Descriptor.Generic.Broken") | !sourcePropertyHandler.HasProperty("Descriptor.Inspect.Broken"))
                throw new Exception("This object has no broken description! Tried to break an object where there is no EntityProperty of Descriptor.Generic.Broken or Descriptor.Generic.Inspect.");

            sourcePropertyHandler.UpsertProperty("IsBroken", true);

        }
    }
}
