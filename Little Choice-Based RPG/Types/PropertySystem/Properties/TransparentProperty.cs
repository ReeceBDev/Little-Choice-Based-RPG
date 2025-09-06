using Little_Choice_Based_RPG.Types.TypedEventArgs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.PropertySystem.Properties
{
    /// <summary> A transparent wrapper to enable properties to run their own OnPropertyChanged events. Exposes itself as if it were a plain, assignable value. 
    /// For example, can be used directly with x = y; - in other words, transparent! See the ITransparent description for more details. </summary>
    internal class TransparentProperty<T> : ITransparent where T : notnull
    {
        /// <summary> The value of the property. This is accessed through implicit conversion to the type of T. </summary>
        private readonly T? value;

        private Dictionary<object, EventHandler<IPropertyChangedEventArgs>> subscribers = []; //Subscriber, Assigned method


        /// <summary> Get a list of all OnPropertyChanged event subscribers so that they may be replicated to other 
        public IReadOnlyDictionary<object, EventHandler<IPropertyChangedEventArgs>> PropertyChangedSubscribers => subscribers;

        /// <summary> Declares if a property is modifiable and deletable. </summary>
        public bool IsReadOnly { get; private set; } = false;


        /// <summary> Creates a freshly wrapped instance from a T value. </summary>
        public TransparentProperty(T? setValue)
        {
            //Normal collection guard clause.
            if (setValue is ICollection && setValue is not ITransparentCollection)
                throw new Exception("Tried to initialise a transparent property with its generic type set to an ordinary collection." +
                    "You must instead use an ITransparentCollection." +
                    "This must enforce external firing of the OnPropertyChanged even (here in this instance) due to the modification of the reference.");

            value = setValue;
            //Doesn't notify as changed as there are no subscribers held on a fresh instance.
        }

        /// <summary> Fire the OnPropertyChanged event. This is a single point of origin for PropertyChanged events, publicly exposed. 
        /// This enables properties to update themselves externally, such as if they are replacing an existing properties reference within a component. 
        /// 
        /// May optionally be marked as becoming null, in-case the property was just straight up set to null. This allows old subscribers to still be notified. </summary>
        public void InvokePropertyChanged(bool isBecomingNull = false)
        {
            object? reportedValue = value;

            if (isBecomingNull == true)
                reportedValue = null;

            propertyChangedBacking?.Invoke(this, new IPropertyChangedEventArgs(typeof(T).Name.ToString(), reportedValue));
        }

        /// <summary> Indicates that this property should not be modified. </summary>
        public void Freeze() => IsReadOnly = true;

        /// <summary>  Indicates that this property should be modifiable. </summary>
        public void Thaw() => IsReadOnly = false;


        /// <summary> Get the value of a property implictly. Implicitly casts a TransparentProperty<> to a value of the same T type. </summary>
        public static implicit operator T?(TransparentProperty<T> target) => target.value;

        /// <summary> Set the value of a new property implictly. Implicitly casts the value of a type T to a new TransparentProperty. Ideally should target the same reference. </summary>
        public static implicit operator TransparentProperty<T>(T target) => new(target);


        /// <summary> Subscribe to events that should indicate the property has been altered. </summary>
        public event EventHandler<IPropertyChangedEventArgs>? PropertyChanged
        {
            add
            {
                var subscriber = value?.Target ?? throw new Exception("The subscribing object must not be null!");
                var methodReference = value as EventHandler<IPropertyChangedEventArgs> ?? throw new Exception("The assigned method must be an EventHandler<PropertyChangedEventArgs>!");

                subscribers.Add(subscriber, methodReference); //Record as a subscriber first.
                propertyChangedBacking += value; //Then actually subscribe.
            }
            remove
            {
                var subscriber = value?.Target ?? throw new Exception("The subscribing object must not be null!");

                subscribers.Remove(subscriber); //Perform First
                propertyChangedBacking -= value; //Perform Second
            }
        }

        private event EventHandler<IPropertyChangedEventArgs>? propertyChangedBacking;
    }
}
