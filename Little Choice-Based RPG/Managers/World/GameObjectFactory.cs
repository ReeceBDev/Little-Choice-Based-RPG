﻿using Little_Choice_Based_RPG.Resources;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Systems.SystemEventBus;
using Little_Choice_Based_RPG.Types.EntityProperties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Managers.World
{
    /// <summary> Manufactures GameObjects of any type based on its properties. Gets allocated an outline of each object from JSONs. </summary>
    internal class GameObjectFactory
    {
        public IPropertyContainer NewGameObject(Dictionary<string, object> properties)
        {
            if (!properties.ContainsKey("Type"))
                throw new ArgumentException("The properties of the new object did not include the Type property! Properties: {properties}");

            Type objectType = Type.GetType(properties["Type"].ToString());

            object newGameObject = Activator.CreateInstance(objectType);

            IPropertyContainer castedGameObject = (IPropertyContainer)newGameObject;

            //Add components
            foreach (EntityProperty property in castedGameObject.Properties.EntityProperties)
            {
                if (!(property.PropertyName.StartsWith("Component.")))
                    continue;

                if (!(property.PropertyValue.Equals(true)))
                    continue; 

                Regex grabComponentName = new Regex("(?<=Component.)[A-Za-z]*");
                string systemReferenceName = grabComponentName.Match(property.PropertyName).Value;

                SystemSubscriptionEventBus.Subscribe(castedGameObject, systemReferenceName);
            }    

            return castedGameObject;
        }
    }
}
