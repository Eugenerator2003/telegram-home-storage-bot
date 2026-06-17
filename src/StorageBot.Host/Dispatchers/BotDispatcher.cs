using Microsoft.Extensions.Localization;
using StorageBot.Host.Factories;
using StorageBot.Host.Routing;
using StorageBot.TelegramContracts.Context;
using StorageBot.TelegramContracts.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace StorageBot.Host.Dispatchers
{
    public abstract class BotDispatcher
    {
        protected readonly static int MaxRedirects = 10;

        protected IHandlerInfoResolver ControllerProvider { get; }

        protected IControllerFactory ControllerFactory { get; }


        public BotDispatcher(
            IHandlerInfoResolver controllerProvider, 
            IControllerFactory controllerFactory)
        {
            ControllerProvider = controllerProvider;
            ControllerFactory = controllerFactory;
        }

        public async Task DispatchUpdate(Update update)
        {
            var context = GetContext(update);
            var pair = GetControllerWithAction(update);
            var state = await InvokeWithContext(pair, context);
            int counter = MaxRedirects;
            while (state != null && counter > 0)
            {
                pair = ControllerProvider.GetHandlerInfo(state.ControllerId, state.ActionId);
                state = await InvokeWithContext(pair, context);
                counter--;
            }
            if (state != null)
            {
                throw new InvalidOperationException(
                    $"Maximum redirect limit of {MaxRedirects} was exceeded while dispatching update.");
            }
        }

        protected async Task<RedirectState?> InvokeWithContext(HandlerInfo pair, BotContext context)
        {
            var controller = ControllerFactory.GetController(pair.ControllerType);
            controller.SetContext(context);
            await (Task)pair.ActionInvoker.Invoke(controller)!;
            return controller.ConsumeRedirectState();
        }
        protected abstract HandlerInfo GetControllerWithAction(Update update);

        protected abstract BotContext GetContext(Update update);
    }
}
