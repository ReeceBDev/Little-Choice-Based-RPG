using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Managers.World
{
    /// <summary> Manufactures GameObjects of any type based on its properties. Gets allocated an outline of each object from JSONs. </summary>
    internal class GameObjectFactory
    {
        public object NewGameObject(Dictionary<string, object> properties)
        {
            if (!properties.ContainsKey("Type"))
                throw new ArgumentException("The properties of the new object did not include the Type property! Properties: {properties}");

            Type objectType = Type.GetType(properties["Type"].ToString());

            object newGameObject = Activator.CreateInstance(objectType);

            return newGameObject;
        }
    }
}
