﻿using Little_Choice_Based_RPG.Types.PropertyExtensions.PropertyExtensionEventArgs;

namespace Little_Choice_Based_RPG.Types.PropertyExtensions
{
    public interface IPropertyExtension
    {
        public string UniqueIdentifier { get; init; }
        public event EventHandler<PropertyExtensionChangedArgs> PropertyExtensionChanged;
    }
}
