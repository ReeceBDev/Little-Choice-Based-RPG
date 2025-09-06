using Little_Choice_Based_RPG.Types.TypedEventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.PropertySystem.Properties
{
    /// <summary> A suite of property wrappers designed specifically for use in components. Outside the components these should look like plain values, with a little bit of extra logic, i.e. events.
    /// Because they are represented by their property references and not the internal values themselves, the logic for these is tightly coupled with the components' PropertyHandler. </summary>
    internal interface ITransparent : IProperty
    {
        /// <summary> Declares whether a property should be modifiable and deletable. </summary>
        public bool IsReadOnly { get; }

        /// <summary> Fire the OnPropertyChanged event. This is a single point of origin for PropertyChanged events, publicly exposed. 
        /// This enables properties to update themselves externally, such as if they are replacing an existing properties reference within a component. 
        /// 
        /// May optionally be marked as becoming null, in-case the property was just straight up set to null. This allows old subscribers to still be notified. </summary>
        public void InvokePropertyChanged(bool isBecomingNull = false);

        /// <summary> Indicates that this property should not be modified. </summary>
        public void Freeze();

        /// <summary>  Indicates that this property should be modifiable. </summary>
        public void Thaw();
    }
}
