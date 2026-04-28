namespace StorageBot.TelegramContracts.States
{
    public interface IRedirectStateProvider
    {
        RedirectState? Consume();

        void SetRedirect(int controllerId, int actionId);
    }
}