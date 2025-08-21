using Little_Choice_Based_RPG.Types.EntityProperties;

namespace Little_Choice_Based_RPG.Types.PropertyExtensions
{
    internal class PropertyExtensionHandler
    {
        private List<IPropertyExtension> localExtensions = new List<IPropertyExtension>();
        public IEnumerable<IPropertyExtension> EntityExtensions => localExtensions;

        public bool Contains(string extension) => localExtensions.Any(i => i.UniqueIdentifier.Equals(extension));

        public void RemoveExtension(IPropertyExtension extension)
        {
            localExtensions.Remove(extension);

            ExtensionRemoved?.Invoke(this, extension);
        }

        public void AddExtension(IPropertyExtension extension)
        {
            if (localExtensions.Contains(extension))
                throw new ArgumentException($"This additional extension {extension} already exists in this event handlers' extensions {localExtensions}!");

            localExtensions.Add(extension);

            ExtensionAdded?.Invoke(this, extension);
        }

        public IPropertyExtension Get(string extension)
        {
            IPropertyExtension? selectedExtension = localExtensions.Find(i => i.UniqueIdentifier.Equals(extension));

            if (selectedExtension == null)
                throw new ArgumentException($"The desired extension {extension} was not present in this ExtensionHandler!");

            return selectedExtension;
        }

        public event EventHandler<IPropertyExtension> ExtensionAdded;
        public event EventHandler<IPropertyExtension> ExtensionRemoved;
    }
}
