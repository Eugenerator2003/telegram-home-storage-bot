using Microsoft.Extensions.Localization;
using StorageBot.Application.UseCases;
using StorageBot.Host.Routing;
using StorageBot.Host.Routing.Attributes.Actions.Required;
using StorageBot.Host.Utilities;
using StorageBot.TelegramContracts.Context;
using StorageBot.TelegramContracts.States.Chat;
using StorageBot.TelegramContracts.States.ControllerState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions;
using Telegram.Bot.Types;

namespace StorageBot.Host.Controllers
{
    public abstract class BotController
    {
        protected ITelegramBotClient BotClient => _dependencies.BotClient;
        protected IStringLocalizer Localizer => _dependencies.Localizer;
        protected INavigation Navigation => _dependencies.Navigation;
        protected IRequiredNavigation RequiredNavigation => _dependencies.RequiredNavigation;
        protected IMenuMessageStore MenuStore => _dependencies.MenuStore;
        protected IRedirectBuffer RedirectBuffer => _dependencies.RedirectBuffer;

        protected BotContext Context { get; private set; } = null!;

        private ControllerDependencies _dependencies = null!;

        public void SetDependencies(ControllerDependencies dependencies)
        {
            ArgumentNullException.ThrowIfNull(dependencies);
            _dependencies = dependencies;
        }

        public void SetContext(BotContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            Context = context;
        }

        public RedirectState? ConsumeRedirectState()
        {
            return RedirectBuffer.Consume();
        }

        public async Task ShowErrorMessage(string errorText)
        {
            var chatId = Context.Chat.Id;
            var menuMessageId = await MenuStore.GetMenuMessageId(chatId);
            var menuActionNavigation = RequiredNavigation.GetRequiredAction<MenuActionAttribute>();
            var callback = CallbackDataEncoder.Encode(
                    new CallbackData(menuActionNavigation.ControllerId, menuActionNavigation.ActionId)
                );
            await BotClient.EditHtml(chatId, (int)menuMessageId, 
                $"""
                {Localizer["View.ShowError"]}: {errorText}
                <keyboard>
                    <button text="{Localizer["View.BackToMenu"]}" callback={callback}>
                </keyboard>
                """);
        }
    }
}
