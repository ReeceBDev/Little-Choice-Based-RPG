using Little_Choice_Based_RPG.Resources.PropertyContainerEventArgs;
using Little_Choice_Based_RPG.Types.EntityProperties;

namespace Little_Choice_Based_RPG.Resources.Systems
{
    public interface IPropertyLogic
    {
        /// <summary> Provide logic for co-ordinating property changes with their relevant methods. </summary>
        public static abstract void OnObjectChanged(object sender, ObjectChangedEventArgs propertyChangedData);

        /// <summary> Apply starting implementation to a subscription. This may include handling the initial interactions based on its initial property state. </summary>
        public static abstract void InitialiseNewSubscriber(PropertyHandler sourceProperties);
    }
}
