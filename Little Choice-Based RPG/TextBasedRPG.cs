using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml;
using Little_Choice_Based_RPG.Managers.Player_Manager;
using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface;
using Little_Choice_Based_RPG.Managers.World;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Equippables.Armour;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Furniture;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Resources.Systems.ContainerSystems;
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
using Little_Choice_Based_RPG.Resources.Systems.SystemEventBus;
using Little_Choice_Based_RPG.Types;
using Little_Choice_Based_RPG.Types.EntityProperties;

internal class TextBasedRPG
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
        new PrivateInteractionsSystem();
        new DirectionSystem();

        //Generate the environment
        GameEnvironment currentEnvironment = new GameEnvironment();
        currentEnvironment.GeneratePredefinedRooms(); //Generate rooms within the environment

        //Generate a player
        Room spawnRoom = currentEnvironment.Rooms.GetRoom(0,0,0); //Select the room to spawn the player in
        PlayerController player1 = new PlayerController(spawnRoom, currentEnvironment); //Create the player in that room
    }
}
