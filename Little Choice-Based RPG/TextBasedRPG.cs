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
using Little_Choice_Based_RPG.Managers.Player_Manager.Frontend.UserInterface;
using Little_Choice_Based_RPG.Managers.World;
using Little_Choice_Based_RPG.Resources.Entities.Conceptual;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Equippables.Armour;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Furniture;
using Little_Choice_Based_RPG.Resources.Entities.Physical.Living.Players;
using Little_Choice_Based_RPG.Resources.Rooms;
using Little_Choice_Based_RPG.Resources.Rooms.Premade.Unique.Test;
using Little_Choice_Based_RPG.Resources.Systems.Damage.Break;
using Little_Choice_Based_RPG.Resources.Systems.Damage.Flammability;
using Little_Choice_Based_RPG.Resources.Systems.Damage.Repair;
using Little_Choice_Based_RPG.Resources.Systems.Gear;
using Little_Choice_Based_RPG.Resources.Systems.SystemEventBus;
using Little_Choice_Based_RPG.Types;
using Little_Choice_Based_RPG.Types.EntityProperties;

internal class TextBasedRPG
{
    private static void Main(string[] args)
    {
        SystemSubscriptionEventBus newSystemSubscription = new SystemSubscriptionEventBus();
        GameObjectFactory currentGameObjectFactory = new GameObjectFactory(newSystemSubscription);

        //Create test object to initialise the current systems:
        new FlammabilitySystem(new TestGameObject());
        new RepairSystem(new TestGameObject());
        new BreakSystem(new TestGameObject());
        new GearSystem();

        //Main
        var mainWorld = new GameEnvironment();
        mainWorld.GenerateAllRooms();

        uint spawnRoomID = mainWorld.Rooms.ElementAt(1).Key;

        //Set player spawn position
        Dictionary<string, object> playerProperties = new Dictionary<string, object>();
        playerProperties.Add("Position", spawnRoomID);

        //Create a helmet
        Dictionary<string, object> davodianMk1Helmet = new Dictionary<string, object>();
        davodianMk1Helmet.Add("Name", "Davodian MkI Covered Faceplate");
        davodianMk1Helmet.Add("Type", "Little_Choice_Based_RPG.Resources.Entities.Physical.Equippables.Armour.Helmet");
        davodianMk1Helmet.Add("Descriptor.Generic.Default", "Discarded aside here is your original Davodian MkI, its dented gunmetal grey faceplate long lustreless.");
        davodianMk1Helmet.Add("Descriptor.Inspect.Default", "Disfigured and mottled from years of abuse, the gunmetal grey protective sensors relay a live feed, protecting your vision and perceptible hearing.\r\nThe units bodywork has been long since reworked in patchwork copper shielding - under the seams near-charred components made up of scrapped debris poke through residual sand.");
        davodianMk1Helmet.Add("Descriptor.Equip", "Dusty residue coats your hands as you heave the thick, heavy metal above you and press your forehead in.\r\nInterlocking clicks engage behind your neck, a gentle buzz as the metal comes online.");
        davodianMk1Helmet.Add("Descriptor.Unequip", "Engaging the clasp at the rear, the locks reluctantly scrape their disengaging clicks and the full weight of the visor bears down on your head.\r\nSandpaper lining scratches the sides of your face when you tilt your head forwards and force the faceplate off.");

        davodianMk1Helmet.Add("Component.Repair", true);
        davodianMk1Helmet.Add("Repair.IsRepairable", true);
        davodianMk1Helmet.Add("Repair.IsRepairableByChoice", true);
        davodianMk1Helmet.Add("Descriptor.Repair.Choice.Interact", "Repair - Re - calibrate the helmets longitudinal wave sensor - array.");
        davodianMk1Helmet.Add("Descriptor.Repair.Choice.Repairing", "You fixed the helmet, yayy!");

        davodianMk1Helmet.Add("Component.Breakable", true);
        davodianMk1Helmet.Add("IsBreakable", true);
        davodianMk1Helmet.Add("IsBreakableByChoice", true);
        davodianMk1Helmet.Add("Descriptor.Choice.Break.Interact", "Damage - Intentionally misalign the helmets longitudinal wave sensor-array");
        davodianMk1Helmet.Add("Descriptor.Choice.Breaking", "You broke the helmet oh nooo!");
        GameObject testDavodian = currentGameObjectFactory.NewGameObject(davodianMk1Helmet);

        //Give the player a helmet
        playerProperties.Add("Gear.Slot.Helmet.ID", testDavodian.Properties.GetPropertyValue("ID"));

        //Main cont.
        var currentPlayer = new Player(playerProperties);
        var currentUserInterfaceHandler = new UserInterfaceHandler(currentPlayer, mainWorld);

        while (true)
        {
            currentUserInterfaceHandler.GenerateOutput();
        }
    }
}
