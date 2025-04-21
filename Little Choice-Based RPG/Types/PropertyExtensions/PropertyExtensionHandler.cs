using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.PropertyExtensions
{
    public class PropertyExtensionHandler
    {
        private List<IPropertyExtension> localExtensions = new List<IPropertyExtension>();

        public bool ContainsExtension(string extension) => localExtensions.Any(i => i.UniqueIdentifier.Equals(extension));
        public void RemoveExtension(IPropertyExtension extension)
        {
            localExtensions.Remove(extension);

            ExtensionRemoved?.Invoke(this, extension);
        }
        public void AddExtension(IPropertyExtension extension)
        {
            if (localExtensions.Contains(extension))
                throw new ArgumentException($"This additional extension {extension} already exists in this event handlers' extensions {localExtensions}!");

            ExtensionAdded?.Invoke(this, extension);
        }
        public IPropertyExtension GetExtension(string extension)
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
