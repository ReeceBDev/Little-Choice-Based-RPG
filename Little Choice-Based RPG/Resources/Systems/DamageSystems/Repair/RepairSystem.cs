using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Repair;
using Little_Choice_Based_RPG.Resources.Systems.SystemEventBus;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.Interactions.InteractDelegate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Little_Choice_Based_RPG.Types.Interactions.InteractDelegate.InteractionUsingGameObject;
using static Little_Choice_Based_RPG.Types.Interactions.InteractDelegate.InteractionUsingNothing;

namespace Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Repair
{
    public class RepairSystem : PropertyLogic
    {
        //This class requires DamageLogic.
        DamageLogic damageCommonInstantiation = DamageLogic.Instance;

        /// <summary> Allows repairs to occur on this object. Requires DamageLogic. </summary>
        static RepairSystem()
        {
            //RepairSystem logic
            PropertyValidation.CreateValidProperty("Repairable.ByChoice", PropertyType.Boolean); //Lets players choose to repair it by choice.
            PropertyValidation.CreateValidProperty("Repairable.RequiresTool", PropertyType.Boolean); //Requires tools if true, but may be repaired by hand if not.
            PropertyValidation.CreateValidProperty("Repairable.RequiredToolType", PropertyType.String); //Must match the type on the repair tool (subject to change upon fleshing out this system.)
            PropertyValidation.CreateValidProperty("Repairable.ToolType", PropertyType.String); //Must match the type on the repair tool (subject to change upon fleshing out this system.)

            //Descriptors
            PropertyValidation.CreateValidProperty("Descriptor.Repair.Repairing", PropertyType.String); //Describes how it looks when it gets repaired.
            PropertyValidation.CreateValidProperty("Descriptor.Repair.Interaction.Title", PropertyType.String); //Describes the interact option presented to the player.
            PropertyValidation.CreateValidProperty("Descriptor.Repair.Interaction.Invoking", PropertyType.String); //Describes the action of repairing it when a player uses the Repair() choice.
        }
       
        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedData)
        {
            PropertyContainer sourceContainer = propertyChangedData.sourceContainer;
            PropertyHandler sourceProperties = sourceContainer.Properties;
            EntityProperty newProperty = propertyChangedData.newProperty;

            // When to do with Damage.Broken
            if (newProperty.PropertyName == "Damage.Broken")
            {
                // When something repairable breaks
                if (newProperty.PropertyName == "Damage.Broken" && newProperty.PropertyValue.Equals(true))
                {
                    if (sourceProperties.HasProperty("Repairable.ByChoice", true))
                        InitialiseRepairChoices(sourceContainer, sourceProperties);
                }
            }
        }

        protected override void InitialiseNewSubscriber(PropertyContainer sourceContainer, PropertyHandler sourceProperties)
        {
            // If the object is broken already
            if (sourceProperties.HasProperty("Damage.Broken", true))
            {
                if (sourceProperties.HasProperty("Repairable.ByChoice", true))
                    InitialiseRepairChoices(sourceContainer, sourceProperties);
            }
        }
            

        private static void InitialiseRepairChoices(PropertyContainer sourceContainer, PropertyHandler sourceProperties)
        {
            //Guard clauses for interaction descriptors
            if (!sourceProperties.HasExistingPropertyName("Descriptor.Repair.Interaction.Title"))
                throw new Exception($"{sourceContainer} of ID {sourceProperties.GetPropertyValue("ID")} has no property of Descriptor.Repair.Interaction.Title!");

            if (!sourceProperties.HasExistingPropertyName("Descriptor.Repair.Interaction.Invoking"))
                throw new Exception($"{sourceContainer} of ID {sourceProperties.GetPropertyValue("ID")} has no property of Descriptor.Repair.Interaction.Invoking!");


            // If the repair requires no tool, give an interaction using nothing.
            if (!(sourceProperties.HasExistingPropertyName("Repairable.RequiredToolType")))
                GiveInteraction(sourceContainer, InitialiseRepairUsingNothing(sourceContainer, sourceProperties));

            // If the repair require a tool, give an interaction requiring a gameobject of the given repairtooltype.
            if (sourceProperties.HasExistingPropertyName("Repairable.RequiredToolType"))
                GiveInteraction(sourceContainer, InitialiseRepairUsingTool(sourceContainer, sourceProperties));
        }

        public static InteractionUsingNothing InitialiseRepairUsingNothing(PropertyContainer sourceContainer, PropertyHandler sourceProperties)
        {
            InteractionUsingNothingDelegate repairUsingNothingDelegate = InvokeRepair;

            InteractionUsingNothing repairUsingNothing = new InteractionUsingNothing(
                repairUsingNothingDelegate,
                sourceContainer,
                (string)sourceProperties.GetPropertyValue("Descriptor.Repair.Interaction.Title"),
                (string)sourceProperties.GetPropertyValue("Descriptor.Repair.Interaction.Invoking")
            );

            return repairUsingNothing;
        }

        public static InteractionUsingGameObject InitialiseRepairUsingTool(PropertyContainer sourceContainer, PropertyHandler sourceProperties)
        {
            InteractionUsingGameObjectDelegate repairUsingGameObjectDelegate = InvokeRepair;

            InteractionUsingGameObject repairUsingTool = new InteractionUsingGameObject(
                repairUsingGameObjectDelegate,
                sourceContainer,
                (string)sourceProperties.GetPropertyValue("Descriptor.Repair.Interaction.Title"),
                (string)sourceProperties.GetPropertyValue("Descriptor.Repair.Interaction.Invoking"),
                $"Select a repair tool which is a {(string)sourceProperties.GetPropertyValue("Repairable.RequiredToolType")}.",
                InteractionRole.Explore,
                new List<EntityProperty>([new EntityProperty("Repairable.ToolType", (string)sourceProperties.GetPropertyValue("Repairable.RequiredToolType"))])
            );

            return repairUsingTool;
        }

        /// <summary> Sets Damage.Broken to false, from the BreakSystem. </summary>
        public static void InvokeRepair(IUserInterface mutexHolder, PropertyContainer sourceContainer) => InvokeRepair(mutexHolder, sourceContainer, null);

        /// <summary> Sets Damage.Broken to false, from the BreakSystem. </summary>
        public static void InvokeRepair(IUserInterface mutexHolder, PropertyContainer sourceContainer, GameObject? repairTool = null)
        {
            //Guard clauses for the values in use.
            if (!sourceContainer.Properties.HasProperty("Damage.Broken", true))
                throw new Exception("This object is not broken! Tried to repair an object where there is no EntityProperty of Damage.Broken = true.");

            //Check if a repair tool is required.
            if (!sourceContainer.Properties.HasProperty("Repairable.RequiresTool", true))
            {
                //When no repair tool is required, initate repair.
                Repair(sourceContainer);
                return;
            }

            //When a repair tool is required:
            if (repairTool == null)
                return; // Error, can't repair without a tool!

            //If repair tool doesn't match
            if (!repairTool.Properties.HasProperty("Repairable.ToolType", sourceContainer.Properties.GetPropertyValue("Repairable.RequiredToolType")))
                return; //Error, repairtooltype doesn't match the requiredrepairtool on this object

            //Repair if repair tool is correct :)
            Repair(sourceContainer);
        }

        private static void Repair(PropertyContainer sourceContainer)
        {
            //Main repairing logic
            sourceContainer.Properties.UpsertProperty("Damage.Broken", false); //Property found in DamageLogic

            //Set the generic and inspect descriptors back to default.
            sourceContainer.Properties.ReplaceProperty("Descriptor.Generic.Current", sourceContainer.Properties.GetPropertyValue("Descriptor.Generic.Default"));
            sourceContainer.Properties.ReplaceProperty("Descriptor.Inspect.Current", sourceContainer.Properties.GetPropertyValue("Descriptor.Generic.Default"));
        }
    }
}
