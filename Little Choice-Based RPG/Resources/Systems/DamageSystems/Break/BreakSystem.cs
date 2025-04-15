using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Repair;
using Little_Choice_Based_RPG.Resources.Systems.SystemEventBus;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.Interactions.InteractDelegate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static Little_Choice_Based_RPG.Types.Interactions.InteractDelegate.InteractionUsingNothing;

namespace Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Break
{
    public class BreakSystem : PropertyLogic
    {
        static BreakSystem()
        {
            //BreakSystem logic
            PropertyValidation.CreateValidProperty("Breakable.ByChoice", PropertyType.Boolean); //Lets players choose to break it by choice. 

            //Descriptors
            PropertyValidation.CreateValidProperty("Descriptor.Breakable.Interaction.Title", PropertyType.String); //Describes the interact option presented to the player.
            PropertyValidation.CreateValidProperty("Descriptor.Breakable.Interaction.Invoking", PropertyType.String); //Describes the action of breaking it when a player uses the Break() choice.
            PropertyValidation.CreateValidProperty("Descriptor.Generic.Broken", PropertyType.String); //Broken at a distance
            PropertyValidation.CreateValidProperty("Descriptor.Inspect.Broken", PropertyType.String); //A closer look
        }

        /// <summary> Allows objects to break. Requires DamageLogic. </summary>
        public BreakSystem()
        {
            DamageLogic damageCommonInstantiation = DamageLogic.Instance;
        }

        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedData)
        {
            PropertyContainer sourceContainer = propertyChangedData.sourceContainer;
            PropertyHandler sourceProperties = sourceContainer.Properties;
            EntityProperty newProperty = propertyChangedData.newProperty;

            // When to do with Damage.Broken
            if (newProperty.PropertyName == "Damage.Broken")
            {
                // When something breakable is repaired
                if (newProperty.PropertyName == "Damage.Broken" && newProperty.PropertyValue.Equals(false))
                {
                    if (sourceProperties.HasProperty("Breakable.ByChoice", true))
                        InitialiseBreakChoices(sourceContainer, sourceProperties);
                }
            }
        }

        protected override void InitialiseNewSubscriber(PropertyContainer sourceContainer, PropertyHandler sourceProperties)
        {
            // If the object is not broken
            if (sourceProperties.HasProperty("Damage.Broken", false))
            {
                if (sourceProperties.HasProperty("Breakable.ByChoice", true))
                    InitialiseBreakChoices(sourceContainer, sourceProperties);
            }
        }


        private void InitialiseBreakChoices(PropertyContainer sourceContainer, PropertyHandler sourceProperties)
        {
            //Guard clauses for interaction descriptors
            if (!sourceProperties.HasExistingPropertyName("Descriptor.Breakable.Interaction.Title"))
                throw new Exception($"{sourceContainer} of ID {sourceProperties.GetPropertyValue("ID")} has no property of Descriptor.Breakable.Interaction.Title!");

            if (!sourceProperties.HasExistingPropertyName("Descriptor.Breakable.Interaction.Invoking"))
                throw new Exception($"{sourceContainer} of ID {sourceProperties.GetPropertyValue("ID")} has no property of Descriptor.Breakable.Interaction.Invoking!");


            //Initialise the Interaction for Break()
            InteractionUsingNothingDelegate breakUsingNothingDelegate = Break;

            InteractionUsingNothing breakUsingNothing = new InteractionUsingNothing(
                breakUsingNothingDelegate,
                sourceContainer,
                (string)sourceProperties.GetPropertyValue("Descriptor.Breakable.Interaction.Title"),
                (string)sourceProperties.GetPropertyValue("Descriptor.Breakable.Interaction.Invoking")
            );

            GiveInteraction(sourceContainer, breakUsingNothing);
        }


        /// <summary> Sets Damage.Broken to true. </summary>
        public void Break(IUserInterface mutexHolder, PropertyContainer sourceContainer)
        {
            //Guard clauses for the values in use.
            if (sourceContainer.Properties.HasProperty("Damage.Broken", true))
                throw new Exception("This object is already broken! Tried to break an object where there is already an EntityProperty of Damage.Broken = true.");

            if (!sourceContainer.Properties.HasExistingPropertyName("Descriptor.Generic.Broken"))
                throw new Exception("This object has no broken description! Tried to break an object where there is no EntityProperty of Descriptor.Generic.Broken.");

            //Main breaking logic.
            sourceContainer.Properties.UpsertProperty("Damage.Broken", true); //Property found in DamageLogic

            //Set generic descriptor.
            sourceContainer.Properties.ReplaceProperty("Descriptor.Generic.Current", sourceContainer.Properties.GetPropertyValue("Descriptor.Generic.Broken"));

            //Set inspect descriptor or default to generic.
            if (!sourceContainer.Properties.HasExistingPropertyName("Descriptor.Inspect.Broken"))
                sourceContainer.Properties.ReplaceProperty("Descriptor.Inspect.Current", sourceContainer.Properties.GetPropertyValue("Descriptor.Inspect.Broken"));
            else
                sourceContainer.Properties.ReplaceProperty("Descriptor.Inspect.Current", sourceContainer.Properties.GetPropertyValue("Descriptor.Generic.Broken"));
        }
    }
}
