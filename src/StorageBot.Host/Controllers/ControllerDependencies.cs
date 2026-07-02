using Microsoft.Extensions.Localization;
using StorageBot.Host.Routing;
using StorageBot.TelegramContracts.Application;
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

        public IUserInputStateStore StateStore { get; }

        public IUserInputLinesStore LinesStore { get; }

        public ICurrentOwnershipStore CurrentOwnershipStore { get; }

        public ITelegramUserService TelegramUserService { get; }

        public ControllerDependencies(
            ITelegramBotClient botClient,
            IStringLocalizer localizer, 
            INavigation navigation,
            IRequiredNavigation requiredNavigation,
            IMenuMessageStore menuStore,
            IRedirectBuffer stateBuffer,
            IUserInputLinesStore linesStore, 
            IUserInputStateStore stateStore,
            ICurrentOwnershipStore ownershipStore,
            ITelegramUserService telegramUserService)
        {
            BotClient = botClient ?? throw new ArgumentNullException(nameof(botClient));
            Localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            Navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            RequiredNavigation = requiredNavigation ?? throw new ArgumentNullException(nameof(requiredNavigation));
            MenuStore = menuStore ?? throw new ArgumentNullException(nameof(menuStore));
            RedirectBuffer = stateBuffer ?? throw new ArgumentNullException(nameof(stateBuffer));
            StateStore = stateStore ?? throw new ArgumentNullException(nameof(stateStore));
            LinesStore = linesStore ?? throw new ArgumentNullException(nameof(linesStore));
            CurrentOwnershipStore = ownershipStore ?? throw new ArgumentNullException(nameof(ownershipStore));
            TelegramUserService = telegramUserService ?? throw new ArgumentNullException(nameof(telegramUserService));
        }
    }
}
