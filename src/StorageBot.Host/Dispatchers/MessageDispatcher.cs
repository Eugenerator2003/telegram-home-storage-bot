using StorageBot.Host.Factories;
using StorageBot.Host.Routing;
using StorageBot.Host.Routing.Attributes.Actions.Required;
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

        public MessageDispatcher(IHandlerInfoResolver handlerInfoResolver, IControllerFactory controllerFactory,
            IRequiredNavigation navigation, IUserInputStateStore userInputStateStore)
            : base(handlerInfoResolver, controllerFactory)
        {
            inputStateStore = userInputStateStore;
            this.navigation = navigation;
        }

        protected override BotContext GetContext(Update update)
        {
            return new MessageBotContext(update);
        }

        protected override HandlerInfo GetHandlerInfo(BotContext context)
        {
            var inputState = inputStateStore.GetState(context.Chat.Id).Result;
            if (inputState != null)
            {
                inputStateStore.ClearState(context.Chat.Id);
                return HandlerInfoResolver.GetHandlerInfo(inputState.ControllerId, inputState.MethodId);
            }
            HandlerInfo? handlerInfo = null!;
            var text = ((MessageBotContext)context).Message.Text;
            if (text is not null)
            {
                text = text.Split(' ')[0];
                handlerInfo = HandlerInfoResolver.GetHandlerInfoByCommand(text);
                if (handlerInfo is null)
                {
                    var navigationInfo = navigation.GetRequiredAction<UnregisteredCommandActionAttribute>();
                    handlerInfo = HandlerInfoResolver.GetHandlerInfo(navigationInfo.ControllerId, navigationInfo.ActionId);
                }
            }
            else
            {
                var navigationInfo = navigation.GetRequiredAction<UnexpectedMessageActionAttribute>();
                handlerInfo = HandlerInfoResolver.GetHandlerInfo(navigationInfo.ControllerId, navigationInfo.ActionId);
            }
            return handlerInfo;
        }
    }
}
