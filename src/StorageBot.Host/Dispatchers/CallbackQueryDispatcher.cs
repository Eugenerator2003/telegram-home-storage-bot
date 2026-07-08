using StorageBot.Host.Factories;
using StorageBot.Host.Routing;
using StorageBot.Host.Routing.Attributes.Actions.Required;
using StorageBot.Host.Utilities;
using StorageBot.TelegramContracts.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace StorageBot.Host.Dispatchers
{
    public class CallbackQueryDispatcher : BotDispatcher
    {
        private readonly IRequiredNavigation navigation;

        public CallbackQueryDispatcher(IHandlerInfoResolver handlerInfoResolver, IControllerFactory controllerFactory,
            IRequiredNavigation navigation) 
            : base(handlerInfoResolver, controllerFactory)
        {
            this.navigation = navigation;
        }

        protected override UpdateContext GetContext(Update update)
        {
            return new CallbackQueryUpdateContext(update);
        }

        protected override HandlerInfo GetHandlerInfo(UpdateContext context)
        {
            var queryContext = (CallbackQueryUpdateContext)context;
            var callbackData = CallbackDataEncoder.Decode(queryContext.Data);
            HandlerInfo handler;
            try
            {
                handler = HandlerInfoResolver.GetHandlerInfo(callbackData.ControllerId, callbackData.ActionId);
            }
            catch
            {
                var action = navigation.GetRequiredAction<UnknownCallbackQueryActionAttribute>();
                handler = HandlerInfoResolver.GetHandlerInfo(action.ControllerId, action.ActionId);
            }
            return handler;
        }
    }
}
