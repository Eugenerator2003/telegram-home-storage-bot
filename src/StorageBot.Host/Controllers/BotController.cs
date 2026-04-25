using Microsoft.Extensions.Localization;
using StorageBot.Host.Routing;
using StorageBot.TelegramContracts.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace StorageBot.Host.Controllers
{
    public abstract class BotController
    {
        protected IStringLocalizer Localizer => _dependencies.Localizer;

        protected INavigationIdProvider Navigation => _dependencies.Navigation;

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
