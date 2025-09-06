using Little_Choice_Based_RPG.Types.PropertySystem.Properties;
using Little_Choice_Based_RPG.Types.PropertySystem.Systems;
using Little_Choice_Based_RPG.Types.TypedEventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.PropertySystem.Components
{
    /// <summary> Handles the properties held within components, allowing their events to be triggered even when the property reference has changed (i.e. property -> null).
    /// Strongly coupled and responsible for the "transparent" workings of TransparentProperties, which are property wrappers designed specifically for use in components. </summary>
    internal static class PropertyHandler
    {
        /// <summary> Updates a component's reference to a property with a fresh value, then tells it to mark itself as having changed.
        /// Must be used within a property setter using the field and value keywords. Field must be passed in as a reference so that it may be modified by this method. </summary>
        public static void ReplaceTransparentProperty<T>(ref TransparentProperty<T>? oldField, TransparentProperty<T>? newValue,
            [CallerArgumentExpression(nameof(oldField))] string propertyFieldExpression = "UNSET DEFAULT ERROR",
            [CallerArgumentExpression(nameof(newValue))] string newValueExpression = "UNSET DEFAULT ERROR")
        {
            //Argument expresssions guard clauses
            if (propertyFieldExpression != "field")
                throw new Exception($"The propertyField argument should have been the keywords \"ref field\", but instead it was \"{propertyFieldExpression}\"!" +
                    $"\nWere the '(ref field, value)' arguments in the correct order, or were they swapped?");

            if (newValueExpression != "value")
                throw new Exception($"The newValue argument should have been the keyword \"value\", but instead it was \"{newValueExpression}\"!" +
                    $"\nWere the '(ref field, value)' arguments in the correct order, or were they swapped?");

            InitialiseNewTransparent(ref oldField, newValue);
        }

        /// <summary> Updates a component's reference to a property with a fresh value, then tells it to mark itself as having changed.
        /// Must be used within a property setter using the field and value keywords. Field must be passed in as a reference so that it may be modified by this method. </summary>
        public static void ReplaceSystemReference(ref TransparentSystemReference? oldField, TransparentSystemReference? newValue, IPropertySystem targetSystem,
            [CallerArgumentExpression(nameof(oldField))] string propertyFieldExpression = "UNSET DEFAULT ERROR",
            [CallerArgumentExpression(nameof(newValue))] string newValueExpression = "UNSET DEFAULT ERROR")
        {
            //Argument expresssions guard clauses
            if (propertyFieldExpression != "field")
                throw new Exception($"The propertyField argument should have been the keywords \"ref field\", but instead it was \"{propertyFieldExpression}\"!" +
                    $"\nWere the '(ref field, value)' arguments in the correct order, or were they swapped?");

            if (newValueExpression != "value")
                throw new Exception($"The newValue argument should have been the keyword \"value\", but instead it was \"{newValueExpression}\"!" +
                    $"\nWere the '(ref field, value)' arguments in the correct order, or were they swapped?");

            //Inject the IPropertySystem into the new value, if it supports it.
            if (newValue is not null)
               newValue.SystemReference = targetSystem;
                   
            InitialiseNewTransparent(ref oldField, newValue);
        }

        private static void InitialiseNewTransparent<T>(ref T? oldField, T? newValue) where T : ITransparent
        {
            //Frozen guard clause
            if (oldField is not null && oldField.IsReadOnly == true)
                throw new Exception($"Tried to modify the reference of a frozen property. The original properties IsReadOnly value was true! Frozen property reference: {oldField}");
            //Note: Readonly doesnt need to be replicated at all as it will be impossible to create a new instance from one which is frozen.

            //Handle PropertyChanged subscriptions
            if (oldField is not null)
            {
                //If a new value is incoming, transition over the old subscribers first
                if (newValue is not null)
                {
                    foreach (var subscriber in oldField.PropertyChangedSubscribers)
                    {
                        SubscribeToPropertyInstance(newValue, subscriber.Value); //Subscribe to the new instance's event, first

                        UnsubscribeFromPropertyInstance(oldField, subscriber.Value); //Unsubscribe from the old instance's event, second
                    }

                    //Tell the property to mark itself as updated.
                    newValue.InvokePropertyChanged();
                }

                //Handle when becoming null, instead.
                if (newValue is null)
                    oldField.InvokePropertyChanged(true); //Let the old subscribers know.
            }

            //Update the component's reference (note: not the property itself, it has already been made and its value assigned!)
            oldField = newValue;
        }

        private static void SubscribeToPropertyInstance(IProperty eventPublisher, EventHandler<IPropertyChangedEventArgs> targetMethod)
            => eventPublisher.PropertyChanged += targetMethod;

        private static void UnsubscribeFromPropertyInstance(IProperty eventPublisher, EventHandler<IPropertyChangedEventArgs> targetMethod)
            => eventPublisher.PropertyChanged -= targetMethod;
    }
}