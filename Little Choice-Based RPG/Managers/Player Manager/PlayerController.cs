﻿using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface;
using Little_Choice_Based_RPG.Managers.World;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Resources.EventArgs;
using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Managers.Player_Manager
{
    /// <summary> One controller is made per active player. Holds information about its player. Provides a central point of access to the entire application. Requires InventorySystem. </summary>
    public class PlayerController
    {
        InventorySystem currentInventorySystem = new InventorySystem.Instance();
        public PlayerController(Room setCurrentRoom, GameEnvironment setCurrentEnvironment)
        {
            //Set current room, environment and player on this controller
            CurrentRoom = setCurrentRoom;
            CurrentEnvironment = setCurrentEnvironment;

            //Generate the player properties
            //Set player spawn position
            Dictionary<string, object> playerProperties = new Dictionary<string, object>();
            playerProperties.Add("Position", CurrentRoom.RoomID);

            //Create a helmet
            Dictionary<string, object> davodianMk1Helmet = new Dictionary<string, object>();
            davodianMk1Helmet.Add("Name", "Davodian MkI Covered Faceplate");
            davodianMk1Helmet.Add("Type", "Little_Choice_Based_RPG.Resources.Entities.Physical.Equippables.Armour.Helmet");
            davodianMk1Helmet.Add("Descriptor.Generic.Default", "Discarded aside here is your original Davodian MkI, its dented gunmetal grey faceplate long lustreless.");
            davodianMk1Helmet.Add("Descriptor.Inspect.Default", "Disfigured and mottled from years of abuse, the gunmetal grey protective sensors relay a live feed, protecting your vision and perceptible hearing.\r\nThe units bodywork has been long since reworked in patchwork copper shielding - under the seams near-charred components made up of scrapped debris poke through residual sand.");
            davodianMk1Helmet.Add("Descriptor.Equip", "Dusty residue coats your hands as you heave the thick, heavy metal above you and press your forehead in.\r\nInterlocking clicks engage behind your neck, a gentle buzz as the metal comes online.");
            davodianMk1Helmet.Add("Descriptor.Unequip", "Engaging the clasp at the rear, the locks reluctantly scrape their disengaging clicks and the full weight of the visor bears down on your head.\r\nSandpaper lining scratches the sides of your face when you tilt your head forwards and force the faceplate off.");

            davodianMk1Helmet.Add("Component.RepairSystem", true);
            davodianMk1Helmet.Add("Repairable.ByChoice", true);
            davodianMk1Helmet.Add("Descriptor.Repair.Interaction.Title", "Repair - Re - calibrate the helmets longitudinal wave sensor - array.");
            davodianMk1Helmet.Add("Descriptor.Repair.Interaction.Invoking", "You fixed the helmet, yayy!");

            davodianMk1Helmet.Add("Component.BreakSystem", true);
            davodianMk1Helmet.Add("Breakable.ByChoice", true);
            davodianMk1Helmet.Add("Descriptor.Breakable.Interaction.Title", "Damage - Intentionally misalign the helmets longitudinal wave sensor-array");
            davodianMk1Helmet.Add("Descriptor.Breakable.Interaction.Invoking", "You broke the helmet oh nooo!");
            GameObject testDavodian = (GameObject) PropertyContainerFactory.NewGameObject(davodianMk1Helmet);

            //Give the player a helmet
            playerProperties.Add("Gear.Slot.Helmet.ID", testDavodian.Properties.GetPropertyValue("ID"));

            //Generate the player
            CurrentPlayer = (Player) PropertyContainerFactory.NewGameObject(playerProperties);

            //Put the player into the room
            currentInventorySystem.StoreInInventory(this, CurrentRoom, CurrentPlayer);

            //Generate userinterface
            var currentUserInterfaceHandler = new UserInterfaceHandler(this);

            //Subscribe to the Room
            CurrentRoom.ObjectChanged += OnCurrentRoomUpdated;

            //Run user interface
            while (true)
            {
                currentUserInterfaceHandler.GenerateOutput();
            }
        }

        private void HandlePlayerChangedRoom()
        {
            HandleRoomInventorySystemInteractions();
        }

        private void HandleRoomInventorySystemInteractions()
        {
            //Remove old room pickup interactions
            throw new NotImplementedException();

            //Add new room pickups interactions
            currentInventorySystem.GivePlayerRoomPickups(CurrentPlayer, CurrentRoom);
        }

        protected virtual void OnCurrentRoomUpdated(object sender, ObjectChangedEventArgs roomUpdateArgs)
        {
            currentInventorySystem.OnRoomUpdate(CurrentPlayer, roomUpdateArgs);
        }

        public GameEnvironment CurrentEnvironment { get; private set; }
        public Room CurrentRoom
        { 
            get; 
            private set
            {
                field = value;

                HandlePlayerChangedRoom();
            }
        }
        public Player CurrentPlayer { get; private set; }
    }
}
