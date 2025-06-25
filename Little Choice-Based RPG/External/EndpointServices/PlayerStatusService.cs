using Little_Choice_Based_RPG.Managers.PlayerControl;
using Little_Choice_Based_RPG.Resources.Systems.InformationalSystems.Descriptor;

namespace Little_Choice_Based_RPG.External.EndpointServices
{
    public class PlayerStatusService
    {
        public static string GetPlayerName(PlayerController currentPlayerController) => (string)currentPlayerController.CurrentPlayer.Properties.GetPropertyValue("Name");
        public static int GetPlayerID(PlayerController currentPlayerController) => (int)currentPlayerController.CurrentPlayer.Properties.GetPropertyValue("ID");
        public static string GetPlayerLocation(PlayerController currentPlayerController) => (string) currentPlayerController.CurrentRoom.Properties.GetPropertyValue("Name");
        public static string GetPlayerLocationDescription(PlayerController currentPlayerController) => DescriptorProcessor.GetDescriptor(currentPlayerController.CurrentRoom, "Descriptor.Generic.Current");
        public static int GetPlayerHealth(PlayerController currentPlayerController) => (int) currentPlayerController.CurrentPlayer.Properties.GetPropertyValue("Health");
    }
}
