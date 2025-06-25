using Little_Choice_Based_RPG.Resources.Entities;
using Little_Choice_Based_RPG.Resources.Systems.SystemEventBus;
using System.Text.RegularExpressions;

namespace Little_Choice_Based_RPG.Managers.World
{
    /// <summary> Manufactures PropertyContainers of any type based on its properties. Gets allocated an outline of each object from JSONs. </summary>
    public static class PropertyContainerFactory
    {
        public static PropertyContainer New(Dictionary<string, object> properties)
        {
            PropertyContainer newGameObject = GenerateFromTypeProperty(properties);

            //Subscribe to components
            SubscribeToComponents(newGameObject);

            //Notify the creaton of a new PropertyContainer.
            NewPropertyContainer?.Invoke(null, newGameObject);

            return newGameObject;
        }

        private static PropertyContainer GenerateFromTypeProperty(Dictionary<string, object> properties)
        {
            if (!properties.ContainsKey("Type"))
                throw new ArgumentException("The properties of the new object did not include the Type property! Properties: {properties}");

            Type objectType = Type.GetType(properties["Type"].ToString());
            object newGameObject = Activator.CreateInstance(objectType, properties);
            PropertyContainer castedGameObject = (PropertyContainer)newGameObject;

            return castedGameObject;
        }

        private static void SubscribeToComponents(PropertyContainer targetGameObject)
        {
            foreach (var property in targetGameObject.Properties.EntityProperties.ToList())
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

        public static event EventHandler<PropertyContainer> NewPropertyContainer;
    }
}
