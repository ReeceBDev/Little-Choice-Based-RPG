using Little_Choice_Based_RPG.Types.PropertySystem.Archive;
using Little_Choice_Based_RPG.Types.PropertySystem.Entities;
using Little_Choice_Based_RPG.Types.PropertySystem.Systems;
using Little_Choice_Based_RPG.Types.TypedEventArgs.PropertyContainerEventArgs;

namespace Little_Choice_Based_RPG.Resources.Systems.InformationalSystems.Descriptor
{
    internal sealed class DescriptorSystem : PropertySystem
    {
        public override void InitialiseNewSubscriber(IPropertyContainer targetContainer, PropertyStore sourceProperties)
        {
            //Freeze default descriptors.
            sourceProperties.FreezeProperty("Descriptor.Generic.Default");
            sourceProperties.FreezeProperty("Descriptor.Inspect.Default");

            //If a descriptor is Additive, check that it has its associated additive modifiers: Prefix, Infix, Suffix and Empty.
            foreach (var property in targetContainer.Properties.EntityProperties)
                if (property.PropertyName.StartsWith("Descriptor.") && property.PropertyName.EndsWith(".IsAdditive") && property.PropertyValue.Equals(true))
                {
                    //Grab the descriptors inner identity
                    string descriptorName =
                        property.PropertyName.Remove(property.PropertyName.Length - ".IsAdditive".Length);

                    //Check it has its associated additive modifiers
                    if (!targetContainer.Properties.HasExistingPropertyName($"{descriptorName}.Additive.Prefix"))
                        throw new Exception($"This descriptors parent {targetContainer} didn't contain a reference for \"{descriptorName}.Prefix\" in its properties list {targetContainer.Properties}!");

                    if (!targetContainer.Properties.HasExistingPropertyName($"{descriptorName}.Additive.Infix"))
                        throw new Exception($"This descriptors parent {targetContainer} didn't contain a reference for \"{descriptorName}.Infix\" in its properties list {targetContainer.Properties}!");

                    if (!targetContainer.Properties.HasExistingPropertyName($"{descriptorName}.Additive.Suffix"))
                        throw new Exception($"This descriptors parent {targetContainer} didn't contain a reference for \"{descriptorName}.Suffix\" in its properties list {targetContainer.Properties}!");

                    if (!targetContainer.Properties.HasExistingPropertyName($"{descriptorName}.Additive.Empty"))
                        throw new Exception($"This descriptors parent {targetContainer} didn't contain a reference for \"{descriptorName}.Suffix\" in its properties list {targetContainer.Properties}!");
                }
        }

        protected override void OnObjectChanged(object sender, ObjectChangedEventArgs propertyChangedData)
        {
            //Empty at the moment for this system.
        }
    }
}
