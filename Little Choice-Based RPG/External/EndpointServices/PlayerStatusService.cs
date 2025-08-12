using Little_Choice_Based_RPG.Managers.PlayerControl;
using Little_Choice_Based_RPG.Managers.Server;
using Little_Choice_Based_RPG.Resources.Systems.InformationalSystems.Descriptor;

namespace Little_Choice_Based_RPG.External.EndpointServices
{
    public sealed class PlayerStatusService
    {
        private PlayerController currentPlayerData;
        public PlayerStatusService(ulong authenticationToken)
        {
            currentPlayerData = SessionManager.GetPlayerController(authenticationToken);
        }

        public string GetPlayerName() => (string)currentPlayerData.CurrentPlayer.Properties.GetPropertyValue("Name");
        public int GetPlayerID() => (int)currentPlayerData.CurrentPlayer.Properties.GetPropertyValue("ID");
        public string GetPlayerLocation() => (string)currentPlayerData.CurrentRoom.Properties.GetPropertyValue("Name");
        public string GetPlayerLocationDescription() => DescriptorProcessor.GetDescriptor(currentPlayerData.CurrentRoom, "Descriptor.Generic.Current");
        public int GetPlayerHealth() => (int)currentPlayerData.CurrentPlayer.Properties.GetPropertyValue("Health");
        public decimal GetPlayerWeightHeld()
        {
            decimal weightInKG = currentPlayerData.CurrentPlayer.Properties.HasExistingPropertyName("Weightbearing.WeightHeldInKG") ?
                (decimal) currentPlayerData.CurrentPlayer.Properties.GetPropertyValue("Weightbearing.WeightHeldInKG") : 0;

            return weightInKG;
        }
        public decimal GetPlayerStrength() => (decimal) currentPlayerData.CurrentPlayer.Properties.GetPropertyValue("Weightbearing.StrengthInKG");
    }
}