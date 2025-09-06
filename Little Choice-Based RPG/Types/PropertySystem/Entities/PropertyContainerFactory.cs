using Little_Choice_Based_RPG.Types.PropertySystem.Components;
using Little_Choice_Based_RPG.Types.PropertySystem.Containers;
using Little_Choice_Based_RPG.Types.PropertySystem.Systems.SystemEventBus;
using System.Text.RegularExpressions;

namespace Little_Choice_Based_RPG.Types.PropertySystem.Entities
{
    /// <summary> Manufactures PropertyContainers of any type based on its properties. Gets allocated an outline of each object from JSONs. </summary>
    internal static class PropertyContainerFactory
    {
        public static IPropertyContainer New(List<Action<PropertyContainer>> properties, Type temporaryTypeForcedEntry)
        {
            IPropertyContainer newGameObject = GenerateFromTypeProperty(properties, temporaryTypeForcedEntry);

            //Subscribe to components
            SubscribeToComponents(newGameObject);

            //Notify the creation of a new PropertyContainer.
            NewPropertyContainer?.Invoke(null, newGameObject);

            return newGameObject;
        }

        private static IPropertyContainer GenerateFromTypeProperty(List<Action<PropertyContainer>> properties, Type temporaryTypeForcedEntry)
        {
            //Removing this until JSONs are the method for generating entities.
            /*
            if (!properties.ContainsKey("Type"))
                throw new ArgumentException("The properties of the new object did not include the Type property! Properties: {properties}");

            Type objectType = Type.GetType(properties["Type"].ToString()) ?? throw new Exception("Failed to get find the type from a set of raw properties. The type fetch returned null!"); ;
            */

            Type objectType = temporaryTypeForcedEntry;
            object newGameObject = Activator.CreateInstance(objectType, properties) ?? throw new Exception("Failed to create an instance from its type and properties. It ended up null!");
            IPropertyContainer castedGameObject = newGameObject as IPropertyContainer ?? throw new Exception("Failed to cast to IPropertyContainer from an entity instance. Returned null!");

            return castedGameObject;
        }

        private static void SubscribeToComponents(IPropertyContainer targetContainer)
        {
            /* OLD PROPERTY SYSTEM (V1)
            foreach (var property in targetContainer.Properties.EntityProperties.ToList())
            {
                if (!property.Value.ToString().StartsWith("Uses") && property.Value.ToString().EndsWith("System"))
                    continue;

                if (!property.Key.Equals(true))
                    continue;

                Regex grabComponentName = new Regex("(?<=Component.)[A-Za-z]*");
                string systemReferenceName = grabComponentName.Match(property.PropertyName).Value;

                SystemSubscriptionEventBus.Subscribe(targetContainer, systemReferenceName);
            }
            */

            //New property system (V2)
            foreach (IPropertyComponent? component in targetContainer.Properties.EntityProperties.Values)
            {
                if (component is ISystemComponent systemComponent)
                {
                    if (systemComponent.UsesSystem == true)
                        systemComponent.UsesSystem.SystemReference.InitialiseContainer(targetContainer);
                }
            }
        }

        public static event EventHandler<IPropertyContainer> NewPropertyContainer;
    }
}
