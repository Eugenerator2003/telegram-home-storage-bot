using Microsoft.Extensions.Localization;
using StorageBot.Host.Routing;
using StorageBot.TelegramContracts.States.Chat;
using StorageBot.TelegramContracts.States.ControllerState;
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

        public IIdNavigation Navigation { get; }

        public IMenuMessageStore MenuStore { get; }

        public IRedirectBuffer RedirectBuffer { get; }

        public ControllerDependencies(
            ITelegramBotClient botClient,
            IStringLocalizer localizer, 
            IIdNavigation navigation, 
            IMenuMessageStore menuStore,
            IRedirectBuffer stateBuffer)
        {
            ArgumentNullException.ThrowIfNull(botClient);
            ArgumentNullException.ThrowIfNull(localizer);
            ArgumentNullException.ThrowIfNull(navigation);
            ArgumentNullException.ThrowIfNull(menuStore);
            ArgumentNullException.ThrowIfNull(stateBuffer);
            BotClient = botClient;
            Localizer = localizer;
            Navigation = navigation;
            MenuStore = menuStore;
            RedirectBuffer = stateBuffer;
        }
    }
}
