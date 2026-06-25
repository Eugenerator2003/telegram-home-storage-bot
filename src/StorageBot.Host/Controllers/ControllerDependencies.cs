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

        public INavigation Navigation { get; }

        public IRequiredNavigation RequiredNavigation { get; }

        public IMenuMessageStore MenuStore { get; }

        public IRedirectBuffer RedirectBuffer { get; }

        public ControllerDependencies(
            ITelegramBotClient botClient,
            IStringLocalizer localizer, 
            INavigation navigation,
            IRequiredNavigation requiredNavigation,
            IMenuMessageStore menuStore,
            IRedirectBuffer stateBuffer)
        {
            ArgumentNullException.ThrowIfNull(botClient);
            ArgumentNullException.ThrowIfNull(localizer);
            ArgumentNullException.ThrowIfNull(navigation);
            ArgumentNullException.ThrowIfNull(menuStore);
            ArgumentNullException.ThrowIfNull(requiredNavigation);
            ArgumentNullException.ThrowIfNull(stateBuffer);
            BotClient = botClient;
            Localizer = localizer;
            Navigation = navigation;
            RequiredNavigation = requiredNavigation;
            MenuStore = menuStore;
            RedirectBuffer = stateBuffer;
        }
    }
}
