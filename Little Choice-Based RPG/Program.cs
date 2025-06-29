﻿using System.Runtime.CompilerServices;
using Little_Choice_Based_RPG.Managers.PlayerControl;
using Little_Choice_Based_RPG.Managers.World;
using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Attach;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Gear;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Inventory;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems.Weightbearing;
using Little_Choice_Based_RPG.Resources.Systems.DamageSystems;
using Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Break;
using Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Flammability;
using Little_Choice_Based_RPG.Resources.Systems.DamageSystems.Repair;
using Little_Choice_Based_RPG.Resources.Systems.InformationalSystems.Descriptor;
using Little_Choice_Based_RPG.Resources.Systems.InteractionSystems.PrivateInteractionsSystems;
using Little_Choice_Based_RPG.Resources.Systems.PlayerSystems;
using Little_Choice_Based_RPG.Resources.Systems.RoomSystems;

internal class Program
{
    private static void Main(string[] args)
    {
        //Initialise the current systems:        
        RuntimeHelpers.RunClassConstructor(typeof(GlobalDamageProperties).TypeHandle);
        new FlammabilitySystem();
        new RepairSystem();
        new BreakSystem();
        new GearSystem();
        new PlayerSystem();
        new InventorySystem();
        new AttachSystem();
        new DescriptorSystem();
        new WeightbearingSystem();
        new PublicInteractionsSystem();
        new DirectionSystem();

        //Generate the environment
        GameEnvironment currentEnvironment = new GameEnvironment();
        currentEnvironment.GeneratePredefinedRooms(); //Generate rooms within the environment

        //Generate a player
        Room spawnRoom = currentEnvironment.Rooms.GetRoom(0,0,0); //Select the room to spawn the player in
        PlayerController player1 = new PlayerController(spawnRoom, currentEnvironment); //Create the player in that room
    }
}
