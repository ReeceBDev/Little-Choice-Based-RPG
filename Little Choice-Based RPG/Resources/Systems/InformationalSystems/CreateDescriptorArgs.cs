using Little_Choice_Based_RPG.Types;
using Little_Choice_Based_RPG.Types.EntityProperties;

namespace Little_Choice_Based_RPG.Resources.Systems.InformationalSystems
{
    public class CreateDescriptorStateList(uint setPriority)
    {
        public CreateDescriptorStateList(string descriptorIdentifier, string descriptorValue, uint priority = 6) 
            : this (priority)
        {
            if (!PropertyValidation.IsValidPropertyName(descriptorIdentifier))
                throw new ArgumentException($"The descriptor identifier, {descriptorIdentifier}, didn't match a valid property in PropertyValidation.");

            Descriptor = new EntityProperty(descriptorIdentifier, descriptorValue);
        }

        public void AddEntityCondition(string? propertyName, object? propertyValue, uint entityID)
        {
            //Throw an exception if only the propertyName or propertyValue is null
            if (propertyName is null != propertyValue is null)
                throw new ArgumentNullException($"One, but not both, of the new EntityProperty values are null! That makes no sense! Either make both null, or neither. Both values being null at the same time is okay, as it can be used to represent an entity which exists without considering its state. The values were {propertyName}, {propertyValue} (Name, value.)");


            EntityState? prospectiveState = null;

            //Check if the new condition should be added to an entityID which already exists in EntityStates
            foreach (EntityState state in EntityStates)
                if (state.EntityReferenceID == entityID)
                {
                    //Create a new state which will replace the old one
                    prospectiveState = state;
                    //Remove the old state from the list
                    EntityStates.Remove(state);
                }


            //Throw an exception if the existingState existed but already contains null
            if (prospectiveState is not null && prospectiveState.Value.RequiredProperties is null)
                throw new Exception($"Tried to add a new condition to an existing state which was already set as null. This makes no sense! Why use a null value to represent an entity that exists without considering its state, if you will just add a stateful condition for the same entity! When you use a stateful condition, the existence of that entity is implicit! Offending state: {prospectiveState.Value}. New values: ID: {entityID}, Name: {propertyName}, Value: {propertyValue}.");


            if (prospectiveState is null) //Create a new existing state, if it doesn't exist
            {
                if (propertyName is null)
                    EntityStates.Add(new EntityState(entityID, null));
                else
                    EntityStates.Add(new EntityState(entityID, new List<EntityProperty>() 
                        { new EntityProperty(propertyName, propertyValue) }));
            }
            else if (propertyName is not null) //Add the new condition to the existing state
            {
                //Update the new prospective state
                prospectiveState.Value.RequiredProperties.Add(new EntityProperty(propertyName, propertyValue));

                //Add the propsective state.
                EntityStates.Add((EntityState) prospectiveState);
            }
            else
                throw new Exception($"The existingState {prospectiveState} already had an entity state {prospectiveState.Value} in EntityStates {EntityStates}, but tried to add a null entitystate, too? The values were {propertyName}, {propertyValue}. The null state should only be used to mean that something exists. Therefore, it makes no sense to check if it exists and matches a state, since matching a state implicitly means the entity must exist!");
        }

        public EntityProperty Descriptor { get; init; }
        public List<EntityState> EntityStates { get; private set; } = new List<EntityState>();
        public uint Priority { get; init; } = setPriority;
    }
}
