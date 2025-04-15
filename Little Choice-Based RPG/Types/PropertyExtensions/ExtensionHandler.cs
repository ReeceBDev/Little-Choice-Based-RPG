using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.Extensions
{
    public class ExtensionHandler
    {
        private List<IExtension> localExtensions = new List<IExtension>();

        public bool ContainsExtension(string extension) => localExtensions.Any(i => i.UniqueIdentifier.Equals(extension));
        public void RemoveExtension(IExtension extension) => localExtensions.Remove(extension);
        public void AddExtension(IExtension extension)
        {
            if (localExtensions.Contains(extension))
                throw new ArgumentException($"This additional extension {extension} already exists in this event handlers' extensions {localExtensions}!");
        }
        public IExtension GetExtension(string extension)
        {
            IExtension? selectedExtension = localExtensions.Find(i => i.UniqueIdentifier.Equals(extension));

            if (selectedExtension == null)
                throw new ArgumentException($"The desired extension {extension} was not present in this ExtensionHandler!");

            return selectedExtension;
        }
    }
}
