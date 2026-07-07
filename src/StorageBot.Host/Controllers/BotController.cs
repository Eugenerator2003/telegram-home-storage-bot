using Microsoft.Extensions.Localization;
using StorageBot.Application.UseCases;
using StorageBot.Host.Routing;
using StorageBot.Host.Routing.Attributes.Actions.Required;
using StorageBot.Host.Utilities;
using StorageBot.Host.Presentation;
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
        protected IUserInputLinesStore InputLinesStore => _dependencies.LinesStore;
        protected IUserInputStateStore InputStateStore => _dependencies.StateStore;

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

        protected async Task ShowErrorMessage(string errorText)
        {
            var callback = GetCallbackDataForRequiredAction<MenuActionAttribute>();
            var message = new HtmlMessage(Localizer["View.ShowError", errorText]);
            message.OpenKeyboard();
            message.AddRowWithCallbackQueryButton(Localizer["View.BackToMenu"], callback);
            message.CloseKeyboard();
            await StartOrEditMenu(message);
        }

        protected CallbackData GetCallbackDataForAction<TController>(string actionName, int valuesCount = 0) where TController : BotController
        {
            var navigation = Navigation.GetAction<TController>(actionName);
            return new CallbackData(navigation.ControllerId, navigation.ActionId, valuesCount);
        }

        protected CallbackData GetCallbackDataForRequiredAction<TRequiredAction>(int valuesCount = 0) where TRequiredAction : RequiredActionAttribute
        {
            var navigation = RequiredNavigation.GetRequiredAction<TRequiredAction>();
            return new CallbackData(navigation.ControllerId, navigation.ActionId, valuesCount);
        }

        protected void SetRedirect<TController>(string actionName) where TController : BotController
        {
            var navigation = Navigation.GetAction<TController>(actionName);
            RedirectBuffer.SetRedirect(navigation.ControllerId, navigation.ControllerId);
        }

        protected async Task SetInputState<TController>(long chatId, string actionName) where TController : BotController
        {
            var navigation = Navigation.GetAction<TController>(actionName);
            var state = new InputState(navigation.ControllerId, navigation.ActionId);
            await InputStateStore.SetState(chatId, state);
        }

        protected async Task StartOrEditMenu(HtmlMessage message)
        {
            var menuId = await MenuStore.GetMenuMessageId(Context.Chat.Id);
            if (menuId == -1)
            {
                var sendedMessage = await BotClient.SendMessage(Context.Chat, message.ToString());
                await MenuStore.SetMenuMessageId(Context.Chat.Id, sendedMessage.Id);
            }
            else 
            {
                await BotClient.EditHtml(Context.Chat, menuId, message.ToString());
            }
        }

        protected async Task DeleteMenuIfExist()
        {
            var menuId = await MenuStore.GetMenuMessageId(Context.Chat.Id);
            if (menuId != -1)
            {
                await BotClient.DeleteMessage(Context.Chat, menuId);
            }
        }
    }
}
