using StorageBot.Host.Factories;
using StorageBot.Host.Routing;
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
        private readonly IIdNavigation navigation;

        public MessageDispatcher(IHandlerInfoResolver handlerInfoResolver, IControllerFactory controllerFactory, 
            IIdNavigation navigation, IUserInputStateStore userInputStateStore) 
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
            var handlerInfo = HandlerInfoResolver.GetHandlerInfoByCommand(((MessageBotContext)context).Text) 
                ?? HandlerInfoResolver.GetHandlerInfo(navigation.GetMainControllerId(), navigation.GetUnregisteredCommandActionId());
            return handlerInfo;
        }
    }
}
