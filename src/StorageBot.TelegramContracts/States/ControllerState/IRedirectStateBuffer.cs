namespace StorageBot.TelegramContracts.States.ControllerState
{
    public interface IRedirectStateBuffer
    {
        RedirectState? Consume();

        void SetRedirect(int controllerId, int actionId);
    }
}