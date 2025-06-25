using Little_Choice_Based_RPG.Resources.PropertyContainerEventArgs;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.Interactions.InteractionDelegates;
using Little_Choice_Based_RPG.Types.PropertyExtensions;
using Little_Choice_Based_RPG.Types.PropertyExtensions.PropertyExtensionEventArgs;
using System.Collections.Concurrent;

namespace Little_Choice_Based_RPG.Resources.Entities
{
    public abstract class PropertyContainer : IPropertyContainer
    {
        public static uint globalCounter;

        private readonly static Dictionary<string, PropertyType> requiredProperties = new Dictionary<string, PropertyType>()
        {
            { "ID", PropertyType.UInt32 },
            {"Type", PropertyType.String},
            {"Name", PropertyType.String},
        };

        private readonly static Dictionary<string, PropertyType> optionalProperties = new Dictionary<string, PropertyType>()
        {

        };

        private Dictionary<string, object> defaultProperties = new Dictionary<string, object>()
        {
            {"Component.DescriptorSystem", true }
        };


        public PropertyHandler Properties { get; set; } = new PropertyHandler();
        public PropertyExtensionHandler Extensions { get; set; } = new PropertyExtensionHandler();
        
        public event EventHandler<ObjectChangedEventArgs> ObjectChanged;

        static PropertyContainer()
        {
            //Define new required and optional ValidProperties for this class
            DeclareNewPropertyTypes(requiredProperties);
            DeclareNewPropertyTypes(optionalProperties);
        }

        private protected PropertyContainer(Dictionary<string, object> derivedProperties)
        {
            Properties.CreateProperty("ID", globalCounter++); // Must be here, as the intialisers are saved per type, inculding the = new Dictionary!

            //Extensions
            //Subscribe to extension add and remove events, for the local ExtensionHandler
            Extensions.ExtensionAdded += OnExtensionAdded;
            Extensions.ExtensionRemoved += OnExtensionRemoved;

            //Properties
            //Subscribe to property change events, for the local PropertyHandler
            Properties.PropertyChanged += OnPropertyChanged;

            //Apply default properties for this class to the current list of derivedProperties
            ApplyDefaultProperties(derivedProperties, defaultProperties);

            //Store the final list of properties
            foreach (KeyValuePair<string, object> property in derivedProperties)
                Properties.CreateProperty(property.Key, property.Value);

            //Set the type property to reflect the classes type.
            Properties.UpsertProperty("Type", GetType().ToString());

            //Validate required properties have been set on entityProperties
            ValidateRequiredProperties(requiredProperties);
        }

        private protected static void DeclareNewPropertyTypes(Dictionary<string, PropertyType> newProperties)
        {
            //Define required ValidProperties
            foreach (var property in newProperties)
                PropertyValidation.CreateValidProperty(property.Key, property.Value);
        }

        private protected static void ApplyDefaultProperties(Dictionary<string, object> derivedProperties, Dictionary<string, object> defaultProperties)
        {
            foreach (KeyValuePair<string, object> property in defaultProperties)
                if (!derivedProperties.ContainsKey(property.Key))
                    derivedProperties.Add(property.Key, property.Value);
        }

        private protected void ValidateRequiredProperties(Dictionary<string, PropertyType> testRequiredProperties)
        {
            foreach (var property in testRequiredProperties)
                if (!Properties.HasExistingPropertyName(property.Key))
                    throw new Exception($"A required property has not been defined in the PropertyHandler of a prospective {GetType().Name.ToString()}. \n\nMissing Property details: \nName: {property.Key}, \nValue: {property.Value}. \n\n{GetType().Name.ToString()} details: \nPropertyContainer: {this}, \nType: {GetType()}, \nName: {(Properties.HasExistingPropertyName("Name") ? Properties.GetPropertyValue("Name") : "Null - Name not set.")}, \nPropertyHandler: {Properties}");
        }

        protected virtual void OnExtensionAdded(object sender, IPropertyExtension subject) => subject.PropertyExtensionChanged += OnExtensionChanged;        

        protected virtual void OnExtensionRemoved(object sender, IPropertyExtension subject) => subject.PropertyExtensionChanged -= OnExtensionChanged;

        protected virtual void OnExtensionChanged(object sender, PropertyExtensionChangedArgs args)
            => ObjectChanged?.Invoke(sender, new ObjectChangedEventArgs(this, args.UniqueIdentifier, args.SubjectChanged));

        protected virtual void OnPropertyChanged(object sender, EntityProperty args) 
            => ObjectChanged?.Invoke(sender, new ObjectChangedEventArgs(this, args.PropertyName, args.PropertyValue));
    }
}
