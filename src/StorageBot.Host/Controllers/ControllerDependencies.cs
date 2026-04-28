using Microsoft.Extensions.Localization;
using StorageBot.Host.Routing;
using StorageBot.TelegramContracts.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace StorageBot.Host.Controllers
{
    public class ControllerDependencies
    {
        public ITelegramBotClient BotClient { get; }

        public IStringLocalizer Localizer { get; }

        public INavigationIdProvider Navigation { get; }

        public IMenuMessageProvider MenuProvider { get; }

        public ControllerDependencies(
            ITelegramBotClient botClient,
            IStringLocalizer localizer, 
            INavigationIdProvider navigation, 
            IMenuMessageProvider menuProvider)
        {
            ArgumentNullException.ThrowIfNull(botClient);
            ArgumentNullException.ThrowIfNull(localizer);
            ArgumentNullException.ThrowIfNull(navigation);
            ArgumentNullException.ThrowIfNull(menuProvider);
            BotClient = botClient;
            Localizer = localizer;
            Navigation = navigation;
            MenuProvider = menuProvider;
        }
    }
}
