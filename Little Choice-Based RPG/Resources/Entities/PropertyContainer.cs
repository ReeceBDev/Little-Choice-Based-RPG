using Little_Choice_Based_RPG.Resources.Components.ComponentAccessors;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PrivateInteractionsSystems;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using Little_Choice_Based_RPG.Types.PropertyExtensions;
using Little_Choice_Based_RPG.Types.PropertyExtensions.PropertyExtensionEventArgs;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using Little_Choice_Based_RPG.Types.PropertySystem.Properties;
using Little_Choice_Based_RPG.Types.TypedEventArgs.PropertyContainerEventArgs;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace Little_Choice_Based_RPG.Types.PropertySystem.Containers
{
    internal abstract class PropertyContainer : IPropertyContainer
    {
        protected static uint GlobalCounter;

        private static readonly List<Func<PropertyContainer, IProperty?>> requiredProperties = new()
        {
            { i => i.Identity.ID },
            { i => i.Identity.EntityType },
            { i => i.Identity.Name },
        };

        private static readonly List<Action<PropertyContainer>> defaultProperties = new()
        {
            { i => i.Descriptor.UsesSystem ??= true },
        };

        public PropertyStore Properties { get; set; } = new();
        public PropertyExtensionHandler Extensions { get; set; } = new();

        private protected PropertyContainer(List<Action<PropertyContainer>>? derivedProperties)
        {
            PrivateInteractions test = this.PrivateInteractions.PrivateInteractionsList.;

            test.UniqueIdentifier
            this.Identity.ID = GlobalCounter++; // Must be here, as the intialisers are saved per type, including the = new Dictionary! - Review: What's this mean??                 
            this.Identity.EntityType ??= GetType(); //Set the type property to reflect the classes type, if not already set.

            //Set the default properties if they haven't already been set
            ConcatenateProperties(derivedProperties, defaultProperties); //No assignment should be required as derivedProperties is a reference.

            //Store the final list of properties
            foreach (var propertySetter in derivedProperties)
                propertySetter.Invoke(this);

            //Validate required properties have been set on entityProperties
            ValidateRequiredProperties(requiredProperties);
        }

        private protected void ValidateRequiredProperties(List<Func<PropertyContainer, IProperty?>> requiredProperties)
        {
            foreach (var property in requiredProperties)
            {
                object output = property.Invoke(this)
                    ?? throw new Exception(
                        $"A required property has not been defined in the input properties of a prospective {GetType().Name.ToString()}" +
                        $"\n\nMissing Property details: {property}" +
                        $"\n\nPropertyHandler: {Properties}"
                    );
            }
        }

        /// <summary> Apply default properties for this class to the current list of derivedProperties. Returns the first value. </summary>
        private protected static List<Action<PropertyContainer>> ConcatenateProperties(List<Action<PropertyContainer>>? derivedProperties,
            List<Action<PropertyContainer>> defaultProperties)
        {
            derivedProperties ??= new();
            derivedProperties.AddRange(defaultProperties);

            return derivedProperties; //Return is required to give (base) the derived list when performing this method in a : base() constructor call.
        }

        /* -- I don't want these to be a thing any more since moving to the V2 Property System. Properties should instead be subscribed to directly!
        //Extensions
        //Subscribe to extension add and remove events, for the local ExtensionHandler
        Extensions.ExtensionAdded += OnExtensionAdded;
        Extensions.ExtensionRemoved += OnExtensionRemoved;

        //Properties
        //Subscribe to property change events, for the local PropertyHandler
        Properties.PropertyChanged += OnPropertyChanged;

        protected virtual void OnExtensionAdded(object sender, IPropertyExtension subject) => subject.PropertyExtensionChanged += OnExtensionChanged;        

        protected virtual void OnExtensionRemoved(object sender, IPropertyExtension subject) => subject.PropertyExtensionChanged -= OnExtensionChanged;

        protected virtual void OnExtensionChanged(object sender, PropertyExtensionChangedArgs args)
            => ContainerChanged?.Invoke(sender, new ObjectChangedEventArgs(this, args.UniqueIdentifier, args.SubjectChanged));

        protected virtual void OnPropertyChanged(object sender, EntityProperty args) 
            => ContainerChanged?.Invoke(sender, new ObjectChangedEventArgs(this, args.PropertyName, args.PropertyValue));
        */

        /*
        private protected static void ApplyDefaultProperties(Dictionary<string, object> derivedProperties, Dictionary<string, object> defaultProperties)
        {
            foreach (KeyValuePair<string, object> property in defaultProperties)
                if (!derivedProperties.ContainsKey(property.Key))
                    derivedProperties.Add(property.Key, property.Value);
        }
        */


        /*
        //Apply default properties for this class to the current list of derivedProperties
        ApplyDefaultProperties(derivedProperties, defaultProperties);

        
        private protected void SetDefaultProperties(List<Action<PropertyContainer>> defaultProperties)
        {
            foreach (var property in defaultProperties)
                property.Invoke(this);
        }
        */
    }
}
