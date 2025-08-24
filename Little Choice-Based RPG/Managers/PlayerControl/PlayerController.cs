using Little_Choice_Based_RPG.External.ConsoleEndpoint;
using Little_Choice_Based_RPG.External.ConsoleEndpoint.ConsoleFunctionalities;
using Little_Choice_Based_RPG.Managers.World;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Resources.Entities.Rooms;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory.InventoryExtensions;
using Little_Choice_Based_RPG.Types.TypedEventArgs;

namespace Little_Choice_Based_RPG.Managers.PlayerControl
{
    /// <summary> One controller is made per active player. Holds information about its player. Provides a central point of access to the entire application. Requires InventorySystem. </summary>
    internal class PlayerController
    {
        public Player CurrentPlayer { get; init; } 
        public HistoryLogElement CurrentHistoryLog { get; init; } = new();
        public InteractionCache CurrentInteractionCache { get; init; }
        public ConsoleEndpoint CurrentConsoleEndpoint { get; private set; }

        public GameEnvironment CurrentEnvironment { get; private set; }
        public Room CurrentRoom
        {
            get;
            private set
            {
                Room oldRoom = field;
                Room newRoom = value;

                //Set the new room
                field = newRoom;

                if (CurrentPlayer is not null)
                    HandlePlayerChangedRoom(newRoom, oldRoom);

                OnPlayerChangedRoom(oldRoom, newRoom);
            }
        }

        public event EventHandler<string> ReceivedLocalUserMessage;
        public event EventHandler<PlayerChangedRoomEventArgs> PlayerChangedRoom;

        public PlayerController(Room setCurrentRoom, GameEnvironment setCurrentEnvironment)
        {
            //Set current room, environment and player on this controller
            CurrentRoom = setCurrentRoom;
            CurrentEnvironment = setCurrentEnvironment;
            CurrentInteractionCache = new InteractionCache(this);

            //Generate the player properties
            //Set player spawn position
            Dictionary<string, object> playerProperties = new Dictionary<string, object>();
            //playerProperties.Add("Position", setCurrentRoom.Properties.GetPropertyValue("ID"));
            playerProperties.Add("Type", "Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players.Player");
            playerProperties.Add("Name", "A Mysterious Man");
            playerProperties.Add("Descriptor.Generic.Default", "You are here.");
            playerProperties.Add("Descriptor.Inspect.Default", "You pat yourself down. You are still here.");

            //Create a helmet
            Dictionary<string, object> davodianMk1Helmet = new Dictionary<string, object>();
            davodianMk1Helmet.Add("Name", "Davodian MkI Covered Faceplate");
            davodianMk1Helmet.Add("Type", "Little_Choice_Based_RPG.Resources.Entities.Physical.Equippables.Armour.Helmet");
            davodianMk1Helmet.Add("Descriptor.Generic.Default", "Discarded aside here is your original Davodian MkI, its dented gunmetal grey faceplate long lustreless.");
            davodianMk1Helmet.Add("Descriptor.Inspect.Default", "Disfigured and mottled from years of weathering, the gunmetal grey protective sensors relay a live feed, protecting your vision and perceptible hearing.\r\nThe units bodywork has been long since reworked in patchwork copper shielding - under the seams near-charred components made up of scrapped debris poke through residual sand.");
            //davodianMk1Helmet.Add("Descriptor.Equip", "Dusty residue coats your hands as you heave the thick, heavy metal above you and press your forehead in.\r\nInterlocking clicks engage behind your neck, a gentle buzz as the metal comes online.");
            //davodianMk1Helmet.Add("Descriptor.Unequip", "Engaging the clasp at the rear, the locks reluctantly scrape their disengaging clicks and the full weight of the visor bears down on your head.\r\nSandpaper lining scratches the sides of your face when you tilt your head forwards and force the faceplate off.");

            davodianMk1Helmet.Add("Component.RepairSystem", true);
            davodianMk1Helmet.Add("Repairable.ByChoice", true);
            davodianMk1Helmet.Add("Descriptor.Repair.Interaction.Title", "Repair - Re - calibrate the helmets longitudinal wave sensor - array.");
            davodianMk1Helmet.Add("Descriptor.Repair.Interaction.Invoking", "You fixed the helmet, yayy!");

            davodianMk1Helmet.Add("Component.BreakSystem", true);
            davodianMk1Helmet.Add("Breakable.ByChoice", true);
            davodianMk1Helmet.Add("Descriptor.Breakable.Interaction.Title", "Damage - Intentionally misalign the helmets longitudinal wave sensor-array");
            davodianMk1Helmet.Add("Descriptor.Breakable.Interaction.Invoking", "You broke the helmet oh nooo!");

            davodianMk1Helmet.Add("Descriptor.InventorySystem.Interaction.Pickup.Title", "");
            davodianMk1Helmet.Add("Descriptor.InventorySystem.Interaction.Pickup.Invoking", "");
            davodianMk1Helmet.Add("Descriptor.InventorySystem.Interaction.Drop.Title", "");
            davodianMk1Helmet.Add("Descriptor.InventorySystem.Interaction.Drop.Invoking", "");
            GameObject testDavodian = (GameObject) PropertyContainerFactory.New(davodianMk1Helmet);

            //Give the player a helmet
            playerProperties.Add("Gear.Slot.Helmet.ID", testDavodian.Properties.GetPropertyValue("ID"));


            //Generate the player
            CurrentPlayer = (Player) PropertyContainerFactory.New(playerProperties);
            InitialisePlayer();
        }

        private void InitialisePlayer()
        {
            //Put the player into the room
            InventoryProcessor.StoreInInventory(this, CurrentRoom, CurrentPlayer);

            //Generate userinterface
            CurrentConsoleEndpoint = new ConsoleEndpoint(this);

            //Subscribe to the Room
            //CurrentRoom.ObjectChanged += OnCurrentRoomUpdated;

            //Force the player notice this Room
            HandlePlayerChangedRoom(CurrentRoom, null);

            //Add the player's own descriptor as the first history log entry.
            CurrentHistoryLog.AddNewHistoryLog((string)CurrentPlayer.Properties.GetPropertyValue("Descriptor.Player.Custom"));

            //Run user interface
            while (true)
            {
                CurrentConsoleEndpoint.GenerateOutput();
            }
        }

        private void HandlePlayerChangedRoom(Room newRoom, Room? oldRoom)
        {
            //Subscribe them to the new local user message event
            ((ItemContainer)newRoom.Extensions.Get("ItemContainer")).BroadcastLocalUserMessage += OnBroadcastLocalUserMessage;

            //Unsubscribe the player from the old local user message event
            if (oldRoom != null)
                ((ItemContainer)oldRoom.Extensions.Get("ItemContainer")).BroadcastLocalUserMessage -= OnBroadcastLocalUserMessage;
        }

        protected virtual void OnBroadcastLocalUserMessage(object sender, string userMessage)
        {
            ReceivedLocalUserMessage?.Invoke(this, userMessage);
        }

        private void OnPlayerChangedRoom(Room oldRoom, Room newRoom) => PlayerChangedRoom?.Invoke(this, new PlayerChangedRoomEventArgs(oldRoom, newRoom));
    }
}
