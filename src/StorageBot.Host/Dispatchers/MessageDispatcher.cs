using StorageBot.Host.Factories;
using StorageBot.Host.Routing;
using StorageBot.Host.Routing.Attributes.Actions.Required;
using StorageBot.TelegramContracts.Account;
using StorageBot.TelegramContracts.Context;
using StorageBot.TelegramContracts.States.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace StorageBot.Host.Dispatchers
{
    public class MessageDispatcher : BotDispatcher
    {
        private readonly IUserInputStateStore inputStateStore;
        private readonly IRequiredNavigation navigation;
        private readonly ITelegramUserService userService;

        public MessageDispatcher(
            IHandlerInfoResolver handlerInfoResolver, 
            IControllerFactory controllerFactory,
            IRequiredNavigation navigation, 
            IUserInputStateStore userInputStateStore,
            ITelegramUserService telegramUserService)
            : base(handlerInfoResolver, controllerFactory)
        {
            inputStateStore = userInputStateStore;
            userService = telegramUserService;
            this.navigation = navigation;
        }

        protected override UpdateContext GetContext(Update update)
        {
            return new MessageUpdateContext(update);
        }

        protected override HandlerInfo GetHandlerInfo(UpdateContext context)
        {
            var user = userService.GetUser(context.Chat);
            var inputState = inputStateStore.GetState(user).Result;
            if (inputState != null)
            {
                inputStateStore.ClearState(user);
                return HandlerInfoResolver.GetHandlerInfo(inputState.ControllerId, inputState.MethodId);
            }
            HandlerInfo? handlerInfo = null!;
            var text = ((MessageUpdateContext)context).Message.Text;
            if (text is not null && text.StartsWith('/'))
            {
                text = text.Split(' ')[0];
                handlerInfo = HandlerInfoResolver.GetHandlerInfoByCommand(text);
                if (handlerInfo is null)
                {
                    var navigationInfo = navigation.GetRequiredAction<UnregisteredCommandActionAttribute>();
                    handlerInfo = HandlerInfoResolver.GetHandlerInfo(navigationInfo.ControllerId, navigationInfo.ActionId);
                }
            }
            if (handlerInfo is null)
            {
                var navigationInfo = navigation.GetRequiredAction<UnexpectedMessageActionAttribute>();
                handlerInfo = HandlerInfoResolver.GetHandlerInfo(navigationInfo.ControllerId, navigationInfo.ActionId);
            }
            return handlerInfo;
        }
    }
}
