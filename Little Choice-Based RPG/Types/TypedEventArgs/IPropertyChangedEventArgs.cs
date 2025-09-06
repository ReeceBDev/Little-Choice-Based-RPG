using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.TypedEventArgs
{
    internal class IPropertyChangedEventArgs(string propertyName, object? propertyValue) : EventArgs
    {
        public string PropertyName { get; init; } = propertyName;
        public object? PropertyValue { get; init; } = propertyValue;
    }
}
