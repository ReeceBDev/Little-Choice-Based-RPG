using Little_Choice_Based_RPG.Types.TypedEventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.PropertySystem.Properties
{
    internal interface IProperty
    {
        public IReadOnlyDictionary<object, EventHandler<IPropertyChangedEventArgs>> PropertyChangedSubscribers { get; }
        public event EventHandler<IPropertyChangedEventArgs>? PropertyChanged;
    }
}
