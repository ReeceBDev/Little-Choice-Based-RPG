using Little_Choice_Based_RPG.Types.PropertySystem.Components;
using Little_Choice_Based_RPG.Types.PropertySystem.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Components.CommonComponents
{
    internal class IdentityComponent : IPropertyComponent
    {
        /// <summary> Unique identity number. This value is incremental and no two properties will be the same. </summary>
        public TransparentProperty<uint>? ID { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }

        /// <summary> The object type which an entity represents. Entities are casted to this and must adhere to it religiously. </summary>
        public TransparentProperty<Type>? EntityType { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }

        /// <summary> A human readable name. Usually for display purposes. Does not need to be unique and cannot be used for differentiating instances. </summary>
        public TransparentProperty<string>? Name { get; set => PropertyHandler.ReplaceTransparentProperty(ref field, value); }
    }
}