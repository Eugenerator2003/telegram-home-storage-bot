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
        private readonly Dictionary<string, HandlerInfo> commandHandlers;
        private readonly IIdNavigation navigation;

        public MessageDispatcher(IHandlerInfoResolver handlerInfoResolver, IControllerFactory controllerFactory, 
            IIdNavigation navigation, IUserInputStateStore userInputStateStore) 
            : base(handlerInfoResolver, controllerFactory)
        {
            inputStateStore = userInputStateStore;
            this.navigation = navigation;
            commandHandlers = new Dictionary<string, HandlerInfo>();
        }

        public void RegisterCommandHandler(string command, HandlerInfo handlerInfo)
        {
            if (string.IsNullOrEmpty(command) || !command.StartsWith('/'))
            {
                throw new ArgumentException($"Command {command} must start with '/'", nameof(command));
            }
            if (commandHandlers.TryGetValue(command, out var _))
            {
                throw new InvalidOperationException($"Handler for command {command} is already registered");
            }
            commandHandlers[command] = handlerInfo;
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
                return HandlerInfoResolver.GetHandlerInfo(inputState.ControllerId, inputState.ControllerId);
            }
            if (commandHandlers.TryGetValue(((MessageBotContext)context).Text, out var handlerInfo)) {
                return handlerInfo;   
            }
            return HandlerInfoResolver.GetHandlerInfo(navigation.GetMainControllerId(), navigation.GetUnregisteredCommandActionId());
        }
    }
}
