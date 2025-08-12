using Little_Choice_Based_RPG.Managers.PlayerControl;
using Little_Choice_Based_RPG.Managers.Server;
using Little_Choice_Based_RPG.Types.TypedEventArgs;

namespace Little_Choice_Based_RPG.External.EndpointServices
{
    public sealed class UserInputService
    {
        private PlayerController playerReference;

        public UserInputService(ulong authenticationToken)
        {
            playerReference = SessionManager.GetPlayerController(authenticationToken);
        }


        public void HandleUserInteraction(ulong input) => UserInputHandler.HandleUserInput(input);
        public void HandleUserCommand(string input) => UserCommands.TryCommand(input, playerReference, out _);

        public void OnThrowErrorMessage(string errorMessage) => ThrowErrorMessage?.Invoke(this, errorMessage);
        public void OnRequestDecisionSubMenu(string submenuTitle, string[] submenuEntries) => RequestDecisionSubMenu?.Invoke(this, new UserDecisionEventArgs(submenuTitle, submenuEntries));
        public void OnRequestOpenContainerSubMenu(string submenuTitle, string[] submenuEntries) => RequestOpenContainerSubMenu?.Invoke(this, new UserDecisionEventArgs(submenuTitle, submenuEntries)); //Yes, they use the same eventargs for now. It works fine :)

        public event EventHandler<string> ThrowErrorMessage;
        public event EventHandler<UserDecisionEventArgs> RequestDecisionSubMenu;
        public event EventHandler RequestOpenContainerSubMenu;

    }
}
