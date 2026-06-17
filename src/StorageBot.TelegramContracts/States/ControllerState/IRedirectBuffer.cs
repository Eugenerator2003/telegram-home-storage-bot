namespace StorageBot.TelegramContracts.States.ControllerState
{
    public interface IRedirectBuffer
    {
        RedirectState? Consume();

        void SetRedirect(int controllerId, int actionId);
    }
}