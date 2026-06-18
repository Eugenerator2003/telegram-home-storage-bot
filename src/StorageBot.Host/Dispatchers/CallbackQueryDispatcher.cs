using StorageBot.Host.Factories;
using StorageBot.Host.Routing;
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
        private readonly IIdNavigation navigation;

        public CallbackQueryDispatcher(IHandlerInfoResolver handlerInfoResolver, IControllerFactory controllerFactory,
            IIdNavigation navigation) 
            : base(handlerInfoResolver, controllerFactory)
        {
            this.navigation = navigation;
        }

        protected override BotContext GetContext(Update update)
        {
            return new CallbackQueryBotContext(update);
        }

        protected override HandlerInfo GetHandlerInfo(BotContext context)
        {
            var queryContext = (CallbackQueryBotContext)context;
            var callbackData = CallbackDataEncoder.Decode(queryContext.Data);
            HandlerInfo handler;
            try
            {
                handler = HandlerInfoResolver.GetHandlerInfo(callbackData.ControllerId, callbackData.ActionId);
            }
            catch
            {
                handler = HandlerInfoResolver.GetHandlerInfo(navigation.GetMainControllerId(), navigation.GetUnknownCallbackQueryActionId());
            }
            return handler;
        }
    }
}
