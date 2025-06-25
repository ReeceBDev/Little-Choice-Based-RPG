using Little_Choice_Based_RPG.Resources.Entities;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.PropertyContainerEventArgs;
using Little_Choice_Based_RPG.Types.EntityProperties;

namespace Little_Choice_Based_RPG.Types.Navigation
{
    public struct RoomConnection
    {
        /// <summary> The origin point. This is where the connection may be accessed. </summary>
        public PropertyContainer Source { get; init; }

        /// <summary> The destination point. This is where the connection leads to. </summary>
        public PropertyContainer Destination { get; init; }

        /// <summary> The direction from the origin point facing the destination, if there is one. </summary>
        public CardinalDirection? Direction { get; init; }

        /// <summary> Represents a GameObject which is responsible for controlling the Connection's Visibility. 
        /// If this is null, visibility cannot be set. </summary>
        public GameObject? AssociatedObject { get; init; } = null;

        /// <summary> Represents if the connection is visible, and therefore, traversible. </summary>
        public bool IsVisible { get; private set; } = true;

        static RoomConnection()
        {
            PropertyValidation.CreateValidProperty("Connection.IsAccessible", PropertyType.Boolean);
        }

        public RoomConnection(PropertyContainer setSource, PropertyContainer setDestination, CardinalDirection? setDirection = null,
            GameObject? setAssociatedObject = null)
        {
            //Ensure that the Source has an ItemContainer to be moved from.
            if (!setSource.Extensions.Contains("ItemContainer"))
                throw new Exception("Tried to create a RoomConnection, but the Source didn't have an ItemContainer! There is nowhere for a source without an ItemContainer to get the movement target from!");

            //Ensure that the Destination has an ItemContainer to be moved into.
            if (!setDestination.Extensions.Contains("ItemContainer"))
                throw new Exception("Tried to create a RoomConnection, but the Destination didn't have an ItemContainer! There is no where to put the movement target!");

            Source = setSource;
            Destination = setDestination;
            Direction = setDirection;


            // When the connection is associated with an object, make it invisible by default.
            // This forces the implementing object to reveal the connection when accessible, i.e. a Door being open.
            if (setAssociatedObject is not null)
            {
                IsVisible = false;

                //Ensure that the AssociatedObject has a property to control the visibility of this.
                if (!setAssociatedObject.Properties.HasExistingPropertyName("Connection.IsAccessible"))
                    throw new Exception("Tried to create a RoomConnection with an Associated Object, but the Associated Object didn't have a property with which to control this Connection's Visibility! Are you sure the Associated Object knew how to handle this Room Connection?");

                //Set the associated Object
                AssociatedObject = setAssociatedObject;

                //Subscribe to its property changed list, to watch for accessibility changes!
                AssociatedObject.ObjectChanged += OnObjectChanged;
            }
        }
        public event EventHandler<(string property, object change)> RoomConnectionModified;
        private void OnObjectChanged(object? sender, ObjectChangedEventArgs e)
        {
            switch (e.Property, e.Change)
            {
                //Upon the connection being made inaccessible, set to invisible.
                case ("Connection.IsAccessible", false):
                    {
                        IsVisible = false;
                        RoomConnectionModified?.Invoke(this, ("IsVisible", false));
                    }
                    break;

                //Upon the connection being made accessible, set to visible.
                case ("Connection.IsAccessible", true):
                    {
                        IsVisible = true;
                        RoomConnectionModified?.Invoke(this, ("IsVisible", true));
                    }

                    break;

                //Upon object being destroyed, open up.
                case ("Damage.Destroyed", true):
                    {
                        IsVisible = true;
                        RoomConnectionModified?.Invoke(this, ("IsVisible", true));
                    }
                    break; 
            }
        }
    }
}