using LCBRPG_User_Console.Types.DisplayDataEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCBRPG_User_Console.Types.ActualElements
{
    internal abstract class ElementLogic : IElement
    {
        private const string emptyElementError = "\n\n ERROR: Missing element body. \n Please report this as a bug! \n";
        private string contentCache;

        public ElementIdentities ElementIdentity { get; init; }
        
        public ElementLogic(ElementIdentities setElementIdentity)
        {
            ElementIdentity = (setElementIdentity != default) ? 
                setElementIdentity : throw new ArgumentNullException("setElementIdentity was default! The ElementIdentity property must be set to a non-default value by each derived class.");

            //Initalise content for the first time
            RefreshContent();
        }

        public void RefreshContent() => contentCache = GenerateContent();
        public string GetCachedContent() => contentCache ?? emptyElementError;
        protected abstract string GenerateContent();
        protected virtual void OnElementUpdating()
        {
            ElementUpdating?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnElementUpdated()
        {
            ElementUpdated?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler ElementUpdated;
        public event EventHandler ElementUpdating; 
    }
}
