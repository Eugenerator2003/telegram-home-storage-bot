using Microsoft.Extensions.Localization;
using StorageBot.Application.UseCases;
using StorageBot.Host.Routing;
using StorageBot.TelegramContracts.Context;
using StorageBot.TelegramContracts.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace StorageBot.Host.Controllers
{
    public abstract class BotController
    {
        protected ITelegramBotClient BotClient => _dependencies.BotClient;

        protected IStringLocalizer Localizer => _dependencies.Localizer;

        protected INavigationIdProvider Navigation => _dependencies.Navigation;

        protected IMenuMessageProvider MenuProvider => _dependencies.MenuProvider;

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
    }
}
