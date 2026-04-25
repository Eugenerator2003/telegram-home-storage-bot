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
        protected BotContext Context { get; private set; } = null!;

        protected IStringLocalizer Localizer { get; private set; } = null!;

        public void SetBotContext(BotContext botContext)
        {
            Context = botContext;
        }

        public void SetLocalizer(IStringLocalizer localizer)
        {
            Localizer = localizer;
        }
    }
}
