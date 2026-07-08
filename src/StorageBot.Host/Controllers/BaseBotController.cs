using Microsoft.Extensions.Localization;
using StorageBot.Application.UseCases;
using StorageBot.Host.Presentation;
using StorageBot.Host.Routing;
using StorageBot.Host.Routing.Attributes.Actions.Required;
using StorageBot.Host.Utilities;
using StorageBot.TelegramContracts.Account;
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
    public abstract class BaseBotController
    {
        protected ITelegramBotClient BotClient => _dependencies.BotClient;
        protected IStringLocalizer Localizer => _dependencies.Localizer;
        protected INavigation Navigation => _dependencies.Navigation;
        protected IRequiredNavigation RequiredNavigation => _dependencies.RequiredNavigation;
        protected IRedirectBuffer RedirectBuffer => _dependencies.RedirectBuffer;

        protected UpdateContext Context { get; private set; } = null!;

        private ControllerDependencies _dependencies = null!;

        public virtual void SetDependencies(ControllerDependencies dependencies)
        {
            ArgumentNullException.ThrowIfNull(dependencies);
            _dependencies = dependencies;
        }

        public void SetContext(UpdateContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            Context = context;
        }

        public RedirectState? ConsumeRedirectState()
        {
            return RedirectBuffer.Consume();
        }

        protected virtual async Task ShowErrorMessage(string errorText)
        {
            var callback = GetCallbackDataForRequiredAction<MenuActionAttribute>();
            var message = new HtmlMessage(Localizer["View.ShowError", errorText]);
            message.OpenKeyboard();
            message.AddRowWithCallbackQueryButton(Localizer["View.BackToMenu"], callback);
            message.CloseKeyboard();
            await SendMessage(message);
        }

        protected CallbackData GetCallbackDataForAction<TController>(string actionName, int valuesCount = 0) where TController : BaseBotController
        {
            var navigation = Navigation.GetAction<TController>(actionName);
            return new CallbackData(navigation.ControllerId, navigation.ActionId, valuesCount);
        }

        protected CallbackData GetCallbackDataForRequiredAction<TRequiredAction>(int valuesCount = 0) where TRequiredAction : RequiredActionAttribute
        {
            var navigation = RequiredNavigation.GetRequiredAction<TRequiredAction>();
            return new CallbackData(navigation.ControllerId, navigation.ActionId, valuesCount);
        }

        protected void SetRedirect<TController>(string actionName) where TController : BaseBotController
        {
            var navigation = Navigation.GetAction<TController>(actionName);
            RedirectBuffer.SetRedirect(navigation.ControllerId, navigation.ControllerId);
        }

        protected async Task<int> SendMessage(HtmlMessage message)
        {
            return (await BotClient.SendMessage(Context.Chat, message.ToString())).Id;
        }

        protected async Task SendMessageWithDelete(HtmlMessage htmlMessage, int milliseconds)
        {
            var id = await SendMessage(htmlMessage);
            await DeleteMessage(id, milliseconds);
        }

        protected async Task DeleteMessage(int messageId, int milliseconds = 0)
        {
            await Task.Delay(milliseconds);
            await BotClient.DeleteMessage(Context.Chat, messageId);
        }

        protected async Task EditMessage(int messageId, HtmlMessage message)
        {
            await BotClient.EditHtml(messageId, messageId, message.ToString());
        }

        protected async Task AnswerCallbackQuery(string queryId)
        {
            await BotClient.AnswerCallbackQuery(queryId);
        }

        protected T ExtractContext<T>() where T : UpdateContext
        {
            if (Context is T context)
            {
                return context;
            }
            else
            {
                throw new InvalidOperationException($"Can't cast context to type {nameof(UpdateContext)}");
            }
        }
    }
}
