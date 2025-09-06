using Little_Choice_Based_RPG.Types.PropertySystem.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types.PropertySystem.Entities
{
    internal class PropertyStore
    {
        private readonly Dictionary<Type, IPropertyComponent> propertyComponents = new();

        public ReadOnlyDictionary<Type, IPropertyComponent> EntityProperties { get => propertyComponents.AsReadOnly(); }

        public void AddComponent<T>() where T : notnull, IPropertyComponent, new() => AddComponent(new T());

        public void AddComponent<T>(T newComponent) where T : notnull, IPropertyComponent
        {
            if (propertyComponents.ContainsKey(typeof(T)))
                return;

            propertyComponents.Add(typeof(T), newComponent);

        }

        /// <summary> Defaults to success, if the target did not exist. Always succeeds. </summary>
        public void Remove<T>() where T : notnull, IPropertyComponent => propertyComponents.Remove(typeof(T));

        /// <summary> Gets the requested component or creates it if it didn't already exist. Always succeeds. </summary>
        public T? Get<T>() where T : notnull, IPropertyComponent, new()
        {
            bool tryGet = propertyComponents.TryGetValue(typeof(T), out var output);

            //Create a new one if it was missing.
            if (!tryGet)
            {
                AddComponent(new T());
                tryGet = propertyComponents.TryGetValue(typeof(T), out output);

                if (!tryGet)
                    throw new Exception("Failed to get a component which should have just been added, after it was initially missing! That should be impossible!");
            }

            if (output is null)
                throw new Exception("Output is null. That shouldnt have happened!");

            return (T)output;
        }
    }
}
