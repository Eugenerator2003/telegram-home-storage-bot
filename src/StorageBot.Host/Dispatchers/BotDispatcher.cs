using Microsoft.Extensions.Localization;
using StorageBot.Host.Factories;
using StorageBot.Host.Routing;
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
        protected IControllerTypeProvider ControllerProvider { get; }

        protected IControllerFactory ControllerFactory { get; }

        protected IStringLocalizer Localizer { get; }

        public BotDispatcher(
            IControllerTypeProvider controllerProvider, 
            IControllerFactory controllerFactory, 
            IStringLocalizer localizer)
        {
            ControllerProvider = controllerProvider;
            ControllerFactory = controllerFactory;
            Localizer = localizer;
        }

        public abstract Task DispatchUpdate(Update update);
    }
}
