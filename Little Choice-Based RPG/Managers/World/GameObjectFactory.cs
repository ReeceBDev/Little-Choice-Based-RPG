using Little_Choice_Based_RPG.Resources;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Systems.SystemEventBus;
using Little_Choice_Based_RPG.Types.EntityProperties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Managers.World
{
    /// <summary> Manufactures GameObjects of any type based on its properties. Gets allocated an outline of each object from JSONs. </summary>
    public static class GameObjectFactory
    {
        public static GameObject NewGameObject(Dictionary<string, object> properties)
        {
            GameObject newGameObject = GenerateObjectFromTypeProperty(properties);
            SubscribeToComponents(newGameObject, properties);

            return newGameObject;
        }

        private static GameObject GenerateObjectFromTypeProperty(Dictionary<string, object> properties)
        {
            if (!properties.ContainsKey("Type"))
                throw new ArgumentException("The properties of the new object did not include the Type property! Properties: {properties}");

            Type objectType = Type.GetType(properties["Type"].ToString());
            object newGameObject = Activator.CreateInstance(objectType, properties);
            GameObject castedGameObject = (GameObject)newGameObject;

            return castedGameObject;
        }

        private static void SubscribeToComponents(GameObject targetGameObject, Dictionary<string, object> properties)
        {
            foreach (EntityProperty property in targetGameObject.Properties.EntityProperties)
            {
                if (!(property.PropertyName.StartsWith("Component.")))
                    continue;

                if (!(property.PropertyValue.Equals(true)))
                    continue;

                Regex grabComponentName = new Regex("(?<=Component.)[A-Za-z]*");
                string systemReferenceName = grabComponentName.Match(property.PropertyName).Value;

                SystemSubscriptionEventBus.Subscribe(targetGameObject, systemReferenceName);
            }
        }
    }
}
