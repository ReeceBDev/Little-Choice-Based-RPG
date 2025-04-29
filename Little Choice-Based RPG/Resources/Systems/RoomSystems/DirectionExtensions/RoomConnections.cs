using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory.InventoryExtensions;
using Little_Choice_Based_RPG.Types.EntityProperties;
using Little_Choice_Based_RPG.Types.Navigation;
using Little_Choice_Based_RPG.Types.PropertyExtensions;
using Little_Choice_Based_RPG.Types.PropertyExtensions.PropertyExtensionEventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Resources.Systems.RoomSystems.ConnectionExtensions
{
    /// <summary> Defines the traversible links between ItemContainers. 
    /// Note: This class is intended for use on Rooms. </summary>
    public class RoomConnections : IPropertyExtension
    {
        public void Add(RoomConnection target)
        {
            //Ensure only one of each source, destination and associated object exists at once
            if (LocalConnections.Any(i => (i.Destination == target.Destination) && (i.Source == target.Source) && (target.AssociatedObject == i.AssociatedObject)))
                throw new Exception("Tried to add a new RoomConnection, but the same one already existed on this list of LocalConnections!");

            LocalConnections.Add(target);
            PropertyExtensionChanged?.Invoke(this, new PropertyExtensionChangedArgs("RoomConnection.Added", target));

            //Subscribe to the RoomConnection's Modified event.
            target.RoomConnectionModified += OnRoomConnectionModified;
        }

        public void Remove(RoomConnection target)
        {
            LocalConnections.Remove(target);
            PropertyExtensionChanged?.Invoke(this, new PropertyExtensionChangedArgs("RoomConnection.Removed", target));

            //Unsubscribe from the RoomConnection's Modified event.
            target.RoomConnectionModified -= OnRoomConnectionModified;
        }

        protected virtual void OnRoomConnectionModified(object? targetModified, (string property, object value) args)
        {
            PropertyExtensionChanged?.Invoke(this, new PropertyExtensionChangedArgs("RoomConnection.Modified", (targetModified, args.property, args.value)));
        }

        public event EventHandler<PropertyExtensionChangedArgs> PropertyExtensionChanged;
        public string UniqueIdentifier { get; init; } = "RoomConnections";
        public List<RoomConnection> LocalConnections { get; private set; } = new();
    }
}
