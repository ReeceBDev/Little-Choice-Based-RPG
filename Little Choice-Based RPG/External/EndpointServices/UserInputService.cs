using Little_Choice_Based_RPG.Managers.PlayerControl;
using Little_Choice_Based_RPG.Types.TypedEventArgs;

namespace Little_Choice_Based_RPG.External.EndpointServices
{
    public class UserInputService
    {
        public event EventHandler<string> ThrowErrorMessage;
        public event EventHandler<UserDecisionEventArgs> RequestDecisionSubMenu;
        public event EventHandler RequestOpenContainerSubMenu;

        public static void HandleUserInput(string input) => UserInputHandler.HandleUserInput(input);

        public void OnThrowErrorMessage(string errorMessage) => ThrowErrorMessage?.Invoke(this, errorMessage);
        public void OnRequestDecisionSubMenu(string submenuTitle, string[] submenuEntries) => RequestDecisionSubMenu?.Invoke(this, new UserDecisionEventArgs(submenuTitle, submenuEntries));
        public void OnRequestOpenContainerSubMenu(string submenuTitle, string[] submenuEntries) => RequestOpenContainerSubMenu?.Invoke(this, new UserDecisionEventArgs(submenuTitle, submenuEntries)); //Yes, they use the same eventargs for now. It works fine :)
    }
}
