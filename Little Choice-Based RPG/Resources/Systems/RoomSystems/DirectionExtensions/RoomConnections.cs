using Little_Choice_Based_RPG.Types.Navigation;
using Little_Choice_Based_RPG.Types.PropertyExtensions;
using Little_Choice_Based_RPG.Types.PropertyExtensions.PropertyExtensionEventArgs;

namespace Little_Choice_Based_RPG.Resources.Systems.RoomSystems.DirectionExtensions
{
    /// <summary> Defines the traversible links between ItemContainers. 
    /// Note: This class is intended for use on Rooms. </summary>
    internal class RoomConnections : IPropertyExtension
    {
        public string UniqueIdentifier { get; init; } = "RoomConnections";
        public List<RoomConnection> LocalConnections { get; private set; } = new();

        public IEnumerable<object> GetAllEntries() => LocalConnections.Cast<object>();

        public void Add(RoomConnection target)
        {
            //Ensure only one of each source, destination and associated object exists at once
            if (LocalConnections.Any(i => i.Destination == target.Destination && i.Source == target.Source && target.AssociatedObject == i.AssociatedObject))
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
    }
}
